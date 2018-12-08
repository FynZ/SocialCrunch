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
    public class TokenRepository : BaseRepository, ITokenRepository
    {
        public TokenRepository(string connectionString) : base(connectionString) { }

        public async Task<IEnumerable<Token>> GetTokens()
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

                return await con.QueryAsync<Token>(sql);
            }
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

        public async Task<IEnumerable<Token>> GetTokensForUser(int userId)
        {
            using (var con = Connection)
            {
                const string sql =
                    @"SELECT 
                        id AS Id
                        user_id AS UserId,
                        access_token AS AccessToken,
                        token_secret AS TokenSecret 
                    FROM 
                        t_social_networks 
                    WHERE 
                        user_id = @userId";

                con.Open();

                return await con.QueryAsync<Token>(sql, new { UserId = userId });
            }
        }

        public async Task<Token> GetToken(int id)
        {
            using (var con = Connection)
            {
                const string sql =
                    @"SELECT 
                        id AS Id
                        user_id AS UserId,
                        access_token AS AccessToken,
                        token_secret AS TokenSecret 
                    FROM 
                        t_social_networks 
                    WHERE 
                        id = @id";

                con.Open();

                return await con.QueryFirstOrDefaultAsync<Token>(sql, new { Id = id });
            }
        }

        public async Task<Token> GetToken(SocialNetworkType type, int id)
        {
            using (var con = Connection)
            {
                const string sql =
                    @"SELECT 
                        id AS Id
                        user_id AS UserId,
                        access_token AS AccessToken,
                        token_secret AS TokenSecret 
                    FROM 
                        t_social_networks 
                    WHERE 
                        social_network_type = @type
                        And id = @id";

                con.Open();

                return await con.QueryFirstOrDefaultAsync<Token>(sql, new { Type = (int)type, Id = id });
            }
        }

        public async Task<Token> GetTokenForUser(SocialNetworkType type, int userId)
        {
            using (var con = Connection)
            {
                const string sql =
                    @"SELECT 
                        id AS Id
                        user_id AS UserId,
                        access_token AS AccessToken,
                        token_secret AS TokenSecret 
                    FROM 
                        t_social_networks 
                    WHERE
                        user_id = @userId
                    AND social_network_type = @type";

                con.Open();

                return await con.QueryFirstOrDefaultAsync<Token>(sql, new { Type = (int)type, UserId = userId });
            }
        }

        public async Task<bool> RevokeToken(int id)
        {
            using (var con = Connection)
            {
                const string sql =
                    @"UPDATE 
                        t_social_networks 
                      SET is_revoked = true
                    WHERE 
                        id = @id";

                con.Open();

                return (await con.ExecuteAsync(sql, new { Id = id })) != 0;
            }
        }
    }
}
