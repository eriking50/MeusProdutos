using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using MyProducts.Entities;
using Microsoft.Extensions.Configuration;

namespace MyProducts.Services
{
    /// <summary>
    /// Classe que cuida de geração de tokens
    /// </summary>
    public class TokenService
    {
        private IConfiguration configuration;
        private string jwtkey;
        public TokenService(IConfiguration iConfig)
        {
            configuration = iConfig;
            jwtkey = configuration.GetSection("JWTKey").Value;
        }
        public string GenerateToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(jwtkey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Name.ToString())
                }),
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}