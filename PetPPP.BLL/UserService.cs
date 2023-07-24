using AutoMapper;
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
        private readonly IMapper _mapper;

        public UserService(IRepository<AppUser> repository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task AddUserAsync(AppUserDTO userDTO, CancellationToken token)
        {
            var user = _mapper.Map<AppUser>(userDTO);
            user.Password = CreatePasswordHash(userDTO.Password);
            await _repository.AddAsync(user, token);
            await _unitOfWork.SaveAsync(token);
        }

        public async Task<bool> EditUserAsync(AppUserDTO userDTO, Guid id, CancellationToken token)
        {
            var user = await _repository.FirstOrDefaultAsync(i => i.Id == id, token);
            if (user == null)
                return false;
            user = _mapper.Map<AppUser>(userDTO);
            _repository.Update(user);
            await _unitOfWork.SaveAsync(token);
            return true;
        }

        public async Task<AppUser> GetUserByIdAsync(Guid id, CancellationToken token)
        {
            return await _repository.FirstOrDefaultAsync(i => i.Id == id, token);
        }

        public async Task<Guid> GetUserIdByUsername(string username, CancellationToken token)
        {
            var user = await _repository.FirstOrDefaultAsync(i => i.Username == username, token);
            if (user != null)
                return user.Id;
            else
                return Guid.Empty;
        }

        public async Task<Guid> LoginUserAsync(LoginDTO userDTO, CancellationToken token)
        {
            var user = await _repository.FirstOrDefaultAsync(i => i.Username == userDTO.Username, token);
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
                    return false;
            return true;
        }
    }
}
