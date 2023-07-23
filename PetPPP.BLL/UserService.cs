using Core.DependencyInjectionExtensions;
using DAL.Entities;
using DAL.Repository;
using DAL.UnitOfWork;
using PetPPP.BLL.Interfaces;
using PetPPP.BLL.Interfaces.DTO;
using System.Security.Cryptography;

namespace PetPPP.BLL
{
    [SelfRegistered(typeof(IUserService))]
    public class UserService : IUserService
    {
        private readonly IRepository<AppUser> _repository;
        private readonly IUnitOfWork _unitOfWork;

        public UserService(IRepository<AppUser> repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task AddUserAsync(AppUserDTO userDTO, CancellationToken token)
        {
            var user = new AppUser()
            {
                Email = userDTO.Email,
                Username = userDTO.UserName,
                Password = CreatePasswordHash(userDTO.Password)
            };
            await _repository.AddAsync(user, token);
            await _unitOfWork.SaveAsync(token);
        }

        public async Task EditUserAsync(AppUserDTO userDTO, Guid id, CancellationToken token)
        {
            var user = await _repository.FirstOrDeafultAsync(i => i.Id == id, token);
            user.Email = userDTO.Email;
            _repository.Update(user);
            await _unitOfWork.SaveAsync(token);
        }

        public async Task<AppUser> GetUserByIdAsync(Guid id, CancellationToken token)
        {
            return await _repository.FirstOrDeafultAsync(i => i.Id == id, token);
        }

        public async Task<bool> IsUserWithUsernameExists(string username, CancellationToken token)
        {
            var user = await _repository.FirstOrDeafultAsync(i => i.Username == username, token);
            return user != null;
        }

        public async Task<Guid> LoginUserAsync(AppUserDTO userDTO, CancellationToken token)
        {
            var user = await _repository.FirstOrDeafultAsync(i => i.Username == userDTO.UserName, token);
            if (VerifyPassword(userDTO.Password, user.Password))
            {
                return user.Id;
            }
            else
                return Guid.Empty;
        }

        private string CreatePasswordHash(string password)
        {
            var salt = new byte[16];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(salt);
            var der = new Rfc2898DeriveBytes(password, salt, 10000, HashAlgorithmName.SHA256);
            var hash = der.GetBytes(20);
            var hashBytes = new byte[36];
            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 20);
            return Convert.ToBase64String(hashBytes);
        }

        private bool VerifyPassword(string enteredPassword, string storedPasswordHash)
        {
            var hashBytes = Convert.FromBase64String(storedPasswordHash);
            var salt = new byte[16];
            Array.Copy(hashBytes, 0, salt, 0, 16);
            var der = new Rfc2898DeriveBytes(enteredPassword, salt, 10000, HashAlgorithmName.SHA256);
            var hash = der.GetBytes(20);
            for (int i = 0; i < 20; i++)
                if (hashBytes[i + 16] != hash[i])
                    throw new UnauthorizedAccessException();
            return true;
        }
    }
}
