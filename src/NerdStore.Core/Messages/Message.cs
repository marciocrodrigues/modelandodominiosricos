using System;

namespace NerdStore.Core.Messages
{
    public abstract class Message
    {
        public string MessageType { get; set; }
        public Guid AggretateId { get; set; }

        protected Message()
        {
            MessageType = GetType().Name;
        }
    }
}
