using Core.DependencyInjectionExtensions;
using Core.Exceptions;
using DAL.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                if (ex.InnerException is SqlException sqlException && sqlException.Number == 2627)
                    throw new UniqueIndexException(ex.Message);
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
                if (ex.InnerException is SqlException sqlException && sqlException.Number == 2627)
                    throw new UniqueIndexException(ex.Message);
            }
        }
    }
}
