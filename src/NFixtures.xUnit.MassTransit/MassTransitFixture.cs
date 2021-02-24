using System;
using System.Threading.Tasks;
using MassTransit;
using MassTransit.Testing;
using Xunit;

namespace NFixtures.xUnit.MassTransit
{
    /// <summary>
    /// The fixture that configures InMemory test harness.
    /// </summary>
    public abstract class MassTransitFixture : IAsyncLifetime
    {
        /// <summary>
        /// Gets the InMemory test harness.
        /// </summary>
        protected InMemoryTestHarness InMemoryTestHarness { get; }

        /// <summary>
        /// Gets the <see cref="IBus"/>.
        /// </summary>
        public IBus Bus => InMemoryTestHarness.Bus;

        /// <summary>
        /// Initializes a new instance of the <see cref="MassTransitFixture"/> class.
        /// </summary>
        protected MassTransitFixture() : this(new InMemoryTestHarness())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MassTransitFixture"/> class.
        /// </summary>
        /// <param name="harness">The InMemory test harness.</param>
        protected MassTransitFixture(InMemoryTestHarness harness)
        {
            InMemoryTestHarness = harness ?? throw new ArgumentNullException(nameof(harness));

            InMemoryTestHarness.OnConfigureInMemoryBus += ConfigureInMemoryBus;
            InMemoryTestHarness.OnConfigureInMemoryReceiveEndpoint += ConfigureInMemoryReceiveEndpoint;
        }

        /// <summary>
        /// Used to configure InMemory bus factory in your tests.
        /// </summary>
        /// <param name="configurator">Bus factory configurator.</param>
        protected virtual void ConfigureInMemoryBus(IInMemoryBusFactoryConfigurator configurator)
        {
        }

        /// <summary>
        /// Used to configure InMemory receive endpoint in your tests.
        /// </summary>
        /// <param name="configurator">Receive endpoint configurator.</param>
        protected virtual void ConfigureInMemoryReceiveEndpoint(IInMemoryReceiveEndpointConfigurator configurator)
        {
        }

        /// <inheritdoc/>
        public Task InitializeAsync() => InMemoryTestHarness.Start();

        /// <inheritdoc/>
        public async Task DisposeAsync()
        {
            await InMemoryTestHarness.Stop().ConfigureAwait(false);

            InMemoryTestHarness.Dispose();
        }
    }
}
