using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using AspNetCoreOAuth2Sample.Model;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Linq;

namespace AspNetCoreOAuth2Sample.Services
{
    public class ApiService : IApiService
    {
        private readonly IOptions<Auth0Settings> _auth0Settings;
        private readonly string _apiUrl;
        private readonly string _authUrl;
        private Token _token;

        public ApiService(IOptions<Auth0Settings> auth0Settings)
        {
            _auth0Settings = auth0Settings;
            _apiUrl = $"https://{_auth0Settings.Value.Domain}/api/v2/";
            _authUrl = $"https://{_auth0Settings.Value.Domain}/oauth/token";

            JsonConvert.DefaultSettings = () => new JsonSerializerSettings()
            {
                ContractResolver = new DefaultContractResolver()
                {
                    NamingStrategy = new SnakeCaseNamingStrategy()
                }
            };
        }
        /// <summary>
        /// This method requires read:clients scope for the ManagementApi Client
        /// </summary>
        /// <returns>A list of clients belonging to the tenants</returns>
        public async Task<IEnumerable<string>> GetAllClients()
        {
            var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(_apiUrl);

            var token = await GetAccessToken();

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(token.TokenType, token.AccessToken);
            var response = await httpClient.GetAsync("clients?fields=name%2Cglobal");
            response.EnsureSuccessStatusCode();

            var jsonData = await response.Content.ReadAsStringAsync();
            var clients = JsonConvert.DeserializeObject<IEnumerable<Client>>(jsonData).Where(c => !c.Global).Select(c => c.Name);
            return clients;
        }
        /// <summary>
        /// This method requires read:rules scope for the ManagementApi Client
        /// </summary>
        /// <returns>A list of rules associated with this tenant</returns>
        public async Task<IEnumerable<Rule>> GetAllRules()
        {
            var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(_apiUrl);

            var token = await GetAccessToken();

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(token.TokenType, token.AccessToken);
            var response = await httpClient.GetAsync("rules");
            response.EnsureSuccessStatusCode();

            var jsonData = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<IEnumerable<Rule>>(jsonData);

        }

        private async Task<Token> GetAccessToken()
        {
            if (_token == null)
            {
                var client = new HttpClient();

                var data = new
                {
                    ClientId = _auth0Settings.Value.ManagerApiClientId,
                    ClientSecret = _auth0Settings.Value.ManagerApiClientSecret,
                    Audience = _apiUrl,
                    GrantType = "client_credentials"
                };

                var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
                var response = await client.PostAsync(_authUrl, content);
                response.EnsureSuccessStatusCode();

                var jsonData = await response.Content.ReadAsStringAsync();
                _token = JsonConvert.DeserializeObject<Token>(jsonData);
            }
            return _token;
        }
    }
}