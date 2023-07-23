using Core.DependencyInjectionExtensions;
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
        public static IServiceCollection UseDatabase(this IServiceCollection services)
        {
            services.AddSelfRegistered(typeof(ServiceCollectionExtensions).Assembly);
            return services;
        }
    }
}
