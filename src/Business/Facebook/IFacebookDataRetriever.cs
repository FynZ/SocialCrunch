using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Models.Facebook;

namespace Business.Facebook
{
    public interface IFacebookDataRetriever : IDailyCollect<FacebookCompleteData>
    {
        bool IsSameUser(string token);

        Task<FacebookUser> GetProfile();
        Task<object> GetFeed();
        Task<object> GetPostDetails(int postId);
        Task<object> GetUserComments();
    }
}
