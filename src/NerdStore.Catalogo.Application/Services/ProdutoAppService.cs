﻿using AutoMapper;
using NerdStore.Catalogo.Application.ViewModels;
using NerdStore.Catalogo.Domain;
using NerdStore.Core.DomainObjects;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NerdStore.Catalogo.Application.Services
{
    public class ProdutoAppService : IProdutoAppService
    {
        private readonly IProdutoRepository _produtoRepository;
        private readonly IEstoqueService _estoqueService;
        private readonly IMapper _mapper;

        public ProdutoAppService(IProdutoRepository produtoRepository,
                                 IEstoqueService estoqueService,
                                 IMapper mapper)
        {
            _produtoRepository = produtoRepository;
            _estoqueService = estoqueService;
            _mapper = mapper;
        }

        public async Task AdicionarProduto(ProdutoViewModel produtoViewModel)
        {
            var produto = _mapper.Map<Produto>(produtoViewModel);
            _produtoRepository.Adicionar(produto);
            await _produtoRepository.UnitOfWork.Commit();
        }

        public async Task AtualizarProduto(ProdutoViewModel produtoViewModel)
        {
            var produto = _mapper.Map<Produto>(produtoViewModel);
            _produtoRepository.Atualizar(produto);
            await _produtoRepository.UnitOfWork.Commit();
        }

        public async Task<ProdutoViewModel> DebitarEstqoue(Guid id, int quantidade)
        {
            if (!_estoqueService.DebitarEstoque(id, quantidade).Result)
            {
                throw new DomainException("Falha ao debitar estoque");
            }

            return _mapper.Map<ProdutoViewModel>(await _produtoRepository.ObterPorId(id));
        }

        public async Task<IEnumerable<ProdutoViewModel>> ObterPorCategoria(int codigo)
        {
            return _mapper.Map<IEnumerable<ProdutoViewModel>>(await _produtoRepository.ObterPorCategoria(codigo));
        }

        public async Task<IEnumerable<CategoriaViewModel>> ObterPorCategorias()
        {
            return _mapper.Map<IEnumerable<CategoriaViewModel>>(await _produtoRepository.ObterCategorias());
        }

        public async Task<ProdutoViewModel> ObterPorId(Guid id)
        {
            return _mapper.Map<ProdutoViewModel>(await _produtoRepository.ObterPorId(id));
        }

        public async Task<IEnumerable<ProdutoViewModel>> ObterTodos()
        {
            return _mapper.Map<IEnumerable<ProdutoViewModel>>(await _produtoRepository.ObterTodos());
        }

        public async Task<ProdutoViewModel> ReporEstoque(Guid id, int quantidade)
        {
            if (!_estoqueService.ReporEstoque(id, quantidade).Result)
            {
                throw new DomainException("Falha ao repor estoque");
            }
            return _mapper.Map<ProdutoViewModel>(await _produtoRepository.ObterPorId(id));
        }

        public async Task AdicionarCategoria(CategoriaViewModel categoriaViewModel)
        {
            var categoria = _mapper.Map<Categoria>(categoriaViewModel);
            _produtoRepository.Adicionar(categoria);
            await _produtoRepository.UnitOfWork.Commit();
        }

        public async Task AtualizarCategoria(CategoriaViewModel categoriaViewModel)
        {
            var categoria = _mapper.Map<Categoria>(categoriaViewModel);
            _produtoRepository.Atualizar(categoria);
            await _produtoRepository.UnitOfWork.Commit();
        }

        public void Dispose()
        {
            _produtoRepository?.Dispose();
            _estoqueService?.Dispose();
        }
    }
}
