using CNNCStorageDB.Validations;
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
        [MaxLength(13)]
        public string Phone { get; set; }
        [DateValidation(ErrorMessage = "The date must be correct!")]
        public DateTime Birthdate { get; set; }
        [MaxLength(300)]
        public string Address { get; set; }
        public bool Army { get; set; }
        [MaxLength(20)]
        public string Passport { get; set; }
        public string Sex { get; set; }
        [DisplayName("Marital status")]
        public string MaritalStatus { get; set; }
        [Range(0, 100, ErrorMessage = "Digit must be more than or equal 0")]
        [DisplayName("Number of childrens")]
        public int Childrens { get; set; }
        [Range(1, 100000, ErrorMessage = "Salary must be more than 0 and less than 100000")]
        public int Salary { get; set; }
        [Range(0, 100, ErrorMessage = "Premium must be more than 0 and less than 100")]
        public int Premium { get; set; }
        [DateValidation(ErrorMessage = "The date must be correct!")]
        [DisplayName("Date of emploment")]
        public DateTime DateOfEmployment { get; set; }
        public string FullInfo => $"{Id} - {FirstName} {MiddleName} {LastName}";
        public int Experience => DateOfEmployment.Year == DateTime.Now.Year ? 0 : DateTime.Now.AddYears(-DateOfEmployment.Year).Year;


        public Location Location { get; set; }
        public Position Position { get; set; }
        public ICollection<Project> Projects { get; set; }

    }
}
