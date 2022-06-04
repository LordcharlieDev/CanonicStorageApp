using System;
using System.Collections.Generic;
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
        public string Name { get; set; }

        public ICollection<Position> Positions { get; set; }
    }
}
