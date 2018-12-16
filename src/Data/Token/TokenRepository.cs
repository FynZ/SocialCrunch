using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using Models;
using Models.Enums;
using Npgsql;

namespace Data.Token
{
    public class TokenRepository : BaseRepository, ITokenRepository
    {
        public TokenRepository(string connectionString) : base(connectionString) { }

        public async Task<IEnumerable<SocialToken>> GetTokens()
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

                return await con.QueryAsync<SocialToken>(sql);
            }
        }

        public async Task<IEnumerable<SocialToken>> GetTokens(SocialNetworkType type)
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

                return await con.QueryAsync<SocialToken>(sql, new {Type = (int)type});
            }
        }

        public async Task<IEnumerable<SocialToken>> GetTokensForUser(int userId)
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

                return await con.QueryAsync<SocialToken>(sql, new { UserId = userId });
            }
        }

        public async Task<SocialToken> GetToken(int id)
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

                return await con.QueryFirstOrDefaultAsync<SocialToken>(sql, new { Id = id });
            }
        }

        public async Task<SocialToken> GetToken(SocialNetworkType type, int id)
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

                return await con.QueryFirstOrDefaultAsync<SocialToken>(sql, new { Type = (int)type, Id = id });
            }
        }

        public async Task<SocialToken> GetTokenForUser(SocialNetworkType type, int userId)
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

                return await con.QueryFirstOrDefaultAsync<SocialToken>(sql, new { Type = (int)type, UserId = userId });
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

                return await con.ExecuteAsync(sql, new { Id = id }) != 0;
            }
        }
    }
}
