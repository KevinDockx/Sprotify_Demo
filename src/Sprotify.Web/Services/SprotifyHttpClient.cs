using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Sprotify.Web.Services
{
    public class SprotifyHttpClient
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private HttpClient _httpClient = new HttpClient();

        public SprotifyHttpClient(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<HttpClient> GetClient()
        {
            string accessToken = string.Empty;

            var currentContext = _httpContextAccessor.HttpContext;

            //accessToken = await currentContext.Authentication.GetTokenAsync(
            //    OpenIdConnectParameterNames.AccessToken);

            // should we renew access & refresh tokens?
            // get expires_at value
            var expires_at = await currentContext.Authentication.GetTokenAsync("expires_at");

            // compare - make sure to use the exact date formats for comparison 
            // (UTC, in this case)
            if (string.IsNullOrWhiteSpace(expires_at)
                || ((DateTime.Parse(expires_at).AddSeconds(-60)).ToUniversalTime()
                < DateTime.UtcNow))
            {
                accessToken = await RenewTokens();
            }
            else
            {
                // get access token
                accessToken = await currentContext.Authentication
                    .GetTokenAsync(OpenIdConnectParameterNames.AccessToken);
            }


            if (!string.IsNullOrWhiteSpace(accessToken))
            {
                _httpClient.SetBearerToken(accessToken);
            }

            var baseUri = Startup.Configuration.GetValue<string>("ApiBaseUri");
            _httpClient.BaseAddress = new Uri(baseUri);

            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            return _httpClient;
        }

        private async Task<string> RenewTokens()
        {
            // get the current HttpContext to access the tokens
            var currentContext = _httpContextAccessor.HttpContext;

            // get the metadata
            var discoveryClient = new DiscoveryClient("https://localhost:44375/");
            var metaDataResponse = await discoveryClient.GetAsync();

            // create a new token client to get new tokens
            var tokenClient = new TokenClient(metaDataResponse.TokenEndpoint,
                "sprotifyclient", "secret");

            // get the saved refresh token
            var currentRefreshToken = await currentContext.Authentication
                .GetTokenAsync(OpenIdConnectParameterNames.RefreshToken);

            // refresh the tokens
            var tokenResult = await tokenClient.RequestRefreshTokenAsync(currentRefreshToken);

            if (!tokenResult.IsError)
            {
                // Save the tokens. 

                // get auth info
                var authenticateInfo = await currentContext.Authentication
                    .GetAuthenticateInfoAsync("Cookies");

                // create a new value for expires_at, and save it
                var expiresAt = DateTime.UtcNow + TimeSpan.FromSeconds(tokenResult.ExpiresIn);
                authenticateInfo.Properties.UpdateTokenValue("expires_at",
                    expiresAt.ToString("o", CultureInfo.InvariantCulture));

                authenticateInfo.Properties.UpdateTokenValue(
                    OpenIdConnectParameterNames.AccessToken,
                    tokenResult.AccessToken);
                authenticateInfo.Properties.UpdateTokenValue(
                    OpenIdConnectParameterNames.RefreshToken,
                    tokenResult.RefreshToken);

                // we're signing in again with the new values.  
                await currentContext.Authentication.SignInAsync("Cookies",
                    authenticateInfo.Principal, authenticateInfo.Properties);

                // return the new access token 
                return tokenResult.AccessToken;
            }
            else
            {
                throw new Exception("Problem encountered while refreshing tokens.",
                    tokenResult.Exception);
            }
        }


    }
}