using Login.Context;
using Login.DTOs;
using Login.Models;
using Microsoft.AspNetCore.Mvc;

namespace Login.Service
{
    public class UsuarioService
    {
        private readonly LoginContext _context;
        private readonly Senha _senha;
        public UsuarioService(LoginContext context, Senha senha) 
        {
            _context = context;
            _senha = senha;
        }

        public async Task<ActionResult<Usuario>> Registrar(UsuarioDTO usuarioDTO) 
        {
            byte[] senhaHash;
            byte[] senhaSalt;
            bool usuarioExiste = await _senha.ChecarQueUsuarioExisteAsync(usuarioDTO.Email);
            if (usuarioExiste.Equals(true))
            {
                return new ConflictResult();
            }

            _senha.CriarSenhaComHashESalt(usuarioDTO.Senha, out senhaHash, out senhaSalt);
            Usuario usuario = new Usuario()
            {
                Nome = usuarioDTO.Nome,
                Sobrenome = usuarioDTO.Sobrenome,
                Email = usuarioDTO.Email,
                SenhaHash = senhaHash,
                SenhaSalt = senhaSalt,
                DataDeCriacao = DateTime.Now,
                Papel = Papel.Cliente
            };
            await _context.Usuarios.AddAsync(usuario);
            await _context.SaveChangesAsync();
            return new CreatedResult("local", usuario);
        }

    }
}
