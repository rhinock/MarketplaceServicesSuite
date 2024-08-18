using AutoMapper;
using Catalog.Core.Items;
using Catalog.UseCases.Items.Create;

namespace Catalog.UseCases.AutoMapper.Profiles;

public class CreateItemCommandToItemProfile : Profile
{
    public CreateItemCommandToItemProfile()
    {
        CreateMap<CreateItemCommand, Item>()
            .ForMember(d => d.Id, opt => opt.MapFrom(s => s.Id))
            .ForMember(d => d.Name, opt => opt.MapFrom(s => s.Name))
            .ForMember(d => d.Description, opt => opt.MapFrom(s => s.Description))
            .ForMember(d => d.Price, opt => opt.MapFrom(s => s.Price))
            .ForMember(d => d.Image, opt => opt.MapFrom(s => s.Image))
            .ForMember(d => d.CategoryId, opt => opt.MapFrom(s => s.CategoryId));
    }
}