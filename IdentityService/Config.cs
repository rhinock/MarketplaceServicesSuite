using System.Security.Claims;
using IdentityModel;
using IdentityServer4.Models;

namespace Identity;

public static class Config
{
    private const string ManagerClientId = "Manager";
    private const string BuyerClientId = "Buyer";
    private const string Read = "Read";
    private const string Create = "Create";
    private const string Update = "Update";
    private const string Delete = "Delete";
    private const string ClaimType = "permission";

    public static IEnumerable<IdentityResource> GetIdentityResources() =>
        [
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
            new("roles", [JwtClaimTypes.Role, ClaimTypes.Role])
        ];

    public static IEnumerable<ApiScope> GetApiScopes() =>
       GetCartApiScopes().Union(GetCatalogApiScopes());

    public static IEnumerable<Client> GetClients() =>
        [
            new()
            {
                ClientId = ManagerClientId,
                AllowedGrantTypes = GrantTypes.ClientCredentials,
                ClientSecrets = { new Secret("secret".Sha256()) },
                AllowedScopes =
                    new List<string> { "openid", "pfofile" }
                    .Union(GetApiScopes().Select(x => x.Name)).ToList(),
                AllowedCorsOrigins = ["http://localhost:5178", "http://localhost:8083"],
                AlwaysIncludeUserClaimsInIdToken = true,
                Claims =
                [
                    new(ClaimType, $"{Read}"),
                    new(ClaimType, $"{Create}"),
                    new(ClaimType, $"{Update}"),
                    new(ClaimType, $"{Delete}"),
                ]
            },
            new()
            {
                ClientId = BuyerClientId,
                AllowedGrantTypes = GrantTypes.ClientCredentials,
                ClientSecrets = { new Secret("secret".Sha256()) },
                AllowedScopes =
                    new List<string> { "openid", "pfofile" }
                    .Union(GetApiScopes().Select(x => x.Name)).ToList(),
                AllowedCorsOrigins = ["http://localhost:5138", "http://localhost:8082"],
                AlwaysIncludeUserClaimsInIdToken = true,
                Claims =
                [
                    new(ClaimType, $"{Read}"),
                ]
            }
        ];

    public static IEnumerable<ApiResource> GetApiResources()
    {
        return
        [
            new ApiResource("catalogapi", "Catalog API")
            {
                Scopes = GetCatalogApiScopes().Select(x => x.Name).ToList(),
                UserClaims = [JwtClaimTypes.Role, ClaimTypes.Role]
            },
            new ApiResource("cartapi", "Cart API")
            {
                Scopes = GetCartApiScopes().Select(x => x.Name).ToList(),
                UserClaims = [JwtClaimTypes.Role, ClaimTypes.Role]
            }
        ];
    }

    private static IEnumerable<ApiScope> GetCatalogApiScopes() =>
        [
            new($"{ManagerClientId}.{Read}"),
            new($"{ManagerClientId}.{Create}"),
            new($"{ManagerClientId}.{Update}"),
            new($"{ManagerClientId}.{Delete}"),
        ];

    private static IEnumerable<ApiScope> GetCartApiScopes() =>
       [
           new($"{BuyerClientId}.{Read}")
       ];
}