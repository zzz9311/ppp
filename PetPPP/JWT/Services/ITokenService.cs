using System.Security.Claims;

namespace PetPPP.JWT.Services
{
    public interface ITokenService
    {
        string GenerateAccessToken(Guid id);
        string GenerateRefreshToken();
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    }
}
