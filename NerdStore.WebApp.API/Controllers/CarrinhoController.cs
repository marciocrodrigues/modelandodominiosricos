using MediatR;
using Microsoft.AspNetCore.Mvc;
using NerdStore.Catalogo.Application.Services;
using NerdStore.Core.Mediator;
using NerdStore.Core.Messages.CommonMessages.Notifications;
using NerdStore.Vendas.Application.Commands;
using NerdStore.Vendas.Application.Queries;
using NerdStore.Vendas.Application.Queries.ViewModels;
using NerdStore.WebApp.API.Extensions;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NerdStore.WebApp.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CarrinhoController : ControllerBaseVendas
    {
        private readonly IProdutoAppService _produtoAppService;
        private readonly IMediatrHandler _mediatrHandler;
        private readonly IPedidoQueries _pedidoQueries;

        public CarrinhoController(INotificationHandler<DomainNotification> notifications,
                                  IProdutoAppService produtoAppService,
                                  IMediatrHandler mediatrHandler,
                                  ErrorsViewModel errors,
                                  IPedidoQueries pedidoQueries)
            : base(notifications, mediatrHandler, errors)
        {
            _produtoAppService = produtoAppService;
            _mediatrHandler = mediatrHandler;
            _pedidoQueries = pedidoQueries;
        }

        /// <summary>
        /// Rota para realizar compra
        /// </summary>
        /// <param name="id"></param>
        /// <param name="quantidade"></param>
        /// <returns></returns>
        [SwaggerResponse(statusCode: 200, description: "Pedido criado com sucesso")]
        [SwaggerResponse(statusCode: 400, description: "Produto não encotrado")]
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

            var erros = await RetornarErros();

            return BadRequest(erros);
        }

        /// <summary>
        /// Rota para buscar carrinho pelo identificador do cliente
        /// </summary>
        /// <param name="clienteId"></param>
        /// <returns></returns>
        [SwaggerResponse(statusCode: 200, type: typeof(CarrinhoViewModel))]
        [HttpGet]
        [Route("meu-carrinho/{clienteId:guid}")]
        public async Task<IActionResult> ObterMeuCarrinho(Guid clienteId)
        {
            return Ok(await _pedidoQueries.ObterCarrinhoCliente(clienteId));
        }

        /// <summary>
        /// Rota para remover carrinho
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [SwaggerResponse(statusCode: 200, description: "Carrinho removido com sucesso")]
        [SwaggerResponse(statusCode: 404, description: "Produto não encontrado no carrinho")]
        [SwaggerResponse(statusCode: 400, type: typeof(List<Errors>))]
        [HttpPost]
        [Route("remover-carrinho")]
        public async Task<IActionResult> RemoverItem(Guid id)
        {
            var produto = await _produtoAppService.ObterPorId(id);
            if (produto == null) return NotFound("Produto não encontrado no carrinho");

            var command = new RemoverItemPedidoCommand(ClienteId, id);
            await _mediatrHandler.EnviarComando(command);

            if (OperacaoValida())
            {
                return Ok("Carrinho removido com sucesso");
            }

            var erros = await RetornarErros();

            return BadRequest(erros);
        }

        /// <summary>
        /// Rota para atualizar item do carrinho
        /// </summary>
        /// <param name="id"></param>
        /// <param name="quantidade"></param>
        /// <returns></returns>
        [SwaggerResponse(statusCode: 200, description: "Item atualizado com sucesso")]
        [SwaggerResponse(statusCode: 404, description: "Produto não encontrado")]
        [SwaggerResponse(statusCode: 400, type: typeof(List<Errors>))]
        [HttpPost]
        [Route("atualizar-item")]
        public async Task<IActionResult> AtualizarItem(Guid id, int quantidade)
        {
            var produto = await _produtoAppService.ObterPorId(id);
            if (produto == null) return NotFound("Produto não encontrado");

            var command = new AtualizarItemPedidoCommand(ClienteId, id, quantidade);
            await _mediatrHandler.EnviarComando(command);

            if (OperacaoValida())
            {
                return Ok("Item atualizado com sucesso");
            }

            var erros = await RetornarErros();

            return BadRequest(erros);
        }

        /// <summary>
        /// Rota para aplicar voucher
        /// </summary>
        /// <param name="voucherCodigo"></param>
        /// <returns></returns>
        [SwaggerResponse(statusCode: 200, description: "Voucher aplicado com sucesso")]
        [SwaggerResponse(statusCode: 400, type: typeof(List<Errors>))]
        [HttpPost]
        [Route("aplicar-voucher")]
        public async Task<IActionResult> AplicarVoucher(string voucherCodigo)
        {
            var command = new AplicarVoucherPedidoCommand(ClienteId, voucherCodigo);
            await _mediatrHandler.EnviarComando(command);

            if (OperacaoValida())
            {
                return Ok("Voucher aplicado com sucesso");
            }

            var erros = await RetornarErros();

            return BadRequest(erros);
        }

        /// <summary>
        /// Rota para trazer o resumo da compra
        /// </summary>
        /// <returns></returns>
        [SwaggerResponse(statusCode: 200, type: typeof(CarrinhoViewModel))]
        [HttpGet]
        [Route("resumo-da-compra")]
        public async Task<IActionResult> ResumoDaCompra()
        {
            return Ok(await _pedidoQueries.ObterCarrinhoCliente(ClienteId));
        }

        [HttpPost]
        [Route("iniciar-pedido")]
        public async Task<IActionResult> IniciarPedido(CarrinhoViewModel carrinhoViewModel)
        {
            var carrinho = await _pedidoQueries.ObterCarrinhoCliente(ClienteId);

            var command = new IniciarPedidoCommand(carrinho.PedidoId, ClienteId, carrinho.ValorTotal, carrinhoViewModel.Pagamento.NomeCartao,
                carrinhoViewModel.Pagamento.NumeroCartao, carrinhoViewModel.Pagamento.ExpiracaoCartao, carrinhoViewModel.Pagamento.CvvCartao) ;

            await _mediatrHandler.EnviarComando(command);

            if (OperacaoValida())
            {
                return Ok("Pedido realizado com sucesso");
            }

            var erros = await RetornarErros();

            return BadRequest(erros);

        }
    }
}
