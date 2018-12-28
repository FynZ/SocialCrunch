using System;
using System.Collections.Generic;
using System.Text;
using Models.Facebook;

namespace Business.Facebook
{
    public interface IFacebookDataRetriever : IDailyCollect<FacebookCompleteData>
    {
    }
}
