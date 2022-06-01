using CNNCStorageDB.Models;

namespace CanonicStorageApp.Models
{
    public class ProjectViewModel
    {
        public Project Project { get; set; }

        public int[] SelectedWorkers { get; set; }

        public List<Worker> Workers { get; set; }

        public ProjectViewModel()
        {

        }
        public ProjectViewModel(Project project, List<Worker> workers)
        {
            Project = project;
            SelectedWorkers = new int[workers.Count];
            Workers = workers;
        }
    }
}
