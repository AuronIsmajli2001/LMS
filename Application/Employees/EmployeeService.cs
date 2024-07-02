using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LMS.Domain.Employees;
using Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using LMS.Domain.Employees;

namespace Application.Employees
{
    public class EmployeeService : IEmployeeService
    {
        private readonly ILmsRepository<Employee> _repository;

        public EmployeeService(ILmsRepository<Employee> repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Employee>> GetAllEmployeesAsync()
        {
            return await _repository.GetAll().ToListAsync();
        }

        public async Task<Employee> GetEmployeeByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task AddEmployeeAsync(Employee employee)
        {
            _repository.Create(employee);
            await _repository.SaveChangesAsync();
        }

        public async Task UpdateEmployeeAsync(Employee employee)
        {
            _repository.Update(employee);
            await _repository.SaveChangesAsync();
        }

        public async Task DeleteEmployeeAsync(int id)
        {
            var employee = await _repository.GetByIdAsync(id);
            if (employee != null)
            {
                _repository.Delete(employee);
                await _repository.SaveChangesAsync();
            }
        }

        public async Task<Employee> GetLeadAsync(int employeeId)
        {
            var employee = await _repository.GetByIdAsync(employeeId);
            return employee != null ? await _repository.GetByCondition(e => e.Email == employee.LeadEmail).FirstOrDefaultAsync() : null;
        }
    }
}
