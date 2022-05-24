using System.ComponentModel.DataAnnotations;

namespace CanonicStorageApp.Models
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "Не введено Login")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Не введено пароль")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Пароль введено неправильно")]
        public string ConfirmPassword { get; set; }
    }
}
