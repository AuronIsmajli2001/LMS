using System.Collections.Generic;
using System.Threading.Tasks;
using LMS.Domain.LeaveRequests;

namespace Application.Interfaces
{
    public interface ILeaveRequestService
    {
        Task<IEnumerable<LeaveRequest>> GetAllLeaveRequestsAsync();
        Task<LeaveRequest> GetLeaveRequestByIdAsync(int id);
        Task AddLeaveRequestAsync(LeaveRequest leaveRequest);
        Task UpdateLeaveRequestAsync(LeaveRequest leaveRequest);
        Task DeleteLeaveRequestAsync(int id);
        Task ApproveLeaveRequestAsync(int id);
        Task RejectLeaveRequestAsync(int id);
    }
}
