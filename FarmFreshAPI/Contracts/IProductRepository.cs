using FarmFreshAPI.Models;
using FarmFreshAPI.Utils;
using FarmFreshAPI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FarmFreshAPI.Contracts
{
    public interface IProductRepository
    {
        Task<PagedList<FirmItemViewModel>> GetAllAsync(string searchProduct, int pageNo, int pageSize);

        Task<FirmItemViewModel> GetByIdAsync(int id);
    }
}