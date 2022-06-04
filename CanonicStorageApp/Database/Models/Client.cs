using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CNNCStorageDB.Models
{
    public class Client
    {
        public Client()
        {
            Projects = new HashSet<Project>();
        }
        public int Id { get; set; }
        [DisplayName("Full name")]
        public string FullName { get; set; }
        public string Address { get; set; }

        [RegularExpression(@"[a-z0-9]+(?:\.[a-z0-9]+)*@(?:[a-z0-9](?:[a-z0-9]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9]*[a-z0-9])?")]
        public string Email { get; set; }

        [RegularExpression(@"^\s*(?:\+?(\d{1,3}))?([-. (]*(\d{3})[-. )]*)?((\d{3})[-. ]*(\d{2,4})(?:[-.x ]*(\d+))?)\s*$")]
        public string Phone { get; set; }

        public ICollection<Project> Projects { get; set; }
    }
}
