using System;
using MassTransit;

namespace NFixtures.xUnit.Tests.MassTransit.Messages
{
    [Serializable]
    public class PingMessage : IEquatable<PingMessage>, CorrelatedBy<Guid>
    {
        private Guid _id = new Guid("95bb0b6d-2754-4882-918c-9187ff768043");

        public PingMessage()
        {
        }

        public PingMessage(Guid id)
        {
            _id = id;
        }

        public Guid CorrelationId
        {
            get { return _id; }
            set { _id = value; }
        }

        public bool Equals(PingMessage obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj._id.Equals(_id);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof(PingMessage)) return false;
            return Equals((PingMessage) obj);
        }

        public override int GetHashCode() => _id.GetHashCode();
    }
}
