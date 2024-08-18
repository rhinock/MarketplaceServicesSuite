using AutoMapper;
using Catalog.Core.Items;
using Catalog.UseCases.Items.Update;

namespace Catalog.UseCases.AutoMapper.Profiles;

public class UpdateItemCommandToItemProfile : Profile
{
    public UpdateItemCommandToItemProfile()
    {
        CreateMap<UpdateItemCommand, Item>()
            .ForMember(d => d.Id, opt => opt.MapFrom(s => s.Id))
            .ForMember(d => d.Name, opt => opt.MapFrom(s => s.Name))
            .ForMember(d => d.Description, opt => opt.MapFrom(s => s.Description))
            .ForMember(d => d.CategoryId, opt => opt.MapFrom(s => s.CategoryId))
            .ForMember(d => d.Price, opt => opt.MapFrom(s => s.Price))
            .ForMember(d => d.Amount, opt => opt.MapFrom(s => s.Amount));
    }
}
