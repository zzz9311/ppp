using Core.DependencyInjectionExtensions;

namespace PetPPP;

[SelfRegistered(typeof(TestService))]
public class TestService
{
    public string a = "test";
}