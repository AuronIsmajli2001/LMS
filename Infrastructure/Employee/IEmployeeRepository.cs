using System.Collections.Generic;
using LMS.Domain.Employees;

namespace Application.Interfaces
{
    public interface IEmployeeRepository
    {
        IEnumerable<Employee> GetAll();
        Employee GetById(int id);
        void Add(Employee employee);
        void Update(Employee employee);
        void Delete(int id);
        Employee GetLead(int employeeId);
    }
}
