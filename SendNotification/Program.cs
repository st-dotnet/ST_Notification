using Microsoft.EntityFrameworkCore;

public class Program
{
    public static async Task Main(string[] args)
    {
        // Get current date
        DateTime currentDate = DateTime.Now.Date;

        // Check if it's a weekday
        if (currentDate.DayOfWeek != DayOfWeek.Saturday && currentDate.DayOfWeek != DayOfWeek.Sunday)
        {
            var employeeLeaves = GetEmployeeLeaves(currentDate);
            // Print each leave entry
            foreach (var leave in employeeLeaves)
            {
                Console.WriteLine($"Employee ID: {leave.EmployeeId}");
                Console.WriteLine($"Leave Type: {leave.LeaveType}");
                Console.WriteLine($"Start Date: {leave.StartDate}");
                Console.WriteLine($"End Date: {leave.EndDate}");
                Console.WriteLine();
            }
        }
    }

    static List<Leave> GetEmployeeLeaves(DateTime date)
    {
        // Add code to fetch leaves from database using Entity Framework
        using (var dbContext = new EmployeeLeavesDbContext())
        {
            return dbContext.Leaves
                             .Where(x => x.StartDate == date)
                             .ToList();
        }
    }
   
    public class EmployeeLeavesDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connectionString = "Server=localhost; Database=STCRM_DB; Integrated Security=true;TrustServerCertificate=True";
            optionsBuilder.UseSqlServer(connectionString);
        }
        public DbSet<Leave> Leaves { get; set; }
    }

    public class Leave
    {
        public Guid Id { get; set; }
        public Guid? EmployeeId { get; set; }
        public int? LeaveType { get; set; }
        public DateTime? StartDate { get; set; }
        public string? Reason { get; set; }
        public int? Status { get; set; }
        public DateTime? CreatedDate { get; set; }
        public Guid? CreatedBy { get; set; }
        public Guid? LastModifiedBy { get; set; }
        public DateTime? LastModified { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? EndDate { get; set; }
    }
}