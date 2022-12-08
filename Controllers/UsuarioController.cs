using Login.DTOs;
using Login.Models;
using Login.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Login.Controllers
{
    [Route("usuario")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioService _service;
        public UsuarioController(IUsuarioService service) 
            => _service = service;
        
        [HttpPost]
        [AllowAnonymous]
        [Route("registrar")]
        public async Task<ActionResult<Usuario>> RegistrarAsync([FromForm] UsuarioDTO usuario) 
            => await _service.RegistrarAsync(usuario);

        [HttpGet]
        [AllowAnonymous]
        [Route("login")]
        public async Task<ActionResult<string>> LoginAsync([FromForm] EmailESenhaDTO usuario) 
            => await _service.LoginAsync(usuario);

        [HttpDelete]
        [Authorize]
        [Route("deletar")]
        public async Task<ActionResult<Usuario>> DeletarUsuarioAsync([FromForm] string senha) 
            =>  await _service.DeletarUsuarioAsync(User.FindFirstValue(ClaimTypes.Email), senha);

        [HttpPut]
        [Authorize]
        [Route("alterar/email")]
        public async Task<ActionResult<Usuario>> MudarEmailAsync([FromForm] string emailNovo)
            => await _service.MudarEmailAsync(User.FindFirstValue(ClaimTypes.Email), emailNovo);

        [HttpPut]
        [Authorize]
        [Route("alterar/senha")]
        public async Task<ActionResult<Usuario>> MudarSenhaAsync(string email,[FromForm] string senha, [FromForm] string senhaNova)
            => await _service.MudarSenhaAsync(User.FindFirstValue(ClaimTypes.Email), senha, senhaNova);
        
    }
}
