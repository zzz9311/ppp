using Core.DependencyInjectionExtensions;
using DAL.Entities;
using DAL.Repository;
using DAL.UnitOfWork;
using PetPPP.BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetPPP.BLL
{
    [SelfRegistered(typeof(IRefreshTokenService))]
    public class RefreshTokenService : IRefreshTokenService
    {
        private readonly IRepository<UsersRefreshToken> _repository;
        private readonly IUnitOfWork _unitOfWork;

        public RefreshTokenService(IRepository<UsersRefreshToken> repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task SetRefreshTokenToUserAsync(Guid id, string refreshToken, string deviceInfo, CancellationToken token)
        {
            var userRefreshToken = new UsersRefreshToken()
            {
                RefreshToken = refreshToken,
                UserId = id,
                DeviceInfo = deviceInfo,
            };
            await _repository.AddAsync(userRefreshToken, token);
            await _unitOfWork.SaveAsync(token);   
        }

        public async Task<UsersRefreshToken> GetUserRefreshTokenAsync(Guid userId, string deviceInfo, CancellationToken token)
        {
            return await _repository.FirstOrDeafultAsync(i => i.UserId == userId && i.DeviceInfo == deviceInfo, token);
        }
    }
}
