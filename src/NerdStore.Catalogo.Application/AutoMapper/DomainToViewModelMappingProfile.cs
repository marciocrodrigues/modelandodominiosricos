using AutoMapper;
using NerdStore.Catalogo.Application.ViewModels;
using NerdStore.Catalogo.Domain;

namespace NerdStore.Catalogo.Application.AutoMapper
{
    public class DomainToViewModelMappingProfile : Profile
    {
        public DomainToViewModelMappingProfile()
        {
            CreateMap<Produto, ProdutoViewModel>()
                .ForMember(d => d.Largura,
                           o => o.MapFrom(s => s.Dimensoes.Largura))
                .ForMember(d => d.Altura,
                           o => o.MapFrom(s => s.Dimensoes.Altura))
                .ForMember(d => d.Profundidade,
                           o => o.MapFrom(s => s.Dimensoes.Profundidade))
                .ForMember(d => d.Categoria,
                           o => o.MapFrom((b, c) =>
                           {
                               return new CategoriaViewModel()
                               {
                                   Id = b.Categoria.Id,
                                   Codigo = b.Categoria.Codigo,
                                   Nome = b.Categoria.Nome
                               };
                           }));

            CreateMap<Categoria, CategoriaViewModel>();
        }
    }
}
