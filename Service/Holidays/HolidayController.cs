using Microsoft.AspNetCore.Mvc;
using Application.Interfaces;
using System.Threading.Tasks;
using System.Collections.Generic;
using LMS.Domain.Holidays;

namespace LMS.Service.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HolidaysController : ControllerBase
    {
        private readonly IHolidayService _holidayService;

        public HolidaysController(IHolidayService holidayService)
        {
            _holidayService = holidayService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var holidays = await _holidayService.GetAllHolidaysAsync();
            return Ok(holidays);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var holiday = await _holidayService.GetHolidayByIdAsync(id);
            return Ok(holiday);
        }

        [HttpPost]
        public async Task<IActionResult> Add(Holiday holiday)
        {
            await _holidayService.AddHolidayAsync(holiday);
            return CreatedAtAction(nameof(GetById), new { id = holiday.Id }, holiday);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Holiday holiday)
        {
            if (id != holiday.Id)
            {
                return BadRequest();
            }

            await _holidayService.UpdateHolidayAsync(holiday);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _holidayService.DeleteHolidayAsync(id);
            return NoContent();
        }
    }
}
