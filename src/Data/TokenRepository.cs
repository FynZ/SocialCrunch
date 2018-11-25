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
    class TokenRepository // : BaseRepository
    {
        private readonly string _connectionString;

        public TokenRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected virtual IDbConnection Connection => new NpgsqlConnection("_connectionString");

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
                    WHERE social_network_type = @type";

                con.Open();

                return con.Query<Token>(sql, new {Type = (int)type});
            }
        }
    }
}
