using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SendNotification.Data;
using SendNotification.Enum;
using SendNotification.Models;
using System.Text;

public class Program
{
    private EmployeeLeavesDbContext _employeeLeavesDbContext;
    private static readonly HttpClient client = new HttpClient();
    public Program()
    {
        // Initialize your DbContext here with options
        var optionsBuilder = new DbContextOptionsBuilder<EmployeeLeavesDbContext>();
        //optionsBuilder.UseSqlServer();
        _employeeLeavesDbContext = new EmployeeLeavesDbContext();
    }

    public static async Task Main(string[] args)
    {
        try
        {
            // Get current date
            DateTime currentDate = DateTime.Now.Date;
            var empData = new List<Leave>();

            // Check if it's a weekday
            if (currentDate.DayOfWeek != DayOfWeek.Saturday && currentDate.DayOfWeek != DayOfWeek.Sunday)
            {
                // Create an instance of the program
                Program program = new Program();

                // Call the instance method
                var employeeLeaves = program.GetEmployeeLeaves(currentDate);

                var casualLeaves = employeeLeaves.Where(l => l.LeaveType == LeaveTypeEnum.CasualLeave).Select(l => l.Employees.FirstName).ToList();
                var workFromHomeLeaves = employeeLeaves.Where(l => l.LeaveType == LeaveTypeEnum.Workfromhome).Select(l => l.Employees.FirstName).ToList();
                var shortLeaves = employeeLeaves.Where(l => l.LeaveType == LeaveTypeEnum.ShortLeave).Select(l => l.Employees.FirstName).ToList();
                var sickLeaves = employeeLeaves.Where(l => l.LeaveType == LeaveTypeEnum.SickLeave).Select(l => l.Employees.FirstName).ToList();

                var webhookUrl = "https://supremetechnologiesindia.webhook.office.com/webhookb2/7f01b6a0-d526-4958-a79f-16c8768fbe32@04b31863-f802-42b0-a7a9-be8dde194ef5/IncomingWebhook/432106c64907488e80d7d1756e6e5774/64395f39-bac9-45d8-af25-75e269c6658f";

                var casualLeavesText = string.Join(", ", casualLeaves);
                var workFromHomeLeavesText = string.Join(", ", workFromHomeLeaves);
                var shortLeavesText = string.Join(", ", shortLeaves);
                var sickLeavesText = string.Join(", ", sickLeaves);

                var message = new
                {
                    text = $"<b>Attendance - </b>{currentDate}\n" +
                           $"<b>Absent for today (Casual Leave): </b>{casualLeavesText}\n" +
                           $"<b>Work from home: </b>{workFromHomeLeavesText}\n" +
                          $"<b>Short Leave: </b>{shortLeavesText}\n" +
                         $"<b>Sick Leave: </b>{sickLeavesText}"
                };

                var jsonMessage = JsonConvert.SerializeObject(message);

                var data = new StringContent(jsonMessage, Encoding.UTF8, "application/json");

                var response = await client.PostAsync(webhookUrl, data);

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Notification sent successfully.");
                }
                else
                {
                    Console.WriteLine("Failed to send notification.");
                    Console.WriteLine("Status Code: " + response.StatusCode);
                    var responseContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine("Response: " + responseContent);
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public List<Leave> GetEmployeeLeaves(DateTime date)
    {
        try
        {
            var leaves = _employeeLeavesDbContext.Leaves
           .Include(leave => leave.Employees)
            .Where(leave => leave.StartDate == date) // Filter leaves by the provided date
            .AsNoTracking()
            .ToList();

            return leaves;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
}


