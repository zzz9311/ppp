using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetPPP.BLL.Interfaces
{
    public interface IRefreshTokenService
    {
        Task SetRefreshTokenToUserAsync(Guid id, string refreshToken, string deviceInfo, CancellationToken token);
        Task<UsersRefreshToken> GetUserRefreshTokenAsync(Guid userId, string deviceInfo, CancellationToken token);
        Task<bool> RevokeUserRefreshTokenAsync(Guid userId, string deviceInfo, CancellationToken token);
    }
}
