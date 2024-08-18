using AutoMapper;
using Carting.Core.CartAggregate;
using Carting.Responses;

namespace Carting.UseCases.AutoMapper.Profiles;

public class CartToCartResponseProfile : Profile
{
    public CartToCartResponseProfile()
    {
        CreateMap<Cart, CartResponse>()
            .ForMember(d => d.Id, opt => opt.MapFrom(s => s.Id))
            .ForMember(d => d.Items, opt => opt.MapFrom(s => s.Items));
    }
}