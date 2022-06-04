using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CNNCStorageDB.Models
{
    public class Project
    {
        public Project()
        {
            Workers = new HashSet<Worker>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public int Budget { get; set; }
        [DisplayName("Start date")]
        public DateTime StartDate { get; set; }
        [DisplayName("End date")]
        public DateTime EndDate { get; set; }
        [DisplayName("Final cost")]
        public int FinalCost { get; set; }

        public Client Client { get; set; }
        public ICollection<Worker> Workers { get; set; }
    }
}
