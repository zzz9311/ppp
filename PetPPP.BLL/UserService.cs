using AutoMapper;
using Core.DependencyInjectionExtensions;
using Core.Exceptions;
using DAL.Entities;
using DAL.Repository;
using DAL.UnitOfWork;
using PetPPP.BLL.Interfaces;
using PetPPP.BLL.Interfaces.DTO;
using PetPPP.BLL.Interfaces.Filters;
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

        public async Task<AppUserDTO> EditUserAsync(AppUserDTO userDTO, Guid id, CancellationToken token)
        {
            var user = await _repository.FirstOrDefaultAsync(i => i.Id == id, token)
                ?? throw new EntityNotFoundException("User with Id not found");
            user = _mapper.Map<AppUser>(userDTO);
            _repository.Update(user);
            await _unitOfWork.SaveAsync(token);
            return _mapper.Map<AppUserDTO>(user);
        }

        public async Task<IEnumerable<AppUser>> GetUsersAsync(UserFilter filter, CancellationToken token)
        {
            var result = new List<AppUser>();
            if (filter.Id.HasValue)
            {
                result.Add(await _repository.FirstOrDefaultAsync(i => i.Id == filter.Id, token));
            }
            else if (filter.Username != null)
            {
                result.Add(await _repository.FirstOrDefaultAsync(i => i.Username == filter.Username, token));
            }
            else
            {
                result = await _repository.ToListAsync(token);
            }
            return result;
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
