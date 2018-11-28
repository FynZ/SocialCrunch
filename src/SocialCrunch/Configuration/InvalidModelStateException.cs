using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace SocialCrunch.Configuration
{
    public class InvalidModelStateException : Exception
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="InvalidModelStateException"/> class.
        /// </summary>
        /// <param name="modelState"> The invalid model state. </param>
        public InvalidModelStateException(ModelStateDictionary modelState)
        {
            ModelState = modelState ?? throw new ArgumentNullException(nameof(modelState));
        }

        /// <summary>
        ///     Error code
        /// </summary>
        public string Code => "Data validation failed";

        /// <summary>
        ///     The invalid model state.
        /// </summary>
        public ModelStateDictionary ModelState { get; }

        /// <inheritdoc />
        public override string ToString()
        {
            string msg = $"{Message} [Code: {Code}]";

            foreach (var keyModelStatePair in ModelState)
            {
                var key = keyModelStatePair.Key;
                var errors = keyModelStatePair.Value.Errors;
                if (errors != null && errors.Count > 0)
                {
                    errors.ToList().ForEach(error =>
                    {
                        msg += string.IsNullOrEmpty(key) ? Environment.NewLine + $"\t- {error.ErrorMessage}"
                            : Environment.NewLine + $"\t- {key}: {error.ErrorMessage}";
                    });
                }
            }

            return msg;
        }
    }
}
