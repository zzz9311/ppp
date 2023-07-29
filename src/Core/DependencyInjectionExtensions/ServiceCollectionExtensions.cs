using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Core.DependencyInjectionExtensions;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Автоматически регистрирует все сервисы в DI, если на них есть аттрибут <see cref="SelfRegisteredAttribute"/>
    /// </summary>
    /// <param name="services">Сервисы</param>
    /// <param name="assembly">Сборка в которой искать сервисы для авторегистрации</param>
    public static IServiceCollection AddSelfRegistered(this IServiceCollection services, Assembly assembly)
    {
        var allAttributedServices = assembly.GetTypes()
            .Where(x => x.IsDefined(typeof(SelfRegisteredAttribute), false)).ToArray();

        foreach (var el in allAttributedServices)
        {
            var attribute = (SelfRegisteredAttribute)el.GetCustomAttribute(typeof(SelfRegisteredAttribute), false);
            if (attribute == null)
            {
                throw new Exception("attribute was not found"); //probably impossible
            }
            
            var declaredInterface = attribute.InterfaceType ?? el.GetInterfaces().FirstOrDefault();
            if (declaredInterface == null)
            {
                throw new Exception($"Not found interface for automatically register service for DI({el})");
            }

            switch (attribute.LifeTime)
            {
                case ServiceLifeTime.Scoped:
                    return services.AddScoped(declaredInterface, el);
                case ServiceLifeTime.Singleton:
                    return services.AddSingleton(declaredInterface, el);
                case ServiceLifeTime.Transient:
                    return services.AddTransient(declaredInterface, el);
            }
        }
        
        return services;
    }
}