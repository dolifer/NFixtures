using System.Threading.Tasks;
using MassTransit;
using NFixtures.xUnit.MassTransit;
using NFixtures.xUnit.Tests.MassTransit.Messages;
using Shouldly;
using Xunit;

namespace NFixtures.xUnit.Tests.MassTransit
{
    public class PublishReceiveTests : IClassFixture<PublishSubscribeFixture>
    {
        private readonly PublishSubscribeFixture _fixture;

        public PublishReceiveTests(PublishSubscribeFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task Received_Message_IsCorrect()
        {
            // arrange
            var message = new PingMessage();

            // act
            await _fixture.Bus.Publish(message);
            ConsumeContext<PingMessage> context = await _fixture.Received;

            // assert
            context.Message.CorrelationId.ShouldBe(message.CorrelationId);
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
