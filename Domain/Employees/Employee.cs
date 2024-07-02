using LMS.Domain.LeaveAllocations;
using LMS.Domain.LeaveRequests;

namespace LMS.Domain.Employees
{
    public class Employee
    {
        public int Id { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public string LeadEmail { get; set; }
        public string Position { get; set; }
        public string ReportsTo { get; set; } // the id of user which this user reports to
        public DateTime DateOfBirth { get; set; }
        public DateTime DateJoined { get; set; }

        public ICollection<LeaveRequest> LeaveRequests { get; set; }
        public ICollection<LeaveAllocation> LeaveAllocations { get; set; }
        public ICollection<LeaveRequest> ApprovedLeaveRequests { get; set; }
    }
}
