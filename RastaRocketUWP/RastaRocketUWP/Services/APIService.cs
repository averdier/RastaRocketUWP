using Newtonsoft.Json;
using RastaRocketUWP.Exceptions;
using RastaRocketUWP.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

    public class APIService
    {
        private const string _serverUrl = "https://levasseurantoine.ovh/rastarocket/api/";
        private string _username;
        private string _password;
        private string _token;
        public string Token { get { return _token; } }

        public APIService() { }

        public APIService(string username, string password)
        {
            this._username = username;
            this._password = password;
        }

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

        public async Task<NeedContainer> GetNeedContainerWithRetryAsync(string titlePrefix = "", string status = "", bool retry = true)
        {
            NeedContainer container = null;
            try
            {
                container = await GetNeedContainerAsync(_token, titlePrefix, status);
            }
            catch (AuthorizationException)
            {
                if (retry)
                {
                    await RenewAuthToken();
                    container = await GetNeedContainerAsync(_token, titlePrefix, status);
                }
                else
                {
                    throw;
                }
            }
            return container;
        }

        public async Task<Boolean> DeleteNeedWithRetryAsync(string need_id, bool retry = true)
        {
            var success = false;
            try
            {
                success = await DeleteNeedAsync(_token, need_id);
            }
            catch (AuthorizationException)
            {
                if (retry)
                {
                    await RenewAuthToken();
                    success = await DeleteNeedAsync(_token, need_id);
                }
                else
                {
                    throw;
                }
            }
            return success;
        }

        public async Task<NeedModel> GetNeedFromIdWithRetryAsync(string need_id, bool retry = true)
        {
            NeedModel need = null;
            try
            {
                need = await GetNeedFromIdAsync(_token, need_id);
            }
            catch (AuthorizationException)
            {
                if (retry)
                {
                    await RenewAuthToken();
                    need = await GetNeedFromIdAsync(_token, need_id);
                }
                else
                {
                    throw;
                }
            }
            return need;
        }

        public async Task<List<CustomerModel>> AutoSuggestCustomerWithRetryAsync(string prefix, bool retry = true)
        {
            List<CustomerModel> customers = new List<CustomerModel>();
            try
            {
                customers = await AutoSuggestCustomerAsync(_token, prefix);
            }
            catch (AuthorizationException)
            {
                if (retry)
                {
                    await RenewAuthToken();
                    customers = await AutoSuggestCustomerAsync(_token, prefix);
                }
                else
                {
                    throw;
                }
            }
            
            return customers;
        }

        public async Task<List<PersonModel>> AutoSuggestContactWithRetryAsync(string prefix, bool retry = true)
        {
            List<PersonModel> contacts = new List<PersonModel>();
            try
            {
                contacts = await AutoSuggestContactAsync(_token, prefix);
            }
            catch (AuthorizationException)
            {
                if (retry)
                {
                    await RenewAuthToken();
                    contacts = await AutoSuggestContactAsync(_token, prefix);
                }
                else
                {
                    throw;
                }
            }

            return contacts;
        }

        public async Task<List<PersonModel>> AutoSuggestConsultantWithRetryAsync(string prefix, bool retry = true)
        {
            List<PersonModel> contacts = new List<PersonModel>();
            try
            {
                contacts = await AutoSuggestConsultantAsync(_token, prefix);
            }
            catch (AuthorizationException)
            {
                if (retry)
                {
                    await RenewAuthToken();
                    contacts = await AutoSuggestConsultantAsync(_token, prefix);
                }
                else
                {
                    throw;
                }
            }

            return contacts;
        }

        public async Task<NeedModel> PostNeedWithRetryAsync(NeedPostModel model, bool retry = true)
        {
            NeedModel need = null;
            try
            {
                need = await PostNeedAsync(_token, model);
            }
            catch (AuthorizationException)
            {
                if (retry)
                {
                    await RenewAuthToken();
                    need = await PostNeedAsync(_token, model);
                }
                else
                {
                    throw;
                }
            }

            return need;
        }

        public async Task<NeedContainer> GetNeedContainerAsync(string token, string titlePrefix = "", string status = "")
        {
            NeedContainer container = null;

            HttpClient client = this.GetHttpClientToken(token);

            string parameters = "";

            if (titlePrefix != null && titlePrefix != "" && titlePrefix != " " && titlePrefix != String.Empty)
            {
                parameters += "?title=" + titlePrefix;
            }

            if (status != null && status != "" && status != String.Empty && status != "Tous")
            {
                if (parameters != "" && parameters != String.Empty) { parameters += "&"; }
                else { parameters = "?"; }
                parameters += "status=" + status;
            }
            Debug.WriteLine(parameters);
            HttpResponseMessage response = await client.GetAsync(_serverUrl + "needs/" + parameters);

            switch (response.StatusCode)
            {
                case System.Net.HttpStatusCode.OK:
                    var content = await response.Content.ReadAsStringAsync();
                    Debug.WriteLine(content);
                    container = JsonConvert.DeserializeObject<NeedContainer>(content);
                    break;

                case System.Net.HttpStatusCode.Unauthorized:
                    throw new AuthorizationException("Unknown token");
            }
            return container;
        }

        public async Task<NeedModel> GetNeedFromIdAsync(string token, string need_id)
        {
            NeedModel need = null;

            HttpClient client = this.GetHttpClientToken(token);

            HttpResponseMessage response = await client.GetAsync(_serverUrl + "needs/" + need_id);

            switch (response.StatusCode)
            {
                case System.Net.HttpStatusCode.OK:
                    var content = await response.Content.ReadAsStringAsync();
                    need = JsonConvert.DeserializeObject<NeedModel>(content);
                    break;

                case System.Net.HttpStatusCode.Unauthorized:
                    throw new AuthorizationException("Unknown token");
            }
            return need;
        }

        public async Task<bool> DeleteNeedAsync(string token, string need_id)
        {
            var success = false;

            HttpClient client = this.GetHttpClientToken(token);

            HttpResponseMessage response = await client.DeleteAsync(_serverUrl + "needs/" + need_id);

            switch (response.StatusCode)
            {
                case System.Net.HttpStatusCode.NoContent:
                    success = true;
                    break;

                case System.Net.HttpStatusCode.Unauthorized:
                    throw new AuthorizationException("Unknown token");
            }

            return success;
        }

        public async Task<List<CustomerModel>> AutoSuggestCustomerAsync(string token, string prefix)
        {
            List<CustomerModel> customers = new List<Models.CustomerModel>();

            HttpClient client = this.GetHttpClientToken(token);

            HttpResponseMessage response = await client.GetAsync(_serverUrl + "customers/?name=" + prefix);

            switch (response.StatusCode)
            {
                case System.Net.HttpStatusCode.OK:
                    var content = await response.Content.ReadAsStringAsync();
                    CustomerContainer container = JsonConvert.DeserializeObject<CustomerContainer>(content);

                    foreach (var customer in container.Customers)
                    {
                        customers.Add(customer);
                    }
                    break;

                case System.Net.HttpStatusCode.Unauthorized:
                    throw new AuthorizationException("Unknown token");
            }

            return customers;
        }

        public async Task<List<PersonModel>> AutoSuggestContactAsync(string token, string prefix)
        {
            List<PersonModel> contacts = new List<Models.PersonModel>();

            HttpClient client = this.GetHttpClientToken(token);

            HttpResponseMessage response = await client.GetAsync(_serverUrl + "contacts/?name=" + prefix);

            switch (response.StatusCode)
            {
                case System.Net.HttpStatusCode.OK:
                    var content = await response.Content.ReadAsStringAsync();
                    ContactContainer container = JsonConvert.DeserializeObject<ContactContainer>(content);

                    foreach (var contact in container.Contacts)
                    {
                        contacts.Add(contact);
                    }
                    break;

                case System.Net.HttpStatusCode.Unauthorized:
                    throw new AuthorizationException("Unknown token");
            }

            return contacts;
        }

        public async Task<List<PersonModel>> AutoSuggestConsultantAsync(string token, string prefix)
        {
            List<PersonModel> contacts = new List<Models.PersonModel>();

            HttpClient client = this.GetHttpClientToken(token);

            HttpResponseMessage response = await client.GetAsync(_serverUrl + "consultants/?name=" + prefix);

            switch (response.StatusCode)
            {
                case System.Net.HttpStatusCode.OK:
                    var content = await response.Content.ReadAsStringAsync();
                    ConsultantContainer container = JsonConvert.DeserializeObject<ConsultantContainer>(content);

                    foreach (var contact in container.Consultants)
                    {
                        contacts.Add(contact);
                        Debug.WriteLine(contact.Name);
                    }
                    break;

                case System.Net.HttpStatusCode.Unauthorized:
                    throw new AuthorizationException("Unknown token");
            }

            Debug.WriteLine(contacts);
            return contacts;
        }

        public async Task<NeedModel> PostNeedAsync(string token, NeedPostModel model)
        {
            HttpClient client = this.GetHttpClientToken(token);

            Uri resourceUri = new Uri(_serverUrl + "needs/");

            string jsonObject = "";
            jsonObject = JsonConvert.SerializeObject(model);

            var response = await client.PostAsync(resourceUri, new System.Net.Http.StringContent(jsonObject, System.Text.Encoding.UTF8, "application/json"));

            var strResponse = await response.Content.ReadAsStringAsync();
            Debug.WriteLine(strResponse);
            var need = JsonConvert.DeserializeObject<NeedModel>(strResponse);

            return need;
        }
    }
}
