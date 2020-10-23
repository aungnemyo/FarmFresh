using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FarmFreshAPI.Models
{
    public class RefreshToken
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string UserRole { get; set; }
        public string Refreshtoken { get; set; }
        public bool Revoked { get; set; }
    }
}