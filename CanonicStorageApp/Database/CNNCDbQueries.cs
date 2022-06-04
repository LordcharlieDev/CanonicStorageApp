using CNNCStorageDB.Data;
using CNNCStorageDB.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CNNCStorageDB
{
    public class CNNCDbQueries
    {
        private readonly CNNCDbContext db;

        public CNNCDbQueries(CNNCDbContext context)
        {
            db = context;
        }
        public async Task<IEnumerable<Department>> GetAllDepartments()
        {
            return await db.Departments.ToListAsync();
        }
        public async Task<IEnumerable<Position>> GetAllPositions()
        {
            return db.Positions.Include(p => p.Department).ToList();
        }
        public async Task<IEnumerable<Worker>> GetAllWorkers()
        {
            return await db.Workers.Include(w => w.Position)
                             .Include(w => w.Location)
                             .ToListAsync();
        }
        public async Task<IEnumerable<Project>> GetAllProjects()
        {
            return db.Projects.ToList();
        }
        public async Task<IEnumerable<Client>> GetAllClients()
        {
            return db.Clients.ToList();
        }
        public async Task<IEnumerable<Location>> GetAllLocations()
        {
            return db.Locations.ToList();
        }        
        public async Task<IEnumerable<User>> GetAllUsers()
        {
            return db.Users.ToList();
        }

        public async Task<bool> CheckUser(string username, string password)
        {
            var result = db.Users.Where(u => u.Username == username && u.Password == password).ToList().Count;
            if (result == 1)
            {
                return true;
            }
            return false;
        }
    }
}
