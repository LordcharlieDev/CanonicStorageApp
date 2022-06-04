using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CNNCStorageDB.Models
{
    public class Worker
    {
        public Worker()
        {
            Projects = new HashSet<Project>();
        }
        public int Id { get; set; }
        [DisplayName("First name")]
        public string FirstName { get; set; }
        [DisplayName("Middle name")]
        public string MiddleName { get; set; }
        [DisplayName("Last name")]
        public string LastName { get; set; }
        [RegularExpression(@"[a-z0-9]+(?:\.[a-z0-9]+)*@(?:[a-z0-9](?:[a-z0-9]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9]*[a-z0-9])?", ErrorMessage = "The email does not meet the requirements.")]
        public string Email { get; set; }
        [RegularExpression(@"^\s*(?:\+?(\d{1,3}))?([-. (]*(\d{3})[-. )]*)?((\d{3})[-. ]*(\d{2,4})(?:[-.x ]*(\d+))?)\s*$", ErrorMessage = "The phone does not meet the requirements.")]
        public string Phone { get; set; }
        public DateTime Birthdate { get; set; }
        [Range(1, 100000, ErrorMessage = "Salary must be greater than 0")]
        public int Salary { get; set; }
        [Range(0, 100, ErrorMessage = "Premium must be a positive number")]
        public int Premium { get; set; }
        public string FullInfo => $"{Id} - {FirstName} {MiddleName} {LastName}";

        public Location Location { get; set; }
        public Position Position { get; set; }
        public ICollection<Project> Projects { get; set; }

    }
}
