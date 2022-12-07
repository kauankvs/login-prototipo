using Login.Context;
using Login.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace Login.Service
{
    public class Senha
    {
        private readonly LoginContext _context;
        public Senha(LoginContext context)
        {
            _context = context;
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
    }
}
