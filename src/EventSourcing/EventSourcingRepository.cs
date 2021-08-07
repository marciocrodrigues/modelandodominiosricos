using EventStore.ClientAPI;
using NerdStore.Core.Data.EventSourcing;
using NerdStore.Core.Messages;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSourcing
{
    public class EventSourcingRepository : IEventSourcingRepository
    {
        private readonly IEventStoreService _eventStoreService;

        public EventSourcingRepository(IEventStoreService eventStoreService)
        {
            _eventStoreService = eventStoreService;
        }

        public async Task<IEnumerable<StoredEvent>> ObterEventos(Guid aggregateId)
        {
            var eventos = await _eventStoreService.GetConnection()
                .ReadStreamEventsBackwardAsync(aggregateId.ToString(), 0, 500, false);

            var listaEventos = new List<StoredEvent>();

            return listaEventos;
        }

        public async Task SalvarEvento<TEvent>(TEvent evento) where TEvent : Event
        {
            await _eventStoreService.GetConnection().AppendToStreamAsync(
                evento.AggretateId.ToString(),
                ExpectedVersion.Any,
                FormatarEvento(evento));
        }

        private static IEnumerable<EventData> FormatarEvento<TEvent>(TEvent evento) where TEvent : Event
        {
            // Objeto que será enviado e persistido no EventoStore
            yield return new EventData(
                Guid.NewGuid(),
                evento.MessageType,
                true,
                Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(evento)),
                null);
        }
    }
}
