using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace NerdStore.Catalogo.Domain.Events
{
    // INotificationHandler interface do Mediatr
    public class ProdutoEventHandler : INotificationHandler<ProdutoAbaixoEstoqueEvent>
    {
        private readonly IProdutoRepository _produtoRepository;

        public ProdutoEventHandler(IProdutoRepository produtoRepository)
        {
            _produtoRepository = produtoRepository;
        }

        public async Task Handle(ProdutoAbaixoEstoqueEvent mensagem, CancellationToken cancellationToken)
        {
            var produto = await _produtoRepository.ObterPorId(mensagem.AggretateId);

            //Ideia enviar um e-mail para aquisição de mais produtos
        }
    }
}
