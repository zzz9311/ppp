using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DAL.Repository
{
    public interface IRepository<T> where T : class
    {
        void SaveChanges();
        Task SaveChangesAsync();
        List<T> ToList();
        List<T> ToList(Expression<Func<T,bool>> expression);
        Task<List<T>> ToListAsync(CancellationToken token);
        Task<List<T>> ToListAsync(Expression<Func<T, bool>> expression, CancellationToken token);
        T FirstOrDefault();
        T FirstOrDefault(Expression<Func<T, bool>> expression);
        Task<T> FirstOrDeafultAsync(CancellationToken token);
        Task<T> FirstOrDeafultAsync(Expression<Func<T, bool>> expression, CancellationToken token);
        Task<EntityEntry<T>> AddAsync(T entity, CancellationToken token);
        void Add(T entity);
        bool Any(Expression<Func<T, bool>> expression);
        Task<bool> AnyAsync(Expression<Func<T, bool>> expression, CancellationToken token);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entities);
        void Update(T entity);
        void UpdateRange(IEnumerable<T> entities);
        IRepository<T> Include(params Expression<Func<T, object>>[] includes);
        IQueryable<T> GetQuery();
    }
}
