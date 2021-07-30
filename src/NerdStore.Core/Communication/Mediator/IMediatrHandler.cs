using NerdStore.Core.Messages;
using NerdStore.Core.Messages.CommonMessages.Notifications;
using System.Threading.Tasks;

namespace NerdStore.Core.Mediator
{
    public interface IMediatrHandler
    {
        Task PublicarEvento<T>(T evento) where T : Event;
        Task<bool> EnviarComando<T>(T comando) where T : Command;
        Task PublicarNotificacao<T>(T notificacao) where T : DomainNotification;
    }
}
