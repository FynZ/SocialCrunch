using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Models.Twitter;

namespace Data
{
    class TwitterDataRepository : BaseRepository
    {
        public TwitterDataRepository(string connectionString) : base(connectionString) { }

        public async Task<bool> InsertDailyData(TwitterDailyData data, int userId)
        {
            using (var con = Connection)
            {
                const string sql =
                    @"DO $$                  
                    BEGIN
                        IF EXISTS
                        (
                            SELECT 1
                            FROM   t_twitter_data
                            WHERE  user_id = @user_id
                            AND    date > now()::date
                        )
                        THEN
                            UPDATE t_twitter_data
                            SET followers = @followers, likes = @likes, retweets = @retweets, replies = @replies
                            WHERE user_id = user_id;
                            AND date > now()::date;
                        ELSE
                            INSERT INTO public.t_twitter_data(
                                user_id, date, followers, likes, retweets, replies)
                            VALUES
                                (@userId, CURRENT_TIMESTAMP, @followers, @likes, @retweets, @likes);
                        END IF;
                    END
                    $$;";

                con.Open();

                return await con.ExecuteAsync(sql, new
                {
                    UserId = userId,
                    Followers = 2,
                    Likes = data.Likes,
                    Retweets = data.Retweets,
                    Replies = data.Replies
                }) != 0;
            }
        }
    }
}
