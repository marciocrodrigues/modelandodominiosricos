using MediatR;
using Microsoft.AspNetCore.Mvc;
using NerdStore.Core.Mediator;
using NerdStore.Core.Messages.CommonMessages.Notifications;
using NerdStore.WebApp.API.Extensions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NerdStore.WebApp.API.Controllers
{
    public abstract class ControllerBaseVendas : ControllerBase
    {
        private readonly DomainNotificationHandler _notifications;
        private readonly ErrorsViewModel _errors;
        private readonly IMediatrHandler _mediatorHandler;

        // Para simular um cliente logado
        protected Guid ClienteId = Guid.Parse("5624e062-c2bb-45c0-838b-a427c19bba5b");

        protected ControllerBaseVendas(INotificationHandler<DomainNotification> notifications,
                                       IMediatrHandler mediatorHandler,
                                       ErrorsViewModel errors)
        {
            _notifications = (DomainNotificationHandler)notifications;
            _mediatorHandler = mediatorHandler;
            _errors = errors;
        }

        protected bool OperacaoValida()
        {
            return !_notifications.TemNotificacao();
        }

        protected void NotificarErro(string codigo, string mensagem)
        {
            _mediatorHandler.PublicarNotificacao(new DomainNotification(codigo, mensagem));
        }

        protected async Task<List<Errors>> RetornarErros()
        {
            return await _errors.InvokeAsync();
        }
    }
}
