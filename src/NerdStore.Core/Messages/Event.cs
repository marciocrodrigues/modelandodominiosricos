using MediatR;
using System;

namespace NerdStore.Core.Messages
{
    // INotification: instalar o mediatr
    public abstract class Event : Message, INotification
    {
        public DateTime Timestamp { get; private set; }

        protected Event()
        {
            Timestamp = DateTime.Now;
        }
    }
}
