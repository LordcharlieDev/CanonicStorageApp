using System.ComponentModel.DataAnnotations;

namespace CanonicStorageApp.Models
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Не введено Username")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Не введено пароль")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Пароль введено неправильно")]
        public string ConfirmPassword { get; set; }
    }
}
