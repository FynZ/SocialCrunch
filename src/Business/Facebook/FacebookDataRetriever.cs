using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Facebook;
using Models.Facebook;

namespace Business.Facebook
{
    public class FacebookDataRetriever
    {
        private readonly FacebookClient _facebook;
        private readonly string _token;

        public FacebookDataRetriever(string token)
        {
            _token = token;
            _facebook = new FacebookClient(token);
        }

        public bool IsSameUser(string token)
        {
            return String.CompareOrdinal(token, _token) == 0;
        }

        public FacebookUser GetProfile()
        {
            return _facebook.Get<FacebookUser>("me?fields=about,id,address,age_range,birthday,education,email,favorite_teams,first_name,gender,hometown,last_name,location,middle_name,name");
        }

        public object GetFeed()
        {
            return _facebook.Get("me/feed");
        }

        public object GetPostDetails(int postId)
        {
            return _facebook.Get($"{postId}?fields=shares,likes.summary(true),comments.summary(true)");
        }

        public async Task<object> GetUserComments()
        {
            return await _facebook.GetTaskAsync($"me/comments?fields=id,created_time,from,like_count");
        }
    }
}
