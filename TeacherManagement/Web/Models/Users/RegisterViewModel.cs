using System.ComponentModel.DataAnnotations;

namespace Web.Models.Users
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Email é uma informação obrigatória")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Nome é uma informação obrigatória")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Senha é uma informação obrigatória")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
