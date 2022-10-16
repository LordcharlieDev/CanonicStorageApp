using CNNCStorageDB.Configurations;
using CNNCStorageDB.Models;
using Microsoft.EntityFrameworkCore;

namespace CNNCStorageDB.Data
{
    public class CNNCDbContext : DbContext
    {
        public CNNCDbContext() 
        {
        }
        public CNNCDbContext(DbContextOptions<CNNCDbContext> options) : base(options)
        {
        }
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    base.OnConfiguring(optionsBuilder);
        //    optionsBuilder.UseSqlServer(@"workstation id=CNNCStorageDb.mssql.somee.com;packet size=4096;user id=lordcharlie_SQLLogin_1;pwd=qhgjyc15ca;data source=CNNCStorageDb.mssql.somee.com;persist security info=False;initial catalog=CNNCStorageDb");
        //    optionsBuilder.UseSqlServer(@"Data Source=DESKTOP-SEGB2UP;Initial catalog = CNNCDb;Integrated Security=True;Connect Timeout=5;");
        //    optionsBuilder.UseSqlServer(@"Data Source=DESKTOP-5D1R5GU;Initial catalog = CNNCDb;Integrated Security=True;Connect Timeout=5;");
        //}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //Configuration
            modelBuilder.ApplyConfiguration(new DepartmentConfig());
            modelBuilder.ApplyConfiguration(new PositionConfig());
            modelBuilder.ApplyConfiguration(new WorkerConfig());
            modelBuilder.ApplyConfiguration(new ProjectConfig());
            modelBuilder.ApplyConfiguration(new ClientConfig());
            modelBuilder.ApplyConfiguration(new UserConfig());
            modelBuilder.ApplyConfiguration(new LocationConfig());

            //Seeds
            modelBuilder.SeedDepartments();
            modelBuilder.SeedPositions();
            modelBuilder.SeedWorkers();
            modelBuilder.SeedProjects();
            modelBuilder.SeedClients();
            modelBuilder.SeedUsers();
            modelBuilder.SeedLocations();
        }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<Worker> Workers { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Location> Locations { get; set; }
    }
}
