using System.ComponentModel.DataAnnotations;

namespace Web.Models.Users
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Preencha o campo e-mail")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Preencha o campo senha")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
