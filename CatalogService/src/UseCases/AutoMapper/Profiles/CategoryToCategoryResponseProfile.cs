using AutoMapper;
using Catalog.Core.Categories;
using Catalog.UseCases.Responses;

namespace Catalog.UseCases.AutoMapper.Profiles;

public class CategoryToCategoryResponseProfile : Profile
{
    public CategoryToCategoryResponseProfile()
    {
        CreateMap<Category, CategoryResponse>()
            .ForMember(d => d.Id, opt => opt.MapFrom(s => s.Id))
            .ForMember(d => d.Name, opt => opt.MapFrom(s => s.Name))
            .ForMember(d => d.Items, opt => opt.MapFrom(s => s.Items))
            .ForMember(d => d.ParentId, opt => opt.MapFrom(s => s.ParentId))
            .ForMember(d => d.Image, opt => opt.MapFrom(s => s.Image));
    }
}