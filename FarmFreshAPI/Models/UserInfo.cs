using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FarmFreshAPI.Models
{
    public class UserInfo
    {
        public int UserInfoId { get; set; }
        public string PhoneNo { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Password { get; set; }
        public string UserRole { get; set; }
    }
}