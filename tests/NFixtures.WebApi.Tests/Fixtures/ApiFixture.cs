using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using NFixtures.Primitives;
using NFixtures.WebApi.Extensions;
using Serilog;

namespace NFixtures.WebApi.Tests.Fixtures
{
    public class ApiFixture : StartupFixture<TestStartup>
    {
        public ApiFixture()
        {
            FirstUser = new TestUser("123", "email@server.com");
        }

        public TestUser FirstUser { get; }

        protected override IWebHostBuilder CreateWebHostBuilder()
        => WebHost.CreateDefaultBuilder()
                .UseStartup<TestStartup>()
                .UseSerilog();

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            base.ConfigureWebHost(builder);
            builder.UseContentRoot(".");
        }

        protected override void ConfigureTestServices(IServiceCollection services)
        {
            services
                .ConfigureTestAuthentication(FirstUser);
        }
    }
}
