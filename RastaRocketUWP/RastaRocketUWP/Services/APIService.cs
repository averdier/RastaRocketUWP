using Newtonsoft.Json;
using RastaRocketUWP.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace RastaRocketUWP.Services
{
    public class AuthToken
    {
        public string Token { get; set; }
    }

    public class Need
    {
        public string CreatedAt { get; set; }
        public string Status { get; set; }
        public string Title { get; set; }
        public string Customer { get; set; }
        public string Id { get; set; }
        public string Contact { get; set; }
        public string StartAtLatest { get; set; }
    }

    public class NeedContainer
    {
        public List<Need> Needs { get; set; }
    }


    public class APIService
    {
        private const string _serverUrl = "https://levasseurantoine.ovh/rastarocket/api/";
        private string _username;
        private string _password;
        private string _token;
        public string Token { get { return _token; } }

        public APIService() { }

        private HttpClient GetHttpClientBasicAuth(string username, string password)
        {
            HttpClient client = new HttpClient();

            client.DefaultRequestHeaders.Add("Accept", "application/json");
            var byteArray = Encoding.ASCII.GetBytes(username + ":" + password);
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

            return client;
        }

        private HttpClient GetHttpClientToken(string token)
        {
            HttpClient client = new HttpClient();

            client.DefaultRequestHeaders.Add("Accept", "application/json");
            client.DefaultRequestHeaders.Add("Authorization", "Token " + token);

            return client;
        }

        public async Task<Boolean> LoginAsync(string username, string password)
        {
            string token = string.Empty;

            HttpClient client = this.GetHttpClientBasicAuth(username, password);

            HttpResponseMessage response = await client.GetAsync(_serverUrl + "token/");

            var success = false;

            switch (response.StatusCode)
            {
                case System.Net.HttpStatusCode.OK:
                    var content = await response.Content.ReadAsStringAsync();
                    var authToken = JsonConvert.DeserializeObject<AuthToken>(content);
                    _token = authToken.Token;
                    success = true;
                    break;

                case System.Net.HttpStatusCode.Unauthorized:
                    throw new AuthorizationException("Unknown user");
            }

            return success;
        }

        private async Task RenewAuthToken()
        {
            HttpClient client = this.GetHttpClientBasicAuth(_username, _password);

            HttpResponseMessage response = await client.GetAsync(_serverUrl + "token/");

            switch (response.StatusCode)
            {
                case System.Net.HttpStatusCode.OK:
                    var content = await response.Content.ReadAsStringAsync();
                    var authToken = JsonConvert.DeserializeObject<AuthToken>(content);
                    _token = authToken.Token;
                    break;

                case System.Net.HttpStatusCode.Unauthorized:
                    throw new AuthorizationException("Unknown user");

                default:
                    throw new Exception(String.Format("Unable to get token : {0}", response.StatusCode));

            }
        }

        public async Task<NeedContainer> GetNeedContainerWithRetryAsync(bool retry = true)
        {
            NeedContainer container = null;
            try
            {
                container = await GetNeedContainerAsync(_token);
            }
            catch (AuthorizationException)
            {
                if (retry)
                {
                    await RenewAuthToken();
                    container = await GetNeedContainerWithRetryAsync(false);
                }
                else
                {
                    throw;
                }
            }
            return container;
        }

        public async Task<NeedContainer> GetNeedContainerAsync(string token)
        {
            NeedContainer container = null;

            HttpClient client = this.GetHttpClientToken(token);

            HttpResponseMessage response = await client.GetAsync(_serverUrl + "needs/");

            switch (response.StatusCode)
            {
                case System.Net.HttpStatusCode.OK:
                    var content = await response.Content.ReadAsStringAsync();
                    container = JsonConvert.DeserializeObject<NeedContainer>(content);
                    break;

                case System.Net.HttpStatusCode.Unauthorized:
                    throw new AuthorizationException("Unknown token");
            }
            return container;
        }
    }
}
