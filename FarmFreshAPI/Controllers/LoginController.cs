using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using FarmFreshAPI.Models;
using FarmFreshAPI.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace eStoreManagement.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly ILogger<LoginController> _logger;
        private readonly IConfiguration _config;
        private readonly FarmFreshContext _context;

        public LoginController(ILogger<LoginController> logger, IConfiguration configuration, FarmFreshContext context)
        {
            _logger = logger;
            this._config = configuration;
            this._context = context;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] User data1)
        {
            IActionResult response = Unauthorized();
            var user = await AuthenticateUserAsync(data1);
            if (user != null)
            {
                var tokenString = GenerateJWTToken(user);
                var _refreshToken = new RefreshToken
                {
                    UserName = user.UserName,
                    FullName = user.FullName,
                    UserRole = user.UserRole,
                    Refreshtoken = Guid.NewGuid().ToString()
                };
                _context.RefreshTokens.Add(_refreshToken);
                await _context.SaveChangesAsync();

                response = Ok(new JWTToken
                {
                    Token = tokenString,
                    RefreshToken = _refreshToken.Refreshtoken
                });
            }
            return response;
        }

        [HttpPost("{refreshToken}")]
        public async Task<IActionResult> RefreshToken([FromRoute] string refreshToken)
        {
            var _refreshToken = await _context.RefreshTokens.SingleOrDefaultAsync(m => m.Refreshtoken == refreshToken);

            if (_refreshToken == null)
            {
                return NotFound("Refresh token not found");
            }
            var tokenString = GenerateJWTToken(_refreshToken);

            _refreshToken.Refreshtoken = Guid.NewGuid().ToString();
            _context.RefreshTokens.Update(_refreshToken);
            await _context.SaveChangesAsync();

            return Ok(new JWTToken { Token = tokenString, RefreshToken = _refreshToken.Refreshtoken });
        }

        #region Private

        private async Task<UserInfo> AuthenticateUserAsync(User loginCredentials)
        {
            var user = await _context.UserInfos.FirstOrDefaultAsync(x => x.UserName.Equals(loginCredentials.UserName) && x.Password.Equals(loginCredentials.Password));
            return user;
        }

        private string GenerateJWTToken(UserInfo userInfo)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:SecretKey"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userInfo.UserName),
                new Claim("fullName", userInfo.FullName.ToString()),
                new Claim("role",userInfo.UserRole),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: credentials
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string GenerateJWTToken(RefreshToken refreshToken)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:SecretKey"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, refreshToken.UserName),
                new Claim("fullName", refreshToken.FullName.ToString()),
                new Claim("role",refreshToken.UserRole),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: credentials
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        #endregion Private
    }
}