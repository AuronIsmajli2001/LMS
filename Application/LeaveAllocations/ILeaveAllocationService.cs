using System.Collections.Generic;
using System.Threading.Tasks;
using LMS.Domain.LeaveAllocations;

namespace Application.Interfaces
{
    public interface ILeaveAllocationService
    {
        Task<IEnumerable<LeaveAllocation>> GetAllLeaveAllocationsAsync();
        Task<LeaveAllocation> GetLeaveAllocationByIdAsync(int id);
        Task AddLeaveAllocationAsync(LeaveAllocation leaveAllocation);
        Task UpdateLeaveAllocationAsync(LeaveAllocation leaveAllocation);
        Task DeleteLeaveAllocationAsync(int id);
    }
}
