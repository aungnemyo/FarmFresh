using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FarmFreshSuperMarket.ViewModels
{
    public class ProductList
    {
        public IEnumerable<FirmItemViewModel> Products { get; set; }
        public Paging Paging { get; set; }
    }
}