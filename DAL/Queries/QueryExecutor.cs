using Core.DependencyInjectionExtensions;
using Microsoft.EntityFrameworkCore;

namespace DAL.Queries;

[SelfRegistered]
public class QueryExecutor<T> : IQueryExecutor<T>
{
    public Task<T[]> ExecuteAsync(IQueryable<T> queryable, CancellationToken cancellationToken = default)
    {
        return queryable.ToArrayAsync(cancellationToken);
    }
}