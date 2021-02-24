using System;
using System.Threading.Tasks;
using MassTransit;
using NFixtures.xUnit.MassTransit;
using NFixtures.xUnit.Tests.MassTransit.Messages;
using Xunit;

namespace NFixtures.xUnit.Tests.MassTransit
{
    public class PublishReceiveTests : IClassFixture<PublishSubscribeFixture>
    {
        private readonly PublishSubscribeFixture _fixture;

        public PublishReceiveTests(PublishSubscribeFixture fixture)
        {
            _fixture = fixture ?? throw new ArgumentNullException(nameof(fixture));
        }

        [Fact]
        public async Task Received_Message_IsCorrect()
        {
            // arrange
            var service = new PingService(_fixture.Bus);

            // act
            await service.PingAsync();
            var context = await _fixture.Received;

            // assert
            Assert.NotNull(context);
            Assert.NotNull(context.Message);
        }
    }

    public class PingService
    {
        private readonly IBus _bus;

        public PingService(IBus bus)
        {
            _bus = bus ?? throw new ArgumentNullException(nameof(bus));
        }

        public async Task PingAsync()
        {
            await _bus.Publish(new PingMessage());
        }
    }

    public class PublishSubscribeFixture : MassTransitFixture
    {
        public Task<ConsumeContext<PingMessage>> Received;

        protected override void ConfigureInMemoryReceiveEndpoint(IInMemoryReceiveEndpointConfigurator configurator)
        {
            Received = InMemoryTestHarness.Handled<PingMessage>(configurator);
        }
    }
}
