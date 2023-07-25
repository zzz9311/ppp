using Core.DependencyInjectionExtensions;
using DAL.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public static class ServiceCollectionsExtensions
    {
        public static IServiceCollection UseDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSelfRegistered(typeof(ServiceCollectionExtensions).Assembly);
            services.AddDbContext<ApplicationDbContext>(opt => opt.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));
            return services;
        }
    }
}
