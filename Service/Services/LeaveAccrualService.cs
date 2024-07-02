using System;
using System.Threading.Tasks;
using Application.Interfaces;
using LMS.Domain.LeaveAllocations;

namespace LMS.Service.Services
{
    public class LeaveAccrualService
    {
        private readonly ILeaveAllocationService _leaveAllocationService;
        private readonly IEmployeeService _employeeService;
        private readonly ILeaveTypeService _leaveTypeService;

        public LeaveAccrualService(ILeaveAllocationService leaveAllocationService, IEmployeeService employeeService, ILeaveTypeService leaveTypeService)
        {
            _leaveAllocationService = leaveAllocationService;
            _employeeService = employeeService;
            _leaveTypeService = leaveTypeService;
        }

        public async Task AccrueLeave()
        {
            var employees = await _employeeService.GetAllEmployeesAsync();
            var annualLeaveType = (await _leaveTypeService.GetAllLeaveTypesAsync()).FirstOrDefault(x => x.Name == "Annual");

            foreach (var employee in employees)
            {
                var leaveAllocation = new LeaveAllocation
                {
                    EmployeeId = employee.Id,
                    LeaveTypeId = annualLeaveType.Id,
                    NumberOfDays = 2,
                    DateCreated = DateTime.UtcNow
                };
                await _leaveAllocationService.AddLeaveAllocationAsync(leaveAllocation);
            }
        }

        public async Task ResetAnnualLeave()
        {
            var employees = await _employeeService.GetAllEmployeesAsync();
            var annualLeaveType = (await _leaveTypeService.GetAllLeaveTypesAsync()).FirstOrDefault(x => x.Name == "Annual");

            foreach (var employee in employees)
            {
                var leaveAllocations = (await _leaveAllocationService.GetAllLeaveAllocationsAsync()).Where(x => x.EmployeeId == employee.Id && x.LeaveTypeId == annualLeaveType.Id).ToList();
                foreach (var allocation in leaveAllocations)
                {
                    await _leaveAllocationService.DeleteLeaveAllocationAsync(allocation.Id);
                }
            }
        }
    }
}
