using MediatR;
using NerdStore.Core.Data.EventSourcing;
using NerdStore.Core.Messages;
using NerdStore.Core.Messages.CommonMessages.Notifications;
using System.Threading.Tasks;

namespace NerdStore.Core.Mediator
{
    public class MediatrHandler : IMediatrHandler
    {
        private readonly IMediator _mediator;
        private readonly IEventSourcingRepository _eventSourcingRepository;

        public MediatrHandler(IMediator mediator,
                              IEventSourcingRepository eventSourcingRepository)
        {
            _mediator = mediator;
            _eventSourcingRepository = eventSourcingRepository;
        }

        public async Task<bool> EnviarComando<T>(T comando) where T : Command
        {
            return await _mediator.Send(comando);
        }

        public async Task PublicarEvento<T>(T evento) where T : Event
        {
            await _mediator.Publish(evento);

            // Verifica se o tipo baseé domainevent, eles não serão persistidos
            if (!evento.GetType().BaseType.Name.Equals("DomainEvent"))
                await _eventSourcingRepository.SalvarEvento(evento);
        }

        public async Task PublicarNotificacao<T>(T notificacao) where T : DomainNotification
        {
            await _mediator.Publish(notificacao);
        }
    }
}
