using Login.Models;
using System.ComponentModel.DataAnnotations;

namespace Login.DTOs
{
    public class UsuarioDTO
    {
        [Required(ErrorMessage = "Campo obrigatório!")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "Campo obrigatório!")]
        public string Sobrenome { get; set; }

        [Required(ErrorMessage = "Campo obrigatório!")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Esse campo deve conter um email!")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Campo obrigatório!")]
        [DataType(DataType.Password)]
        public string Senha { get; set; }
    }
}
