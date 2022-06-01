using System.ComponentModel.DataAnnotations;

namespace CanonicStorageApp.Models
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Enter username")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Enter password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; }
        public bool IsAdmin { get; set; }
    }
}
