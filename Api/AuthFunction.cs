using System.Collections.Generic;
using System;
using System.IO;
using System.Net;
using System.Net.Http.Json;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Auth0.ManagementApi;
using BlazorApp.Shared.Auth;
using System.Linq;

namespace Api
{
    public class AuthFunction
    {
        private readonly ILogger _logger;

        public AuthFunction(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<AuthFunction>();
        }

        #region Utility Methods
        private static async Task<string> GetAccessTokenAsync()
        {
            var auth0Domain = Environment.GetEnvironmentVariable("AUTH0_DOMAIN");
            var auth0ClientId = Environment.GetEnvironmentVariable("AUTH0_CLIENT_ID");
            var auth0ClientSecret = Environment.GetEnvironmentVariable("AUTH0_CLIENT_SECRET");

            using var client = new HttpClient();
            client.BaseAddress = new Uri(auth0Domain);
            var response = await client.PostAsync("/oauth/token", new FormUrlEncodedContent(
                new Dictionary<string, string>
                {
                            { "grant_type", "client_credentials" },
                            { "client_id", auth0ClientId },
                            { "client_secret", auth0ClientSecret },
                            { "audience", $"{auth0Domain}/api/v2/" }
                }
            ));

            if (!response.IsSuccessStatusCode)
                return null;

            var token = await response.Content.ReadFromJsonAsync<BearerToken>();

            return token.Access_Token;
        }

        private async Task<IEnumerable<string>> GetRolesFromManagementApiAsync(ClientPrincipal data)
        {
            var auth0Domain = Environment.GetEnvironmentVariable("AUTH0_DOMAIN");

            try
            {
                //get token for API
                var token = await GetAccessTokenAsync();

                //call API and get roles
                var client = new ManagementApiClient(token, new Uri($"{auth0Domain}/api/v2/"));
                var roles = await client.Users.GetRolesAsync(data.UserId);
                return roles?.Select(r => r.Name);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while retreiving roles from Auth0");
                return null;
            }
        }
        #endregion

        [Function("GetRoles")]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req)
        {
            _logger.LogInformation("Request for GetRoles retrieved");
            var data = await req.ReadFromJsonAsync<ClientPrincipal>();

            //get roles from Auth0 Management API
            var roles = await GetRolesFromManagementApiAsync(data);
            roles ??= Array.Empty<string>();
            var result = new { roles = roles.ToArray() };

            //create response
            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(result);

            return response;
        }
    }
}
