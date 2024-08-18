namespace Carting.Web.Invariants;

public static class Claims
{
    public const string ClaimType = "client_permission";
    public static string[] BuyerClaimValues => ["Read"];
    public static string[] ManagerClaimValues => ["Read", "Create", "Delete", "Update"];
}