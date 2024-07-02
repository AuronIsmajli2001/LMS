using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using LMS.Domain.LeaveAllocations;

namespace Application.LeaveAllocations
{
    public class LeaveAllocationService : ILeaveAllocationService
    {
        private readonly ILmsRepository<LeaveAllocation> _repository;

        public LeaveAllocationService(ILmsRepository<LeaveAllocation> repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<LeaveAllocation>> GetAllLeaveAllocationsAsync()
        {
            return await _repository.GetAll().ToListAsync();
        }

        public async Task<LeaveAllocation> GetLeaveAllocationByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task AddLeaveAllocationAsync(LeaveAllocation leaveAllocation)
        {
            _repository.Create(leaveAllocation);
            await _repository.SaveChangesAsync();
        }

        public async Task UpdateLeaveAllocationAsync(LeaveAllocation leaveAllocation)
        {
            _repository.Update(leaveAllocation);
            await _repository.SaveChangesAsync();
        }

        public async Task DeleteLeaveAllocationAsync(int id)
        {
            var leaveAllocation = await _repository.GetByIdAsync(id);
            if (leaveAllocation != null)
            {
                _repository.Delete(leaveAllocation);
                await _repository.SaveChangesAsync();
            }
        }
    }
}
