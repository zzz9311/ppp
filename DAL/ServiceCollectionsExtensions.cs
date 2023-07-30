using Core.DependencyInjectionExtensions;
using DAL.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DAL
{
    public static class ServiceCollectionsExtensions
    {
        public static IServiceCollection UseDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSelfRegistered(typeof(UnitOfWork.UnitOfWork).Assembly);
            services.AddDbContext<ApplicationDbContext>(opt =>
                opt.UseNpgsql(configuration.GetConnectionString("DefaultConnection"),
                    x => x.MigrationsAssembly(nameof(DAL))));
            
            return services;
        }
    }
}
