using AutoMapper;
using Carting.Infrastructure.AutoMapper.Profiles;

namespace CartingService.Tests;

public class ItemUpdateMessageToItemProfileTests
{
    private readonly IMapper _mapper = null!;

    public ItemUpdateMessageToItemProfileTests()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<ItemUpdateMessageToItemProfile>());
        _mapper = config.CreateMapper();
    }

    [Fact]
    public void WhenConfigurationIsValid_ReturnsSuccess()
    {
        _mapper.ConfigurationProvider.AssertConfigurationIsValid();
    }
}