using System.ComponentModel.DataAnnotations;

namespace CanonicStorageApp.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Не введено Login")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Не введено пароль")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
