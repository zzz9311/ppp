using Core.DependencyInjectionExtensions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetPPP.BLL
{
    public static class ServiceCollectionsExtensions
    {
        public static IServiceCollection UseServices(this IServiceCollection services)
        {
            services.AddSelfRegistered(typeof(UserService).Assembly);
            return services;
        }
    }
}
