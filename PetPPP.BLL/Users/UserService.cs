using System.Security.Cryptography;
using AutoMapper;
using Core.DependencyInjectionExtensions;
using Core.Exceptions;
using DAL.Entities;
using DAL.Queries;
using DAL.Repository;
using DAL.UnitOfWork;
using PetPPP.BLL.Interfaces.DTO;
using PetPPP.BLL.Interfaces.QueryCreators;
using PetPPP.BLL.Interfaces.Users;

namespace PetPPP.BLL.Users
{
    [SelfRegistered(typeof(IUserService))]
    public class UserService : IUserService
    {
        private readonly IRepository<AppUser> _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IQueryCreator<AppUser, UserFilter> _queryCreator;
        private readonly IQueryExecutor<AppUser> _executor;

        public UserService(IRepository<AppUser> repository,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IQueryCreator<AppUser, UserFilter> queryCreator,
            IQueryExecutor<AppUser> executor)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _queryCreator = queryCreator;
            _executor = executor;
        }

        public async Task AddAsync(UserChangeableDTO userChangeableDto, CancellationToken token)
        {
            var user = _mapper.Map<AppUser>(userChangeableDto);
            user.Password = CreatePasswordHash(userChangeableDto.Password);
            
            await _repository.AddAsync(user, token);
            await _unitOfWork.SaveAsync(token);
        }

        public async Task<UserDTO> EditAsync(Guid id, UserChangeableDTO userChangeableDto, CancellationToken token)
        {
            var user = await _repository.FirstOrDefaultAsync(i => i.Id == id, token)
                ?? throw new EntityNotFoundException("User with Id not found");
            
            _mapper.Map(userChangeableDto, user);
            
            await _unitOfWork.SaveAsync(token);
            return _mapper.Map<UserDTO>(user);
        }

        public async Task<UserDTO[]> ListAsync(UserFilter filter, CancellationToken token = default)
        {
            var query = _queryCreator.Create(filter);
            var users = await _executor.ExecuteAsync(query, token);

            return _mapper.Map<UserDTO[]>(users);
        }

        public async Task<UserDTO> GetAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var user = await _repository.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
            return _mapper.Map<UserDTO>(user);
        }
        
        public async Task<Guid> LoginAsync(LoginDTO userDTO, CancellationToken token)
        {
            var user = await _repository.FirstOrDefaultAsync(i => i.Username == userDTO.Username, token);
            if (VerifyHashedPassword(userDTO.Password, user.Password))
            {
                return user.Id;
            }

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

        private bool VerifyHashedPassword(string enteredPassword, string storedPasswordHash)
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
