using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Microsoft.Extensions.Options;
using InsightAPISample.WebApp.Models;
using System.Text;

namespace InsightAPISample.WebApp.Repositories
{
    /// <summary>
    /// this class encapsulates all calls the the new VantagePoint 2.0 REST API
    /// The documentation can be found here: https://api.deltek.com/Product/Vantagepoint/api/
    /// This sample uses the restSharp library from http://restsharp.org to simplify connectivity
    /// </summary>
    public class VantagePointRepository : IRepository
    {
        private Models.VantagePointSettings Settings;

        //public VantagePointRepository(Models.VantagePointSettings settings)
        //{
        //    Settings = settings;
        //}

        /// <summary>
        /// using the IOptionsSnapshot will allow the application to reload the settings if necessary
        /// </summary>
        /// <param name="options"></param>
        public VantagePointRepository(IOptionsSnapshot<Models.VantagePointSettings> options)
        {
            Settings = options.Value;
        }

        /// <summary>
        /// gets a default http client
        /// </summary>
        /// <returns></returns>
        public HttpClient GetVantagePointClient()
        {
            //set up client
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(Settings.BaseURL);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            return client;
        }

        /// <summary>
        /// gets a http client that has the token already assigned in authentication
        /// </summary>
        /// <returns></returns>
        public async Task<HttpClient> GetAuthorizedVantagePointClientAsync()
        {
            HttpClient client = GetVantagePointClient();

            //for simplicity sake we ALWAYS request a new token each time you request a 
            //new authorized client. Instead here you should check if there is an existing 
            //token (retrieve from some settings) and check its validity.
            //If the token expired, request a refreshed token via RefreshTokenAsync
            
            //TokenInfo token = GetFromSettings();
            //token = await RefreshTokenAsync(token);

            TokenInfo token = await GetVantagePointTokenAsync(client);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                "Bearer", token.access_token);

            return client;
        }

        /// <summary>
        /// gets the token. Needs additional error handling
        /// </summary>
        /// <returns></returns>
        public async Task<TokenInfo> GetVantagePointTokenAsync(HttpClient client)
        {
            Dictionary<string, string> values = new Dictionary<string, string>();
            values.Add("username", Settings.Username);
            values.Add("password", Settings.Password);
            values.Add("grant_type", "password");
            values.Add("integrated", "N");
            values.Add("database", Settings.Database);
            values.Add("client_Id", Settings.ClientId);
            values.Add("client_secret", Settings.ClientSecret);

            var content = new FormUrlEncodedContent(values);

            HttpResponseMessage response = await client.PostAsync("token", content);
            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();
                TokenInfo token = JsonConvert.DeserializeObject<TokenInfo>(json);

                return token;
            }

            return null;
        }

        public async Task<TokenInfo> RefreshTokenAsync(HttpClient client, TokenInfo currentToken)
        {
            Dictionary<string, string> values = new Dictionary<string, string>();
            values.Add("refresh_token", currentToken.refresh_token);
            values.Add("grant_type", "refresh_token");
            values.Add("client_Id", Settings.ClientId);
            values.Add("client_secret", Settings.ClientSecret);

            var content = new FormUrlEncodedContent(values);

            HttpResponseMessage response = await client.PostAsync("token", content);
            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();
                TokenInfo token = JsonConvert.DeserializeObject<TokenInfo>(json);

                return token;
            }

            return null;
        }

        #region http methods
        public async Task<dynamic> GetAsync(HttpClient authorizedClient, string requestUri)
        {
            HttpResponseMessage response = await authorizedClient.GetAsync(requestUri);
            if (response.IsSuccessStatusCode)
            {
                string strContent = await response.Content.ReadAsStringAsync();
                dynamic obj = JsonConvert.DeserializeObject(strContent);
                return obj;
            }
            return null;
        }

        public async Task<dynamic> PostAsync(HttpClient authorizedClient, string requestUri, dynamic data)
        {
            string strJSON = JsonConvert.SerializeObject(data);
            StringContent jsonContent = new StringContent(strJSON, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await authorizedClient.PostAsync(requestUri, jsonContent);
            if (response.IsSuccessStatusCode)
            {
                string strContent = await response.Content.ReadAsStringAsync();
                dynamic obj = JsonConvert.DeserializeObject(strContent);
                return obj;
            }
            return null;
        }

        public async Task<T> GetAsync<T>(HttpClient authorizedClient, string requestUri)
        {
            HttpResponseMessage response = await authorizedClient.GetAsync(requestUri);
            if (response.IsSuccessStatusCode)
            {
                string strContent = await response.Content.ReadAsStringAsync();
                T obj = JsonConvert.DeserializeObject<T>(strContent);
                return obj;
            }

            throw new ApplicationException($"REST call failed with status {response.StatusCode} {response.ReasonPhrase}");
        }

        public async Task<T> PostAsync<T>(HttpClient authorizedClient, string requestUri, dynamic data)
        {
            string strJSON = JsonConvert.SerializeObject(data);
            StringContent jsonContent = new StringContent(strJSON, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await authorizedClient.PostAsync(requestUri, jsonContent);
            if (response.IsSuccessStatusCode)
            {
                string strContent = await response.Content.ReadAsStringAsync();
                T obj = JsonConvert.DeserializeObject<T>(strContent);
                return obj;
            }

            throw new ApplicationException($"REST call failed with status {response.StatusCode} {response.ReasonPhrase}"); ;
        }


        #endregion

        #region load and save settings
        public VantagePointSettings GetSettings()
        {
            var model = new VantagePointSettings();
            return model;

        }

        public bool SaveSettings(Models.VantagePointSettings model)
        {
            return true;
        }
        #endregion
    }
}
