﻿using System.Collections.Generic;
using System.Threading.Tasks;
using LMS.Domain.Employees;

namespace Application.Interfaces
{
    public interface IEmployeeService
    {
        Task<IEnumerable<Employee>> GetAllEmployeesAsync();
        Task<Employee> GetEmployeeByIdAsync(int id);
        Task AddEmployeeAsync(Employee employee);
        Task UpdateEmployeeAsync(Employee employee);
        Task DeleteEmployeeAsync(int id);
        Task<Employee> GetLeadAsync(int employeeId);
    }
}
