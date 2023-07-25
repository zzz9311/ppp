using Core.DependencyInjectionExtensions;
using DAL.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Query.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DAL.Repository
{
    [SelfRegistered(typeof(IRepository<>))]
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _context;
        private IQueryable<T> _query;
        private readonly DbSet<T> _dbSet;

        public Repository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
            _query = _context.Set<T>();
        }
        public void Add(T entity)
        {
            _dbSet.Add(entity);
        }

        public async Task<EntityEntry<T>> AddAsync(T entity, CancellationToken token)
        {
            return await _dbSet.AddAsync(entity, token);
        }

        public bool Any(Expression<Func<T, bool>> expression)
        {
            return _query.Any(expression);
        } 
        public Task<bool> AnyAsync(Expression<Func<T, bool>> expression, CancellationToken token)
        {
            return GetQuery().AnyAsync(expression, token);
        }

        public Task<T> FirstOrDefaultAsync(CancellationToken token)
        {
            return GetQuery().FirstOrDefaultAsync(token);
        }

        public Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> expression, CancellationToken token)
        {
            return GetQuery().FirstOrDefaultAsync(expression, token);
        }

        public T FirstOrDefault()
        {
            return GetQuery().FirstOrDefault();
        }

        public T FirstOrDefault(Expression<Func<T, bool>> expression)
        {
            return GetQuery().FirstOrDefault(expression); 
        }

        public IRepository<T> Include(params Expression<Func<T, object>>[] includes)
        {
            foreach (var include in includes)
            {
                _query = _query.Include(include);
            }
            return this;
        }

        public void Remove(T entity)
        {
            _dbSet.Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            _dbSet.RemoveRange(entities);
        }

        public List<T> ToList()
        {
            return GetQuery().ToList();
        }
        public List<T> ToList(Expression<Func<T, bool>> expression)
        {
            return GetQuery().Where(expression).ToList();
        }

        public Task<List<T>> ToListAsync(CancellationToken token)
        {
            return GetQuery().ToListAsync(token);
        }

        public Task<List<T>> ToListAsync(Expression<Func<T, bool>> expression, CancellationToken token)
        {
            return GetQuery().Where(expression).ToListAsync(token);
        }

        public void Update(T entity)
        {
            _context.Update(entity);
        }

        public IQueryable<T> GetQuery()
        {
            return _query;
        }

        public void UpdateRange(IEnumerable<T> entities)
        {
            _context.UpdateRange(entities);
        }

        public T[] ToArray()
        {
            return GetQuery().ToArray();
        }

        public T[] ToArray(Expression<Func<T, bool>> expression)
        {
            return GetQuery().Where(expression).ToArray();
        }

        public async Task<T[]> ToArrayAsync(CancellationToken token)
        {
            return await GetQuery().ToArrayAsync(token);
        }

        public async Task<T[]> ToArrayAsync(Expression<Func<T, bool>> expression, CancellationToken token)
        {
            return await GetQuery().Where(expression).ToArrayAsync(token);
        }
    }
}
