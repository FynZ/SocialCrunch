using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Models;
using Models.Enums;

namespace Data
{
    public interface ITokenRepository
    {
        Task<IEnumerable<Token>> GetTokens();
        Task<IEnumerable<Token>> GetTokens(SocialNetworkType type);
        Task<IEnumerable<Token>> GetTokensForUser(int userId);

        Task<Token> GetToken(int tokenId);
        Task<Token> GetToken(SocialNetworkType type, int userId);
        Task<Token> GetTokenForUser(SocialNetworkType type, int userId);

        Task<bool> RevokeToken(int id);
    }
}
