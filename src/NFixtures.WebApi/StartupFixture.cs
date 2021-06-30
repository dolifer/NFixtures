using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using Xunit;
using Xunit.Abstractions;

namespace NFixtures.WebApi
{
    /// <summary>
    /// Fixture for bootstrapping an application in memory for functional end to end tests.
    /// </summary>
    /// <typeparam name="TStartup">A type in the entry point assembly of the application.
    /// Typically the Startup or Program classes can be used.</typeparam>
    public abstract class StartupFixture<TStartup> : WebApplicationFactory<TStartup>, IAsyncLifetime
        where TStartup : class
    {
        /// <inheritdoc/>
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureAppConfiguration(ConfigureAppConfiguration);
            builder.ConfigureTestServices(services =>
            {
                services.AddLogging(x =>
                {
                    x.ClearProviders();
                    x.AddConsole();
                });

                ConfigureTestServices(services);
            });
        }

        /// <summary>
        /// Allows configuring the <see cref="IConfigurationBuilder"/> that will construct an <see cref="IConfiguration"/>.
        /// </summary>
        /// <param name="configurationBuilder">The <see cref="IConfigurationBuilder"/> to configure.</param>
        protected virtual void ConfigureAppConfiguration(IConfigurationBuilder configurationBuilder)
        {
        }

        /// <summary>
        /// Configures the test service provider.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to configure.</param>
        protected virtual void ConfigureTestServices(IServiceCollection services)
        {
        }

        /// <summary>
        /// Configures internal logger to use <see cref="ITestOutputHelper"/> as destination for log output.
        /// </summary>
        /// <param name="output">The xUnit test output helper.</param>
        /// <param name="minLogLevel">The minimal log level.</param>
        public void SetLogger(ITestOutputHelper output, LogEventLevel minLogLevel = LogEventLevel.Verbose)
        {
            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .WriteTo.TestOutput(output, minLogLevel)
                .CreateLogger();
        }

        /// <inheritdoc/>
        public virtual Task InitializeAsync() => Task.CompletedTask;

        /// <inheritdoc/>
        public virtual Task DisposeAsync() => Task.CompletedTask;
    }
}
