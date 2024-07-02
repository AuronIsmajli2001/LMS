using Application.Interfaces;
using LMS.Domain.LeaveRequests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class LeaveRequestsController : ControllerBase
{
    private readonly ILeaveRequestService _leaveRequestService;

    public LeaveRequestsController(ILeaveRequestService leaveRequestService)
    {
        _leaveRequestService = leaveRequestService;
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetAll()
    {
        var leaveRequests = await _leaveRequestService.GetAllLeaveRequestsAsync();
        return Ok(leaveRequests);
    }

    [HttpGet("{id}")]
    [Authorize]
    public async Task<IActionResult> GetById(int id)
    {
        var leaveRequest = await _leaveRequestService.GetLeaveRequestByIdAsync(id);
        return Ok(leaveRequest);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Add(LeaveRequest leaveRequest)
    {
        await _leaveRequestService.AddLeaveRequestAsync(leaveRequest);
        return CreatedAtAction(nameof(GetById), new { id = leaveRequest.Id }, leaveRequest);
    }

    [HttpPut("{id}")]
    [Authorize]
    public async Task<IActionResult> Update(int id, LeaveRequest leaveRequest)
    {
        if (id != leaveRequest.Id)
        {
            return BadRequest();
        }

        await _leaveRequestService.UpdateLeaveRequestAsync(leaveRequest);
        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize(Policy = "RequireLeadRole")]
    public async Task<IActionResult> Delete(int id)
    {
        await _leaveRequestService.DeleteLeaveRequestAsync(id);
        return NoContent();
    }

    [HttpPost("{id}/approve")]
    [Authorize(Policy = "RequireLeadRole")]
    public async Task<IActionResult> Approve(int id)
    {
        await _leaveRequestService.ApproveLeaveRequestAsync(id);
        return NoContent();
    }

    [HttpPost("{id}/reject")]
    [Authorize(Policy = "RequireLeadRole")]
    public async Task<IActionResult> Reject(int id)
    {
        await _leaveRequestService.RejectLeaveRequestAsync(id);
        return NoContent();
    }
}
