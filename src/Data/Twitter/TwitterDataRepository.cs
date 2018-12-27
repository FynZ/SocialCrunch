using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Models.Twitter;

namespace Data.Twitter
{
    public class TwitterDataRepository : BaseRepository, ITwitterDataRepository
    {
        public TwitterDataRepository(string connectionString) : base(connectionString) { }

        public async Task<TwitterDailyData> GetDailyData(int userId, DateTime? day = null)
        {
            var date = day?.Date ?? DateTime.Today;

            using (var con = Connection)
            {
                const string sql =
                    @"
                    SELECT
                        id          AS Id,
                        followers   AS Followers,
                        likes       AS Likes,
                        retweets    AS Retweets,
                        replies     AS Replies
                    FROM
                        t_twiter_data
                    WHERE
                        user_id = @userId
                    AND
                        date = @date
                    ";

                con.Open();

                return await con.QueryFirstOrDefaultAsync<TwitterDailyData>(sql, new
                {
                    UserId = userId,
                    Date = date
                });
            }
        }

        public async Task<bool> InsertDailyData(TwitterDailyData data, int userId)
        {
            using (var con = Connection)
            {
                const string sql =
                    @"DO $$                  
                    BEGIN
                        IF EXISTS
                        (
                            SELECT
                                1
                            FROM
                                t_twitter_data
                            WHERE
                                user_id = @userId
                            AND
                                date = @date;
                        )
                        THEN
                            UPDATE 
                                t_twitter_data
                            SET 
                                followers = @followers, 
                                likes = @likes, 
                                retweets = @retweets, 
                                replies = @replies
                            WHERE 
                                user_id = user_id;
                            AND 
                                date = @date;
                        ELSE
                            INSERT INTO public.t_twitter_data
                            (
                                user_id, 
                                date, 
                                followers, 
                                likes, 
                                retweets, 
                                replies
                            )
                            VALUES
                            (
                                @userId, 
                                @date,
                                @followers, 
                                @likes, 
                                @retweets, 
                                @likes
                            );
                        END IF;
                    END
                    $$;";

                con.Open();

                return await con.ExecuteAsync(sql, new
                {
                    UserId = userId,
                    Date = DateTime.Today,
                    Followers = data.Followers,
                    Likes = data.Likes,
                    Retweets = data.Retweets,
                    Replies = data.Replies
                }) != 0;
            }
        }

        public async Task<bool> InsertDailySummary(TwitterDailySummary data, int userId)
        {
            using (var con = Connection)
            {
                const string sql =
                    @"DO $$                  
                    BEGIN
                        IF EXISTS
                        (
                            SELECT 
                                1
                            FROM
                                t_twitter_summary
                            WHERE
                                user_id = @userId
                            AND
                                date = @date;
                        )
                        THEN
                            UPDATE 
                                t_twitter_data
                            SET 
                                followers = @followers,
                                followings = @followings,
                                friends = @friends,
                                tweets = @tweets,
                                likes = @likes
                            WHERE
                                user_id = user_id;
                            AND
                                date = @date;
                        ELSE
                            INSERT INTO public.t_twitter_data
                            (
                                user_id,
                                date,
                                followers,
                                followings,
                                friends,
                                tweets,
                                likes
                            )
                            VALUES
                            (
                                @userId,
                                @date,
                                @followers,
                                @followings,
                                @friends,
                                @tweets,
                                @likes
                            );
                        END IF;
                    END
                    $$;";

                con.Open();

                return await con.ExecuteAsync(sql, new
                {
                    UserId = userId,
                    Date = DateTime.Today,
                    Followers = data.Followers,
                    Followings = data.Followings,
                    Friends = data.Friends,
                    Tweets = data.Tweets,
                    Likes = data.Likes
                }) != 0;
            }
        }

        public async Task<bool> InsertBestDailyTweets(IEnumerable<Tweet> tweets, int userId)
        {
            using (var con = Connection)
            {
                const string sql =
                    @"DO $$                  
                    BEGIN
                        IF EXISTS
                        (
                            SELECT
                                1
                            FROM
                                t_best_daily_tweets
                            WHERE
                                user_id = @userId
                            AND
                                date = @date
                        )
                        THEN
                            DELETE
                            FROM
                                t_best_daily_tweets
                            WHERE
                                user_id = @userId
                            AND
                                date = @date
                        END IF;

                        INSERT INTO public.t_best_daily_tweets
                        (
                            user_id
                            date,
                            tweet_id,
                            tweet_user_id, 
                            creation_date,
                            content, 
                            reply_count, 
                            quote_count, 
                            retweet_count, 
                            like_count
                        )
                        VALUES
                        (
                            @userId
                            @date,
                            @tweetId,
                            @tweetUserId, 
                            @creationDate, 
                            @content, 
                            @replyCount, 
                            @quoteCount,
                            @retweetCount, 
                            @likeCount
                        );
                    END
                    $$;";

                con.Open();

                foreach (var tweet in tweets)
                {
                    await con.ExecuteAsync(sql, new
                    {
                        UserId = userId,
                        Date = DateTime.Today,
                        TweetId = tweet.Id,
                        TweetUserId = tweet.UserId,
                        CreationDate = tweet.CreationDate,
                        Content = tweet.Content,
                        ReplyCount = tweet.ReplyCount,
                        QuoteCount = tweet.QuoteCount,
                        RetweetCount = tweet.RetweetCount,
                        LikeCount = tweet.LikeCount,

                    });
                }

                return true;
            }
        }

        public Task<bool> InsertBestTweets(IEnumerable<Tweet> tweets, int userId)
        {
            throw new NotImplementedException();
        }
    }
}
