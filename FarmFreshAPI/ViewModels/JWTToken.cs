﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FarmFreshAPI.ViewModels
{
    public class JWTToken
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
    }
}