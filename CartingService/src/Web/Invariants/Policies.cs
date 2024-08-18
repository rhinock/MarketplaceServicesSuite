namespace Carting.Web.Invariants;

public static class Policies
{
    public const string Buyer = "Buyer";
    public const string Manager = "Manager";
    public const string AllPolicies = $"{Buyer}, {Manager}";
}