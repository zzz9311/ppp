using Core.DependencyInjectionExtensions;

namespace PetPPP;

[SelfRegistered(typeof(TestService))]
public class TestService
{
    public string a = "test";

    public async Task Asdasdas()
    {
        var a = new RedisCacheDecorated(null);

        var fromCache = await a.GetOrAddAsync("text", () => { return new Testing(); }, null, CancellationToken.None);
    }
}

public class Testing
{
    public int ID { get; set; }
}