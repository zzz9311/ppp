using Core.DependencyInjectionExtensions;
using Core.Exceptions;
using DAL.Data;
using Microsoft.EntityFrameworkCore;
using System.Data.SqlClient;

namespace DAL.UnitOfWork
{
    [SelfRegistered(typeof(IUnitOfWork))]
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Save()
        {
            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                UniqueIndexExceptionHandler(ex);
            }
        }

        public async Task SaveAsync(CancellationToken token)
        {
            try
            {
                await _context.SaveChangesAsync(token);
            }
            catch (DbUpdateException ex)
            {
                UniqueIndexExceptionHandler(ex);
            }
        }

        private void UniqueIndexExceptionHandler(DbUpdateException exception)
        {
            if (exception.InnerException is SqlException sqlException && sqlException.Number == 2627)
                throw new UniqueIndexException("Unique constraint error");
        }
    }
}
