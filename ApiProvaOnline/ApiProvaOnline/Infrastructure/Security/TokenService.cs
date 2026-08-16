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

        private SymmetricSecurityKey GetKey()
        {
            var secret = _configuration["Jwt:Key"]
                ?? throw new InvalidOperationException("Jwt:Key não configurada. Use variáveis de ambiente ou User Secrets.");
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secret));
        }

        public string GenerateTokenProfessor(Professor professor)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
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
                SigningCredentials = new SigningCredentials(GetKey(), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public string GenerateTokenAdministrativo(Administrativo admin)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
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
                SigningCredentials = new SigningCredentials(GetKey(), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        /// <summary>
        /// Gera um JWT de sessão para o aluno, vinculado ao alunoId e provaId.
        /// Expira em 4 horas — suficiente para concluir qualquer prova.
        /// </summary>
        public string GenerateTokenAluno(int alunoId, int provaId, string nomeAluno)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, nomeAluno),
                    new Claim(ClaimTypes.Role, "aluno"),
                    new Claim("AlunoId", alunoId.ToString()),
                    new Claim("ProvaId", provaId.ToString())
                }),
                Expires = DateTime.UtcNow.AddHours(4),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"],
                SigningCredentials = new SigningCredentials(GetKey(), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
