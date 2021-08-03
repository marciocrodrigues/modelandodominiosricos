using NerdStore.Core.Messages;
using System;

namespace NerdStore.Vendas.Application.Events
{
    public class PedidoProdutoAtualiadoEvent : Event
    {
        public PedidoProdutoAtualiadoEvent(Guid clienteId, Guid pedidoId, Guid produtoId, int quantidade)
        {
            AggretateId = pedidoId;
            ClienteId = clienteId;
            PedidoId = pedidoId;
            ProdutoId = produtoId;
            Quantidade = quantidade;
        }

        public Guid ClienteId { get; private set; }
        public Guid PedidoId { get; private set; }
        public Guid ProdutoId { get; private set; }
        public int Quantidade { get; private set; }
    }
}
