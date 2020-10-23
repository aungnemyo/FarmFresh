using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FarmFreshAPI.Models;
using Microsoft.AspNetCore.Authorization;
using FarmFreshAPI.Contracts;
using FarmFreshAPI.ViewModels;
using Newtonsoft.Json;

namespace FarmFreshAPI.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Policy = Policies.Admin)]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _repository;

        public ProductsController(IProductRepository repository) => _repository = repository;

        // GET: api/Products
        [HttpGet]
        public async Task<IActionResult> GetProducts(string searchProduct, int pageNo = 1, int pageSize = 10)
        {
            var products = await _repository.GetAllAsync(searchProduct, pageNo, pageSize);

            foreach (var product in products)
            {
                if (product.ProductImage != null)
                {
                    product.Image = Convert.ToBase64String(product.ProductImage);
                    product.ProductImage = null;
                }
            }

            var metadata = new Paging
            {
                TotalCount = products.TotalCount,
                PageSize = products.PageSize,
                CurrentPage = products.CurrentPage,
                TotalPages = products.TotalPages,
                HasNext = products.HasNext,
                HasPrevious = products.HasPrevious
            };
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

            return Ok(products);
        }

        // GET: api/Products/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct(int id)
        {
            var product = await _repository.GetByIdAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }
    }
}