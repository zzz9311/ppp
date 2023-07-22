using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<UsersRefreshToken> UsersRefreshTokens { get; set; }
        public ApplicationDbContext() : base()
        {
            
        }
    }
}
