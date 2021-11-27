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
            builder.ConfigureTestServices(ConfigureTestServices);
        }

        /// <inheritdoc cref="IWebHostBuilder.ConfigureAppConfiguration"/>
        protected virtual void ConfigureAppConfiguration(IConfigurationBuilder configurationBuilder)
        {
        }

        /// <inheritdoc cref="Microsoft.AspNetCore.TestHost.WebHostBuilderExtensions.ConfigureTestServices"/>
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
        async Task IAsyncLifetime.DisposeAsync() => await DisposeAsync();
    }
}
