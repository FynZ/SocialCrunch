using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using Models;
using Models.Enums;
using Npgsql;

namespace Data
{
    public class TokenRepository : ITokenRepository
    {
        private readonly string _connectionString;

        public TokenRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected virtual IDbConnection Connection => new NpgsqlConnection(_connectionString);

        public async Task<IEnumerable<Token>> GetTokens()
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Token>> GetTokens(SocialNetworkType type)
        {
            using (var con = Connection)
            {
                const string sql =
                    @"SELECT 
                        user_id AS UserId,
                        access_token AS AccessToken,
                        token_secret AS TokenSecret 
                    FROM 
                        t_social_networks 
                    WHERE 
                        social_network_type = @type";

                con.Open();

                return await con.QueryAsync<Token>(sql, new {Type = (int)type});
            }
        }

        public async Task<object> GetTokensForUser(int userId)
        {
            throw new NotImplementedException();
        }

        public async Task<Token> GetToken(int tokenId)
        {
            throw new NotImplementedException();
        }

        public async Task<Token> GetToken(SocialNetworkType type, int userId)
        {
            throw new NotImplementedException();
        }

        public async Task<Token> GetTokenForUser(SocialNetworkType type, int userId)
        {
            using (var con = Connection)
            {
                const string sql =
                    @"SELECT 
                        user_id AS UserId,
                        access_token AS AccessToken,
                        token_secret AS TokenSecret 
                    FROM 
                        t_social_networks 
                    WHERE
                        user_id = @userId
                    AND social_network_type = @type";

                con.Open();

                return await con.QueryFirstAsync<Token>(sql, new { Type = (int)type, UserId = userId });
            }
        }
    }
}
