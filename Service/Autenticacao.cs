using Login.Context;
using Login.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Login.Service
{
    public class Autenticacao
    {
        private readonly LoginContext _context;
        private readonly IConfiguration _configuration;
        public Autenticacao(LoginContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public void CriarSenhaComHashESalt(string senha, out byte[] senhaHash, out byte[] senhaSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                senhaSalt = hmac.Key;
                senhaHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(senha));
            }
        }

        public async Task<bool> ChecarSeSenhaECorretaAsync(string senha, string email)
        {
            Usuario usuario = await _context.Usuarios.AsNoTracking().FirstOrDefaultAsync(user => user.Email.Equals(email));
            bool senhaCorreta = VerificarSenhaHash(senha, usuario.SenhaHash, usuario.SenhaSalt);
            return senhaCorreta;
        }

        public bool VerificarSenhaHash(string senha, byte[] senhaHash, byte[] senhaSalt)
        {
            using (var hmac = new HMACSHA512(senhaSalt))
            {
                var hashComputado = hmac.ComputeHash(Encoding.UTF8.GetBytes(senha));
                return hashComputado.Equals(senhaHash);
            }
        }
        public async Task<bool> ChecarQueUsuarioExisteAsync(string email)
        {
            Usuario usuario = null;
            usuario = await _context.Usuarios.AsNoTracking().FirstOrDefaultAsync(user => user.Email == email);
            return usuario != null;
        }

        public async Task<string> CriarTokenAsync(string email)
        {
            Usuario usuario = await _context.Usuarios.AsNoTracking().FirstOrDefaultAsync(user => user.Email.Equals(email));
            var chave = Encoding.ASCII.GetBytes(_configuration.GetSection("AppSettings:Token").Value);
            var gerenciadorToken = new JwtSecurityTokenHandler();
            var descritorToken = new SecurityTokenDescriptor()
            {
                Expires = DateTime.UtcNow.AddHours(8),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(chave), SecurityAlgorithms.HmacSha256Signature),
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Email, email),
                    new Claim(ClaimTypes.Role, usuario.Papel.ToString())
                }),
            };
            var token = gerenciadorToken.CreateToken(descritorToken);
            return gerenciadorToken.WriteToken(token);
        }

    }
}
