using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ApiProvaOnline.Models;
using Microsoft.IdentityModel.Tokens;

namespace ApiProvaOnline.Infrastructure.Security
{
    public class TokenService
    {
        private readonly IConfiguration _configuration;

        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateTokenProfessor(Professor professor)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var secret = _configuration["Jwt:Key"] ?? throw new ArgumentNullException("Jwt:Key is missing");
            var key = Encoding.ASCII.GetBytes(secret);
            
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, professor.Nome),
                    new Claim(ClaimTypes.Email, professor.Email),
                    new Claim(ClaimTypes.Role, "professor"),
                    new Claim("Id", professor.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddHours(2),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public string GenerateTokenAdministrativo(Administrativo admin)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var secret = _configuration["Jwt:Key"] ?? throw new ArgumentNullException("Jwt:Key is missing");
            var key = Encoding.ASCII.GetBytes(secret);
            
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, admin.Nome),
                    new Claim(ClaimTypes.Email, admin.Email),
                    new Claim(ClaimTypes.Role, "admin"),
                    new Claim("Id", admin.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddHours(2),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
