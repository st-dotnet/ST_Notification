using SendNotification.Enum;

namespace SendNotification.Models
{
    public class Leave
    {
        public Guid Id { get; set; }
        public Guid? EmployeeId { get; set; }
        public LeaveTypeEnum LeaveType { get; set; }
        public DateTime? StartDate { get; set; }
        public string? Reason { get; set; }
        public int? Status { get; set; }
        public DateTime? CreatedDate { get; set; }
        public Guid? CreatedBy { get; set; }
        public Guid? LastModifiedBy { get; set; }
        public DateTime? LastModified { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? EndDate { get; set; }
        public Employee Employees { get; set; }

    }
}
