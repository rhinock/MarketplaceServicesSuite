using AutoMapper;
using Catalog.Core.Items;
using Common.RabbitMq;

namespace Catalog.UseCases.AutoMapper.Profiles;

public class ItemToItemUpdateMessageProfile : Profile
{
    public ItemToItemUpdateMessageProfile()
    {
        CreateMap<Item, ItemUpdateMessage>()
            .ForMember(d => d.Id, opt => opt.MapFrom(s => s.Id))
            .ForMember(d => d.Name, opt => opt.MapFrom(s => s.Name))
            .ForMember(d => d.ImageUrl, opt => opt.MapFrom(s => s.Image))
            .ForMember(d => d.Price, opt => opt.MapFrom(s => s.Price))
            .ForMember(d => d.Quantity, opt => opt.MapFrom(s => s.Amount));
    }
}