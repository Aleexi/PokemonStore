using Duende.IdentityServer.Models;
using IdentityModel;

namespace IdentityService;

public static class Config
{
    public static IEnumerable<IdentityResource> IdentityResources =>
        new IdentityResource[]
        {
            // return one access token and one profile token containing user information 
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
        };

    public static IEnumerable<ApiScope> ApiScopes =>
        new List<ApiScope>
        {
            new ApiScope("pokemonApp", "Pokemon App Full Access")
        };

    public static IEnumerable<Client> Clients =>
        new List<Client>
        {
            // For development -> Postman 
            new Client
            {
                ClientId = "postman",
                ClientName = "Postman",
                AllowedScopes = {"openid", "profile", "pokemonApp"},
                RedirectUris = {"http://www.getpostman.com/oauth2/callback"},
                ClientSecrets = new Secret[]
                {
                    new Secret("NotASecret".ToSha256())
                },
                AllowedGrantTypes = { GrantType.ResourceOwnerPassword }
            }
        };
}
