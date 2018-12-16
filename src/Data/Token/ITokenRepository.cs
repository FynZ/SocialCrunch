using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Models;
using Models.Enums;

namespace Data.Token
{
    public interface ITokenRepository
    {
        Task<IEnumerable<SocialToken>> GetTokens();
        Task<IEnumerable<SocialToken>> GetTokens(SocialNetworkType type);
        Task<IEnumerable<SocialToken>> GetTokensForUser(int userId);

        Task<SocialToken> GetToken(int tokenId);
        Task<SocialToken> GetToken(SocialNetworkType type, int userId);
        Task<SocialToken> GetTokenForUser(SocialNetworkType type, int userId);

        Task<bool> RevokeToken(int id);
    }
}
