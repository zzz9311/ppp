using DAL.Entities;
using PetPPP.BLL.Interfaces.DTO;

namespace PetPPP.BLL.Interfaces
{
    public interface IUserService
    {
        Task<AppUser> GetUserByIdAsync(Guid id, CancellationToken token);
        Task AddUserAsync(AppUserDTO user, CancellationToken token);
        Task<bool> EditUserAsync(AppUserDTO userDTO, Guid id, CancellationToken token);
        Task<Guid> LoginUserAsync(LoginDTO user, CancellationToken token);
        Task<Guid> GetUserIdByUsername(string username, CancellationToken token);

    }
}
