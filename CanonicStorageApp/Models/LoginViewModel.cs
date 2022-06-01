using System.ComponentModel.DataAnnotations;

namespace CanonicStorageApp.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Enter username")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Enter password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
