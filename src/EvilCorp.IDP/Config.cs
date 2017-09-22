using IdentityServer4.Models;
using IdentityServer4.Test;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;

namespace EvilCorp.IDP
{
    public static class Config
    {
        public static List<TestUser> GetUsers()
        {
            return new List<TestUser>
            {
                new TestUser
                {
                    SubjectId = "45ae3bb3-5922-42ed-a441-3f78356f3755",
                    Username = "Wesley",
                    Password = "password",

                    Claims = new List<Claim>
                    {
                        new Claim("given_name", "Wesley"),
                        new Claim("family_name", "Cabus"),
                        new Claim("address", "1, Main Road")
                    }
                },
                new TestUser
                {
                    SubjectId = "24ec92ba-e6a3-421f-a599-2a0bf88c807a",
                    Username = "Kevin",
                    Password = "password",

                    Claims = new List<Claim>
                    {
                        new Claim("given_name", "Kevin"),
                        new Claim("family_name", "Dockx"),
                        new Claim("address", "2, Big Street"),
                    }
                }
            };
        }

        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                new Client
                {
                    ClientId = "sprotifyclient",
                    AllowedGrantTypes = GrantTypes.Hybrid,
 
                    // secret for authentication
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },
                        
                    // scopes that client has access to
                    AllowedScopes = { "openid", "profile", "address", "sprotifyapi" },

                    RedirectUris = { "https://localhost:44396/signin-oidc" },
                    PostLogoutRedirectUris = { "https://localhost:44396/signout-callback-oidc" },

                    AllowOfflineAccess = true
                }
            };
        }

        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource("sprotifyapi", "Sprotify API")
            };
        }

        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Address(),
            };
        }
    }

}
