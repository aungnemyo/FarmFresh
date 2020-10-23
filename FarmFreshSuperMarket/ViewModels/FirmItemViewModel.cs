using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FarmFreshSuperMarket.ViewModels
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
    }
}