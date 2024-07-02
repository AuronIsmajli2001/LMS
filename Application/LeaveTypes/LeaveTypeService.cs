using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using LMS.Domain.LeaveTypes;

namespace Application.LeaveTypes
{
    public class LeaveTypeService : ILeaveTypeService
    {
        private readonly ILmsRepository<LeaveType> _repository;

        public LeaveTypeService(ILmsRepository<LeaveType> repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<LeaveType>> GetAllLeaveTypesAsync()
        {
            return await _repository.GetAll().ToListAsync();
        }

        public async Task<LeaveType> GetLeaveTypeByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task AddLeaveTypeAsync(LeaveType leaveType)
        {
            _repository.Create(leaveType);
            await _repository.SaveChangesAsync();
        }

        public async Task UpdateLeaveTypeAsync(LeaveType leaveType)
        {
            _repository.Update(leaveType);
            await _repository.SaveChangesAsync();
        }

        public async Task DeleteLeaveTypeAsync(int id)
        {
            var leaveType = await _repository.GetByIdAsync(id);
            if (leaveType != null)
            {
                _repository.Delete(leaveType);
                await _repository.SaveChangesAsync();
            }
        }
    }
}
