using AutoMapper;
using Catalog.Core.Categories;
using Catalog.UseCases.Categories.Update;

namespace Catalog.UseCases.AutoMapper.Profiles;

public class UpdateCategoryCommandToCategoryProfile : Profile
{
    public UpdateCategoryCommandToCategoryProfile()
    {
        CreateMap<UpdateCategoryCommand, Category>()
            .ForMember(d => d.Id, opt => opt.MapFrom(s => s.Id))
            .ForMember(d => d.Name, opt => opt.MapFrom(s => s.Name))
            .ForMember(d => d.Image, opt => opt.MapFrom(s => s.Image))
            .ForMember(d => d.ParentId, opt => opt.MapFrom(s => s.ParentId));
    }
}