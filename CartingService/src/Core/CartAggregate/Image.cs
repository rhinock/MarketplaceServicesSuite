using Ardalis.SharedKernel;

namespace Carting.Core.CartAggregate;

public class Image : ValueObject
{
    public string Url { get; set; } = null!;
    public string AltText { get; set; } = null!;

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Url;
        yield return AltText;
    }
}