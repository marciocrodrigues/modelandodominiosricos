using MediatR;
using Microsoft.AspNetCore.Mvc;
using NerdStore.Catalogo.Application.Services;
using NerdStore.Core.Mediator;
using NerdStore.Core.Messages.CommonMessages.Notifications;
using NerdStore.Vendas.Application.Commands;
using NerdStore.WebApp.API.Extensions;
using System;
using System.Threading.Tasks;

namespace NerdStore.WebApp.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CarrinhoController : ControllerBaseVendas
    {
        private readonly IProdutoAppService _produtoAppService;
        private readonly IMediatrHandler _mediatrHandler;

        public CarrinhoController(INotificationHandler<DomainNotification> notifications,
                                  IProdutoAppService produtoAppService,
                                  IMediatrHandler mediatrHandler,
                                  ErrorsViewModel errors)
            : base(notifications, mediatrHandler, errors)
        {
            _produtoAppService = produtoAppService;
            _mediatrHandler = mediatrHandler;
        }

        [HttpPost]
        [Route("meu-carrinho")]
        public async Task<IActionResult> AdicionarItem(Guid id, int quantidade)
        {
            var produto = await _produtoAppService.ObterPorId(id);
            if (produto == null)
                return BadRequest("Produto não encotrado");

            if (produto.QuantidadeEstoque < quantidade)
            {
                return BadRequest($"Produto com {produto.QuantidadeEstoque} em estoque");
            }

            var command = new AdicionarItemPedidoCommand(ClienteId, produto.Id, produto.Nome, quantidade, produto.Valor);
            await _mediatrHandler.EnviarComando(command);

            // se tudo deu certo?
            if (OperacaoValida())
            {
                return Ok("Pedido criado com sucesso");
            }

            return BadRequest(await RetornarErros());
        }
    }
}
