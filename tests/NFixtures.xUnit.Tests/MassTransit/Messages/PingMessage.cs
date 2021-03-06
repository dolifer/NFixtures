using System;
using MassTransit;

namespace NFixtures.xUnit.Tests.MassTransit.Messages
{
    [Serializable]
    public class PingMessage : CorrelatedBy<Guid>
    {
        public PingMessage()
        {
        }

        public PingMessage(Guid id)
        {
            CorrelationId = id;
        }

        public Guid CorrelationId { get; set; }
    }
}
