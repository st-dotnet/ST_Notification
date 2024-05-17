using System.Collections.Generic;
using System.Data.SqlClient;

public class Program
{
    private static string connectionString = "Server=localhost; Database=STCRM_DB; Integrated Security=true;TrustServerCertificate=True";

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

    private static List<Leave> GetEmployeeLeaves(DateTime date)
    {
        var leaves = new List<Leave>();
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            //string query = "SELECT * FROM Leaves WHERE StartDate = @CurrentDate or EndDate = @CurrentDate ";
            string query = "SELECT * FROM Leaves WHERE StartDate = @CurrentDate ";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@CurrentDate", date);
            connection.Open();
            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    leaves.Add(new Leave
                    {
                        Id = Guid.Parse(reader["Id"].ToString()),
                        EmployeeId = Guid.Parse(reader["EmployeeId"].ToString()),
                        LeaveType = int.Parse(reader["LeaveType"].ToString()),
                        StartDate = DateTime.Parse(reader["StartDate"].ToString()),
                        Reason = reader["Reason"].ToString(),
                        Status = int.Parse(reader["Status"].ToString()),
                        CreatedDate = DateTime.Parse(reader["CreatedDate"].ToString()),
                        CreatedBy = Guid.Parse(reader["CreatedBy"].ToString()),
                        LastModifiedBy = Guid.Parse(reader["LastModifiedBy"].ToString()),
                        LastModified = DateTime.Parse(reader["LastModified"].ToString()),
                        IsActive = bool.Parse(reader["IsActive"].ToString()),
                        EndDate = DateTime.Parse(reader["EndDate"].ToString())
                    });
                }
            }
        }
        return leaves;
    }

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