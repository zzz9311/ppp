using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetPPP.BLL.Interfaces
{
    public interface IRefreshTokenService
    {
        Task SetRefreshTokenToUserAsync(Guid id, string refreshToken, CancellationToken token);
    }
}
