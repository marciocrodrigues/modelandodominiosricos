using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NerdStore.Catalogo.Application.Services;
using NerdStore.Catalogo.Application.ViewModels;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NerdStore.WebApp.API.Controllers.Admin
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AdminProdutosController : ControllerBase
    {
        private readonly IProdutoAppService _produtoAppService;

        public AdminProdutosController(IProdutoAppService produtoAppService)
        {
            _produtoAppService = produtoAppService;
        }

        /// <summary>
        /// Rota para cadastrar categorias
        /// </summary>
        /// <returns></returns>
        [SwaggerResponse(statusCode: 200)]
        [HttpPost]
        [Route("nova-categoria")]
        public async Task<IActionResult> NovoProduto(CategoriaViewModel categoriaViewModel)
        {
            await _produtoAppService.AdicionarCategoria(categoriaViewModel);

            return Created("", null);
        }

        /// <summary>
        /// Rota para cadastrar produto
        /// </summary>
        /// <param name="produtoViewModel"></param>
        /// <returns></returns>
        [SwaggerResponse(statusCode: 204)]
        [HttpPost]
        [Route("novo-produto")]
        public async Task<IActionResult> NovoProduto(ProdutoViewModel produtoViewModel)
        {
            await _produtoAppService.AdicionarProduto(produtoViewModel);
            
            return Created("", null);
        }

        /// <summary>
        /// Rota para buscar produto pelo identificador
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [SwaggerResponse(statusCode: 200, type: typeof(ProdutoViewModel))]
        [SwaggerResponse(statusCode: 404)]
        [HttpGet]
        [Route("produtos-atualizar-estoque/{id:guid}")]
        public async Task<IActionResult> AtualizarEstoque(Guid id)
        {
            var produtoViewModel = await _produtoAppService.ObterPorId(id);

            if (produtoViewModel == null)
                return NotFound();

            return Ok(produtoViewModel);
        }

        /// <summary>
        /// Rota para atualizar produto
        /// </summary>
        /// <param name="id"></param>
        /// <param name="produtoViewModel"></param>
        /// <returns></returns>
        [SwaggerResponse(statusCode: 200, type: typeof(ProdutoViewModel))]
        [HttpPost]
        [Route("editar-produto/{id:guid}")]
        public async Task<IActionResult> AtualizarProduto(Guid id, ProdutoViewModel produtoViewModel)
        {
            var produto = await _produtoAppService.ObterPorId(id);
            produtoViewModel.QuantidadeEstoque = produto.QuantidadeEstoque;

            ModelState.Remove("QuantidadeEstoque");
            
            await _produtoAppService.AtualizarProduto(produtoViewModel);

            return Ok(await _produtoAppService.ObterPorId(produto.Id));
        }

        /// <summary>
        /// Rota pra editar categorias
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [SwaggerResponse(statusCode: 200, type: typeof(IEnumerable<CategoriaViewModel>))]
        [HttpPost]
        [Route("editar-categoria/{id:guid}")]
        public async Task<IActionResult> AtualiarProduto(Guid id, CategoriaViewModel categoriaViewModel)
        {
            await _produtoAppService.AtualizarCategoria(categoriaViewModel);

            return Ok(await _produtoAppService.ObterPorCategorias());
        }

        [SwaggerResponse(statusCode: 200, type: typeof(IEnumerable<CategoriaViewModel>))]
        [SwaggerResponse(statusCode: 404)]
        [HttpGet]
        [Route("listar-categorias")]
        public async Task<IActionResult> ListarCategorias()
        {
            var categoriasViewModel = await _produtoAppService.ObterPorCategorias();

            if (categoriasViewModel.Count() == 0)
                return NotFound();

            return Ok(categoriasViewModel);
        }

        /// <summary>
        /// Rota para atualizar o estoque
        /// Passar quantidade negativa para retirar do estoque
        /// </summary>
        /// <param name="id"></param>
        /// <param name="quantidade"></param>
        /// <returns></returns>
        [SwaggerResponse(statusCode: 200, type: typeof(IEnumerable<ProdutoViewModel>))]
        [HttpGet]
        [Route("produtos-atualizar-estoque/{id:guid}/{quantidade:int}")]
        public async Task<IActionResult> AtualizarEstoque(Guid id, int quantidade)
        {
            if (quantidade > 0)
            {
                await _produtoAppService.ReporEstoque(id, quantidade);
            }
            else
            {
                await _produtoAppService.DebitarEstqoue(id, quantidade);
            }

            return Ok(await _produtoAppService.ObterTodos());
        }
    }
}
