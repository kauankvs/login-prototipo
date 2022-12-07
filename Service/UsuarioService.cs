using Login.Context;
using Login.Controllers;
using Login.DTOs;
using Login.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Login.Service
{
    public class UsuarioService
    {
        private readonly LoginContext _context;
        private readonly Autenticacao _auth;
        public UsuarioService(LoginContext context, Autenticacao senha) 
        {
            _context = context;
            _auth = senha;
        }

        public async Task<ActionResult<Usuario>> RegistrarAsync(UsuarioDTO usuarioDTO) 
        {
            byte[] senhaHash;
            byte[] senhaSalt;
            bool usuarioExiste = await _auth.ChecarQueUsuarioExisteAsync(usuarioDTO.Email);
            if (usuarioExiste.Equals(true))
            {
                return new ConflictResult();
            }
            _auth.CriarSenhaComHashESalt(usuarioDTO.Senha, out senhaHash, out senhaSalt);
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
            return new CreatedResult(nameof(UsuarioController), usuario);
        }
        
        public async Task<ActionResult<string>> LoginAsync(EmailESenhaDTO usuario) 
        {
            bool usuarioExiste = await _auth.ChecarQueUsuarioExisteAsync(usuario.Email);
            if (usuarioExiste.Equals(false))
            {
                return new NotFoundObjectResult(usuario);
            }
            bool senheECorreta = await _auth.ChecarSeSenhaECorretaAsync(usuario.Senha, usuario.Email);
            if (senheECorreta.Equals(false)) 
            {
                return new ForbidResult();
            }
            var token = await _auth.CriarTokenAsync(usuario.Email);
            return new OkObjectResult(token);
        }



    }
}
