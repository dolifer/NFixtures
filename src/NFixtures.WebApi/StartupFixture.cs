using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Events;
using Xunit;
using Xunit.Abstractions;

namespace NFixtures.WebApi
{
    public abstract class StartupFixture<TStartup> : WebApplicationFactory<TStartup>, IAsyncLifetime
        where TStartup : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureAppConfiguration(ConfigureAppConfiguration);
            builder.ConfigureTestServices(ConfigureTestServices);
        }

        protected virtual void ConfigureAppConfiguration(IConfigurationBuilder configurationBuilder)
        {
        }

        protected virtual void ConfigureTestServices(IServiceCollection services)
        {
        }

        public void SetLogger(ITestOutputHelper output, LogEventLevel minLogLevel = LogEventLevel.Verbose)
        {
            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .WriteTo.TestOutput(output, minLogLevel)
                .CreateLogger();
        }

        public virtual Task InitializeAsync() => Task.CompletedTask;

        public virtual Task DisposeAsync() => Task.CompletedTask;
    }
}
