using AutoMapper;
using Carting.Core.CartAggregate;
using Carting.Responses;

namespace Carting.UseCases.AutoMapper.Profiles;

public class ImageToImageResponseProfile : Profile
{
    public ImageToImageResponseProfile()
    {
        CreateMap<Image, ImageResponse>()
            .ForMember(d => d.AltText, opt => opt.MapFrom(s => s.AltText))
            .ForMember(d => d.Url, opt => opt.MapFrom(s => s.Url));
    }
}