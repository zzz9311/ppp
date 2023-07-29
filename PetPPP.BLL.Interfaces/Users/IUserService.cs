using PetPPP.BLL.Interfaces.DTO;

namespace PetPPP.BLL.Interfaces.Users
{
    public interface IUserService
    {
        /// <summary>
        /// Добавляет нового пользователя
        /// </summary>
        /// <param name="userChangeable">Данные нового пользователя</param>
        /// <param name="token">Токен отмены</param>
        Task AddAsync(UserChangeableDTO userChangeable, CancellationToken token);
        
        /// <summary>
        /// Изменяет данные пользователя
        /// </summary>
        /// <param name="id">ID пользователя</param>
        /// <param name="userChangeableDto">Данные для замены пользователя</param>
        /// <param name="token">Токен отмены</param>
        Task<UserDTO> EditAsync(Guid id, UserChangeableDTO userChangeableDto, CancellationToken token);

        /// <summary>
        /// Получение списка пользователей
        /// </summary>
        /// <param name="filter">Фильтр для получения</param>
        /// <param name="token">Токен отмены</param>
        Task<UserDTO[]> ListAsync(UserFilter filter, CancellationToken token);
        
        Task<UserDTO> GetAsync(Guid id, CancellationToken cancellationToken = default);
        
        Task<Guid> LoginAsync(LoginDTO user, CancellationToken token);
    }
}
