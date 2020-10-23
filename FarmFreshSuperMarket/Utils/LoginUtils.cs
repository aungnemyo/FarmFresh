using FarmFreshSuperMarket.ViewModels;
using Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Http;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace FarmFreshSuperMarket.Utils
{
    public class LoginUtils
    {
        private readonly RestClient client;
        public string refreshToken { get; private set; }

        public LoginUtils(string baseURL)
        {
            client = new RestClient(baseURL);
        }

        public async Task<string> Login(User user)
        {
            string tokenStr = null;
            var request = new RestRequest("Login")
                .AddJsonBody(user);
            var response = await client.ExecuteAsync<JWTToken>(request, Method.POST);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                tokenStr = response.Data.Token;
                refreshToken = response.Data.RefreshToken;
            }
            return tokenStr;
        }

        public async Task<string> RefreshToken(string refreshToken)
        {
            string tokenStr = null;
            var request = new RestRequest("Login/{refreshToken}")
                .AddUrlSegment("refreshToken", refreshToken);
            var response = await client.ExecuteAsync<JWTToken>(request, Method.POST);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                tokenStr = response.Data.Token;
                refreshToken = response.Data.RefreshToken;
            }
            return tokenStr;
        }

        public async Task<FirmItemViewModel> GetProduct(string id, string token)
        {
            FirmItemViewModel data = null;
            var request = new RestRequest("api/Products/{id}")
                .AddHeader("Authorization", $"Bearer {token}")
                .AddUrlSegment("id", id);
            var response = await client.ExecuteAsync<FirmItemViewModel>(request, Method.GET);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                data = response.Data;
            }
            return data;
        }

        public async Task<IEnumerable<FirmItemViewModel>> GetAllProduct(int pageNo, int pageSize, string token)
        {
            IEnumerable<FirmItemViewModel> data = null;
            var request = new RestRequest("api/Products")
                .AddHeader("Authorization", $"Bearer {token}")
                .AddQueryParameter("pageNo", pageNo.ToString())
                .AddQueryParameter("pageSize", pageSize.ToString());
            var response = await client.ExecuteAsync<IEnumerable<FirmItemViewModel>>(request, Method.POST);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                data = response.Data;
                string headers = (string)response.Headers.Where(f => f.Name.Equals("X-Pagination")).Select(s => s.Value).FirstOrDefault();
                var pagination = JsonConvert.DeserializeObject<Paging>(headers);
            }
            return data;
        }
    }
}