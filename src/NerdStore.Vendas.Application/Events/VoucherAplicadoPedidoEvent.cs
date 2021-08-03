using NerdStore.Core.Messages;
using System;

namespace NerdStore.Vendas.Application.Events
{
    public class VoucherAplicadoPedidoEvent : Event
    {
        public VoucherAplicadoPedidoEvent(Guid clienteId, Guid pedidoId, Guid voucherId)
        {
            AggretateId = pedidoId;
            ClienteId = clienteId;
            PedidoId = pedidoId;
            VoucherId = voucherId;
        }

        public Guid ClienteId { get; private set; }
        public Guid PedidoId { get; private set; }
        public Guid VoucherId { get; private set; }
    }
}
