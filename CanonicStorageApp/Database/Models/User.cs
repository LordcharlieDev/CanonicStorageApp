using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CNNCStorageDB.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        [RegularExpression(@"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[a-zA-Z]).{8,}$", ErrorMessage = "The password does not meet the requirements.")]
        public string Password { get; set; }
        public bool IsAdmin { get; set; }

    }
}
