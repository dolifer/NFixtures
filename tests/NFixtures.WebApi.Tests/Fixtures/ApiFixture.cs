using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace NFixtures.WebApi.Tests.Fixtures
{
    public class ApiFixture : StartupFixture<TestStartup>
    {
        protected override IWebHostBuilder CreateWebHostBuilder()
            => WebHost.CreateDefaultBuilder()
                .UseStartup<TestStartup>();

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseContentRoot(".");
            base.ConfigureWebHost(builder);
        }
    }
}
