namespace Core.DependencyInjectionExtensions;

public class SelfRegisteredAttribute : Attribute
{
    public ServiceLifeTime LifeTime { get; }
    public Type InterfaceType { get; }

    public SelfRegisteredAttribute(Type interfaceType, ServiceLifeTime lifeTime = ServiceLifeTime.Scoped)
    {
        InterfaceType = interfaceType;
        LifeTime = lifeTime;
    }
    
    public SelfRegisteredAttribute(ServiceLifeTime lifeTime = ServiceLifeTime.Scoped)
    {
        LifeTime = lifeTime;
    }
}