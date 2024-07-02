using Application.Interfaces;
using LMS.Domain.LeaveAllocations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.InteropServices;

[ApiController]
[Route("api/[controller]")]
public class LeaveAllocationsController : ControllerBase
{
    private readonly ILeaveAllocationService _leaveAllocationService;

    public LeaveAllocationsController(ILeaveAllocationService leaveAllocationService)
    {
        _leaveAllocationService = leaveAllocationService;
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetAll()
    {
        var leaveAllocations = await _leaveAllocationService.GetAllLeaveAllocationsAsync();
        return Ok(leaveAllocations);
    }

    [HttpGet("{id}")]
    [Authorize]
    public async Task<IActionResult> GetById(int id)
    {
        var leaveAllocation = await _leaveAllocationService.GetLeaveAllocationByIdAsync(id);
        return Ok(leaveAllocation);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Add(LeaveAllocation leaveAllocation)
    {
        await _leaveAllocationService.AddLeaveAllocationAsync(leaveAllocation);
        return CreatedAtAction(nameof(GetById), new { id = leaveAllocation.Id }, leaveAllocation);
    }

    [HttpPut("{id}")]
    [Authorize]
    public async Task<IActionResult> Update(int id, LeaveAllocation leaveAllocation)
    {
        if (id != leaveAllocation.Id)
        {
            return BadRequest();
        }

        await _leaveAllocationService.UpdateLeaveAllocationAsync(leaveAllocation);
        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize(Policy = "RequireLeadRole")]
    public async Task<IActionResult> Delete(int id)
    {
        await _leaveAllocationService.DeleteLeaveAllocationAsync(id);
        return NoContent();
    }
}
