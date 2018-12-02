using System;
using System.Collections.Generic;
using System.Text;
using Facebook;
using Models.Facebook;

namespace Business
{
    public class FacebookDataRetriever
    {
        private readonly FacebookClient _facebook;

        public FacebookDataRetriever(String token)
        {
            _facebook = new FacebookClient(token);
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

        public object GetUserComments()
        {
            return _facebook.Get($"me/comments?fields=id,created_time,from,like_count");
        }
    }
}
