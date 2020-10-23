using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using FarmFreshSuperMarket.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestSharp;

namespace FarmFreshSuperMarket.Controllers
{
    public class BaseController : Controller
    {
        private readonly string baseURL = "https://localhost:44380/";
        private readonly RestClient client;
        public string refreshToken { get; private set; }

        public BaseController()
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

        public async Task<FirmItemViewModel> GetProduct(int id, string token)
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

        public async Task<ProductList> GetAllProduct(string searchProduct, int pageNo, int pageSize, string token)
        {
            ProductList data = null;
            var request = new RestRequest("api/Products")
                .AddHeader("Authorization", $"Bearer {token}")
                .AddQueryParameter("searchProduct", searchProduct)
                .AddQueryParameter("pageNo", pageNo.ToString())
                .AddQueryParameter("pageSize", pageSize.ToString());
            var response = await client.ExecuteAsync<IEnumerable<FirmItemViewModel>>(request, Method.GET);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                data = new ProductList();
                data.Products = response.Data;
                string headers = (string)response.Headers.Where(f => f.Name.Equals("X-Pagination")).Select(s => s.Value).FirstOrDefault();
                data.Paging = JsonConvert.DeserializeObject<Paging>(headers);
            }
            return data;
        }
    }
}