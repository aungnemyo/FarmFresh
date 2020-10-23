using FarmFreshAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FarmFreshAPI.ViewModels
{
    public class FirmItemViewModel
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public byte[] ProductImage { get; set; }
        public string Image { get; set; }
        public string Category { get; set; }
        public string Package { get; set; }

        public static implicit operator FirmItemViewModel(Product product)
        {
            return new FirmItemViewModel
            {
                ProductId = product.ProductId,
                Name = product.Name,
                Description = product.Description,
                Image = product.ProductImage != null ? Convert.ToBase64String(product.ProductImage) : null,
                ProductImage = null,
                Category = product.Category != null ? product.Category.Name : "",
                Package = product.Package != null ? product.Package.Name : ""
            };
        }
    }
}