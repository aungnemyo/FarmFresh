using FarmFreshAPI.Contracts;
using FarmFreshAPI.Models;
using FarmFreshAPI.Utils;
using FarmFreshAPI.ViewModels;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FarmFreshAPI.Services
{
    public class ProductRepository : IProductRepository
    {
        private readonly FarmFreshContext _context;

        public ProductRepository(FarmFreshContext context) => _context = context;

        public async Task<PagedList<FirmItemViewModel>> GetAllAsync(string searchProduct, int pageNo, int pageSize)
        {
            IQueryable<FirmItemViewModel> query = null;
            if (string.IsNullOrEmpty(searchProduct))
            {
                query = _context.Products
                    .Select(s => new FirmItemViewModel
                    {
                        ProductId = s.ProductId,
                        Name = s.Name,
                        Description = s.Description,
                        ProductImage = s.ProductImage
                    })
                    .AsNoTracking()
                    .OrderBy(o => o.Name);
            }
            else
            {
                query = _context.Products
                    .Select(s => new FirmItemViewModel
                    {
                        ProductId = s.ProductId,
                        Name = s.Name,
                        Description = s.Description,
                        ProductImage = s.ProductImage
                    })
                    .Where(w => w.Name.ToUpper().Contains(searchProduct.ToUpper()))
                    .AsNoTracking()
                    .OrderBy(o => o.Name);
            }
            return await PagedList<FirmItemViewModel>.ToPagedListAsync(query, pageNo, pageSize);
        }

        public async Task<FirmItemViewModel> GetByIdAsync(int id)
        {
            return await _context.Products
                .Include(p => p.Package)
                .Include(c => c.Category)
                .AsNoTracking()
                .FirstOrDefaultAsync(f => f.ProductId == id);
        }
    }
}