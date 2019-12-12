using Microsoft.AspNetCore.Mvc.Testing;

namespace NFixtures.WebApi
{
    public class StartupFixture<TStartup> : WebApplicationFactory<TStartup>
        where TStartup : class
    {
    }
}