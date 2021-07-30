using MediatR;
using NerdStore.Core.Messages.CommonMessages.Notifications;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NerdStore.WebApp.API.Extensions
{
    public class ErrorsViewModel
    {
        private readonly DomainNotificationHandler _notifications;

        public ErrorsViewModel(INotificationHandler<DomainNotification> notifications)
        {
            _notifications = (DomainNotificationHandler)notifications;
        }

        public async Task<List<Errors>> InvokeAsync()
        {
            var notificacoes = await Task.FromResult(_notifications.ObterNotificacoes());

            var listErros = new List<Errors>();

            notificacoes.ForEach(c => listErros.Add(new Errors(c.Key, c.Value)));

            return listErros;
        }
    }
}
