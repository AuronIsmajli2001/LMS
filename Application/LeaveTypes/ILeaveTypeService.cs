using System.Collections.Generic;
using System.Threading.Tasks;
using LMS.Domain.LeaveTypes;

namespace Application.Interfaces
{
    public interface ILeaveTypeService
    {
        Task<IEnumerable<LeaveType>> GetAllLeaveTypesAsync();
        Task<LeaveType> GetLeaveTypeByIdAsync(int id);
        Task AddLeaveTypeAsync(LeaveType leaveType);
        Task UpdateLeaveTypeAsync(LeaveType leaveType);
        Task DeleteLeaveTypeAsync(int id);
    }
}
