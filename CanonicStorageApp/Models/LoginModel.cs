using System.ComponentModel.DataAnnotations;

namespace CanonicStorageApp.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Не введено Username")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Не введено пароль")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
