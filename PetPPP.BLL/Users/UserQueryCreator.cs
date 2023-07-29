using Core.DependencyInjectionExtensions;
using DAL.Entities;
using DAL.Repository;
using PetPPP.BLL.Interfaces.QueryCreators;
using PetPPP.BLL.Interfaces.Users;

namespace PetPPP.BLL.Users;

[SelfRegistered]
public class UserQueryCreator : IQueryCreator<AppUser, UserFilter>
{
    private readonly IRepository<AppUser> _repository;

    public UserQueryCreator(IRepository<AppUser> repository)
    {
        _repository = repository;
    }

    public IQueryable<AppUser> Create(UserFilter filter)
    {
        var query = _repository.GetQuery();

        if (filter.Id.HasValue)
        {
            query = query.Where(x => x.Id == filter.Id);
        }

        if (!string.IsNullOrEmpty(filter.Username))
        {
            query = query.Where(x => x.Username.Contains(filter.Username));
        }

        return query;
    }
}