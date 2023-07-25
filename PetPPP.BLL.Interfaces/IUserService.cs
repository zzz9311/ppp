using DAL.Entities;
using PetPPP.BLL.Interfaces.DTO;
using PetPPP.BLL.Interfaces.Filters;

namespace PetPPP.BLL.Interfaces
{
    public interface IUserService
    {
        Task AddUserAsync(AppUserDTO user, CancellationToken token);
        Task<AppUserDTO> EditUserAsync(AppUserDTO userDTO, Guid id, CancellationToken token);
        Task<Guid> LoginUserAsync(LoginDTO user, CancellationToken token);
        Task<IEnumerable<AppUser>> GetUsersAsync(UserFilter filter, CancellationToken token);

    }
}
