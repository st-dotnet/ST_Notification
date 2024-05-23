using Microsoft.EntityFrameworkCore;
using SendNotification.Models;

namespace SendNotification.Data
{
    public class EmployeeLeavesDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = "Server=localhost; Database=STCRM_DB; Integrated Security=true;TrustServerCertificate=True";
            optionsBuilder.UseSqlServer(connectionString);
        }
        public DbSet<Leave> Leaves { get; set; }
        public DbSet<Employee> Employees { get; set; }

    }
}
