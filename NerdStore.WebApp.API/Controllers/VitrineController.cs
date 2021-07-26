using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NerdStore.Catalogo.Application.Services;
using NerdStore.Catalogo.Application.ViewModels;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NerdStore.WebApp.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class VitrineController : ControllerBase
    {
        private readonly IProdutoAppService _produtoAppService;

        public VitrineController(IProdutoAppService produtoAppService)
        {
            _produtoAppService = produtoAppService;
        }

        /// <summary>
        /// Rota para listar todos os produtos
        /// </summary>
        /// <returns></returns>
        [SwaggerResponse(statusCode: 200, type: typeof(IEnumerable<ProdutoViewModel>))]
        [SwaggerResponse(statusCode: 404)]
        [HttpGet]
        [Route("vitrine")]
        public async Task<IActionResult> GetTodos()
        {
            var produtosViewModel = await _produtoAppService.ObterTodos();

            if (produtosViewModel.Count() == 0)
                return NotFound();

            return Ok(produtosViewModel);
        }

        /// <summary>
        /// Rota para buscar produto pelo identificador
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [SwaggerResponse(statusCode: 200, type: typeof(ProdutoViewModel))]
        [SwaggerResponse(statusCode: 404)]
        [HttpGet]
        [Route("produto-detalhe/{id:guid}")]
        public async Task<IActionResult> ProdutoDetalhe(Guid id)
        {
            var produtoViewModel = await _produtoAppService.ObterPorId(id);

            if (produtoViewModel == null)
                return NotFound();

            return Ok(produtoViewModel);
        }
    }
}
