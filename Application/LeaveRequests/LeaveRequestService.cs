using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using LMS.Domain.LeaveRequests;

namespace Application.LeaveRequests
{
    public class LeaveRequestService : ILeaveRequestService
    {
        private readonly ILmsRepository<LeaveRequest> _repository;

        public LeaveRequestService(ILmsRepository<LeaveRequest> repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<LeaveRequest>> GetAllLeaveRequestsAsync()
        {
            return await _repository.GetAll().ToListAsync();
        }

        public async Task<LeaveRequest> GetLeaveRequestByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task AddLeaveRequestAsync(LeaveRequest leaveRequest)
        {
            _repository.Create(leaveRequest);
            await _repository.SaveChangesAsync();
        }

        public async Task UpdateLeaveRequestAsync(LeaveRequest leaveRequest)
        {
            _repository.Update(leaveRequest);
            await _repository.SaveChangesAsync();
        }

        public async Task DeleteLeaveRequestAsync(int id)
        {
            var leaveRequest = await _repository.GetByIdAsync(id);
            if (leaveRequest != null)
            {
                _repository.Delete(leaveRequest);
                await _repository.SaveChangesAsync();
            }
        }

        public async Task ApproveLeaveRequestAsync(int id)
        {
            var leaveRequest = await _repository.GetByIdAsync(id);
            if (leaveRequest != null)
            {
                leaveRequest.Status = "Approved";
                _repository.Update(leaveRequest);
                await _repository.SaveChangesAsync();
            }
        }

        public async Task RejectLeaveRequestAsync(int id)
        {
            var leaveRequest = await _repository.GetByIdAsync(id);
            if (leaveRequest != null)
            {
                leaveRequest.Status = "Rejected";
                _repository.Update(leaveRequest);
                await _repository.SaveChangesAsync();
            }
        }
    }
}
