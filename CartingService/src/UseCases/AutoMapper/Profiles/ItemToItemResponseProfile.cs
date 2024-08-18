using AutoMapper;
using Carting.Core.CartAggregate;
using Carting.Responses;

namespace Carting.UseCases.AutoMapper.Profiles;

public class ItemToItemResponseProfile : Profile
{
    public ItemToItemResponseProfile()
    {
        CreateMap<Item, ItemResponse>()
            .ForMember(d => d.Id, opt => opt.MapFrom(s => s.Id))
            .ForMember(d => d.Name, opt => opt.MapFrom(s => s.Name))
            .ForMember(d => d.Image, opt => opt.MapFrom(s => s.Image))
            .ForMember(d => d.Price, opt => opt.MapFrom(s => s.Price))
            .ForMember(d => d.Quantity, opt => opt.MapFrom(s => s.Quantity));
    }
}