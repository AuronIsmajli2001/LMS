using Application.Interfaces;
using LMS.Domain.Employees;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class EmployeesController : ControllerBase
{
    private readonly IEmployeeService _employeeService;

    public EmployeesController(IEmployeeService employeeService)
    {
        _employeeService = employeeService;
    }

    [HttpGet]
    [Authorize(Policy = "RequireLeadRole")]
    public async Task<IActionResult> GetAll()
    {
        var employees = await _employeeService.GetAllEmployeesAsync();
        return Ok(employees);
    }

    [HttpGet("{id}")]
    [Authorize]
    public async Task<IActionResult> GetById(int id)
    {
        var employee = await _employeeService.GetEmployeeByIdAsync(id);
        return Ok(employee);
    }

    [HttpPost]
    [Authorize(Policy = "RequireLeadRole")]
    public async Task<IActionResult> Add(Employee employee)
    {
        await _employeeService.AddEmployeeAsync(employee);
        return CreatedAtAction(nameof(GetById), new { id = employee.Id }, employee);
    }

    [HttpPut("{id}")]
    [Authorize(Policy = "RequireLeadRole")]
    public async Task<IActionResult> Update(int id, Employee employee)
    {
        if (id != employee.Id)
        {
            return BadRequest();
        }

        await _employeeService.UpdateEmployeeAsync(employee);
        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize(Policy = "RequireLeadRole")]
    public async Task<IActionResult> Delete(int id)
    {
        await _employeeService.DeleteEmployeeAsync(id);
        return NoContent();
    }

    [HttpGet("{id}/lead")]
    [Authorize]
    public async Task<IActionResult> GetLead(int id)
    {
        var lead = await _employeeService.GetLeadAsync(id);
        return Ok(lead);
    }
}
