using Core.DependencyInjectionExtensions;
using DAL.Entities;
using DAL.Repository;
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

        public RefreshTokenService(IRepository<UsersRefreshToken> repository)
        {
            _repository = repository;
        }

        public async Task SetRefreshTokenToUserAsync(Guid id, string refreshToken, CancellationToken token)
        {
            var userRefreshToken = new UsersRefreshToken()
            {
                RefreshToken = refreshToken,
                UserId = id
            };
            await _repository.AddAsync(userRefreshToken, token);
            await _repository.SaveChangesAsync();   
        }
    }
}
