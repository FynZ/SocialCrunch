﻿using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Core;
using Serilog.Core.Enrichers;
using Serilog.Events;

namespace SocialCrunch.Configuration.Middleware
{
    /// <summary>
    /// Logging middleware based on Serilog.
    /// </summary>
    public class SerilogMiddleware
    {
        private const string MessageTemplate = "HTTP {RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed:0.0000} ms";
        private static readonly ILogger Log = Serilog.Log.ForContext<SerilogMiddleware>();
        private readonly RequestDelegate _next;
        private readonly bool _enableHttpRequestBodyLogging;

        /// <summary>
        /// Initializes a new instance of the <see cref="SerilogMiddleware"/> class.
        /// </summary>
        /// <param name="next">Next request delegate in the pipeline.</param>
        /// <param name="configuration">
        ///     Set of key/value application configuration properties. 
        ///     When EnableHttpRequestBodyLogging == true, log the <see cref="HttpRequest.Body"/>, otherwise not. Default: true.
        /// </param>
        public SerilogMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _enableHttpRequestBodyLogging = configuration?.GetValue("EnableHttpRequestBodyLogging", true) ?? true;
        }

        /// <summary>
        /// Middleware core implementation.
        /// </summary>
        /// <param name="httpContext">The current HTTP context.</param>
        /// <returns>Next task in the pipeline.</returns>
        public async Task Invoke(HttpContext httpContext)
        {
            if (httpContext == null)
            {
                throw new ArgumentNullException(nameof(httpContext));
            }

            var start = Stopwatch.GetTimestamp();
            var request = httpContext.Request;

            try
            {
                await _next(httpContext);

                var statusCode = httpContext.Response?.StatusCode;
                var logLevel = statusCode > 499 ? LogEventLevel.Error : LogEventLevel.Information;
                var elapsedMs = GetElapsedMilliseconds(start, Stopwatch.GetTimestamp());
                var log = await CreateEnrichedLoggerAsync(httpContext.Request);
                log.Write(logLevel, MessageTemplate, request.Method, request.Path, statusCode, elapsedMs);
            }
            catch (Exception ex)
            {
                var elapsedMs = GetElapsedMilliseconds(start, Stopwatch.GetTimestamp());
                var log = await CreateEnrichedLoggerAsync(httpContext.Request);
                log.Write(LogEventLevel.Error, ex, MessageTemplate, request.Method, request.Path, (int)HttpStatusCode.InternalServerError, elapsedMs);
                throw;
            }
        }

        /// <summary>
        ///     Creates a logger enriched with additionnal data from the <paramref name="request"/>.
        /// </summary>
        /// <remarks> If an exception occurs, it returns a simple, not enriched, valid logger. </remarks>
        /// <param name="request"> The current http request. </param>
        /// <returns> Always returns a new valid logger. </returns>
        private async Task<ILogger> CreateEnrichedLoggerAsync(HttpRequest request)
        {
            try
            {
                var logEventEnrichers = new Collection<ILogEventEnricher>
                {
                    new PropertyEnricher("RequestHeaders", request.Headers.ToDictionary(h => h.Key, h => h.Value.ToString()), destructureObjects: true),
                    new PropertyEnricher("RequestQueryString", request.Query.ToDictionary(v => v.Key, v => v.Value.ToString()), destructureObjects: true),
                    new PropertyEnricher("RemoteIpAddress", request.HttpContext.Connection.RemoteIpAddress)
                };

                if (_enableHttpRequestBodyLogging && request.ContentLength > 0)
                {
                    // Log the request body
                    var bodyContent = await ReadBodyAsStringAsync(request) ?? string.Empty;
                    logEventEnrichers.Add(new PropertyEnricher("Request.Body", bodyContent));
                }

                return Log.ForContext(logEventEnrichers);
            }
            catch
            {
                return Log.ForContext<SerilogMiddleware>();
            }
        }

        private static async Task<string> ReadBodyAsStringAsync(HttpRequest request)
        {
            try
            {
                using (var bodyReader = new StreamReader(request.Body))
                {
                    string bodyAsText = await bodyReader.ReadToEndAsync();
                    // replace the stream of the request body, by a new one at position = 0
                    var bytesToWrite = Encoding.UTF8.GetBytes(bodyAsText);
                    var injectedStream = new MemoryStream();
                    await injectedStream.WriteAsync(bytesToWrite, 0, bytesToWrite.Length);
                    injectedStream.Position = 0;
                    request.Body = injectedStream;

                    return bodyAsText;
                }
            }
            catch (Exception ex)
            {
                return $"Error retrieving information from the http request body. {ex.Message}";
            }
        }

        private static double GetElapsedMilliseconds(long start, long stop) => (stop - start) * 1000 / (double)Stopwatch.Frequency;
    }
}