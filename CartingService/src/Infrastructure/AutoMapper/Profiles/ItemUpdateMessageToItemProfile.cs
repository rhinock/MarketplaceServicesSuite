using AutoMapper;
using Carting.Core.CartAggregate;
using Common.RabbitMq;

namespace Carting.Infrastructure.AutoMapper.Profiles;

public class ItemUpdateMessageToItemProfile : Profile
{
    public ItemUpdateMessageToItemProfile()
    {
        CreateMap<ItemUpdateMessage, Item>()
            .ForMember(d => d.Id, opt => opt.MapFrom(s => s.Id))
            .ForMember(d => d.Name, opt => opt.MapFrom(s => s.Name))
            .ForMember(d => d.Image, opt => opt.MapFrom(s =>
                s.ImageUrl != null ? new Image { Url = s.ImageUrl, AltText = s.Name } : null))
            .ForMember(d => d.Price, opt => opt.MapFrom(s => s.Price))
            .ForMember(d => d.Quantity, opt => opt.MapFrom(s => s.Quantity));
    }
}