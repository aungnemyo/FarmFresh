using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using FarmFreshSuperMarket.Models;
using FarmFreshSuperMarket.Utils;
using FarmFreshSuperMarket.ViewModels;
using FarmFreshSuperMarket.Controllers;

namespace FarmFresh.Controllers
{
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> Landing()
        {
            string token = await Login(new User { UserName = "aung", Password = "1234" });
            HttpContext.Session.SetObjectAsJson("token", token);
            //var product = await utils.GetProduct("1", token);
            //var products = await utils.GetAllProduct(1, 10, token);
            return View();
        }

        public IActionResult ProductList(string searchProduct)
        {
            ViewBag.SearchProduct = searchProduct;
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetProductList(string searchProduct, int pageNo, int pageSize)
        {
            string token = HttpContext.Session.GetObjectFromJson<string>("token");
            var products = await GetAllProduct(searchProduct, pageNo, pageSize, token);
            return Ok(products);
        }

        [HttpGet]
        public async Task<IActionResult> GetProductDetail(int id)
        {
            string token = HttpContext.Session.GetObjectFromJson<string>("token");
            var product = await GetProduct(id, token);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}