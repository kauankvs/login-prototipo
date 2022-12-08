using Login.DTOs;
using Login.Models;
using Microsoft.AspNetCore.Mvc;

namespace Login.Service
{
    public interface IUsuarioService
    {
        public Task<ActionResult<Usuario>> RegistrarAsync(UsuarioDTO usuarioDTO);
        public Task<ActionResult<string>> LoginAsync(EmailESenhaDTO usuario);
        public Task<ActionResult<Usuario>> DeletarUsuarioAsync(string email, string senha);
        public Task<ActionResult<Usuario>> MudarEmailAsync(string email, string emailNovo);
        public Task<ActionResult<Usuario>> MudarSenhaAsync(string email, string senha, string senhaNova);
    }
}
