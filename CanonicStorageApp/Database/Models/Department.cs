using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CNNCStorageDB.Models
{
    public class Department
    {
        public Department()
        {
            Positions = new HashSet<Position>();
        }
        public int Id { get; set; }
        [MaxLength(100)]
        public string Name { get; set; }

        public ICollection<Position> Positions { get; set; }
    }
}
