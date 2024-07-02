using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using LMS.Domain.Holidays;

namespace Application.Holidays
{
    public class HolidayService : IHolidayService
    {
        private readonly ILmsRepository<Holiday> _repository;

        public HolidayService(ILmsRepository<Holiday> repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Holiday>> GetAllHolidaysAsync()
        {
            return await _repository.GetAll().ToListAsync();
        }

        public async Task<Holiday> GetHolidayByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task AddHolidayAsync(Holiday holiday)
        {
            _repository.Create(holiday);
            await _repository.SaveChangesAsync();
        }

        public async Task UpdateHolidayAsync(Holiday holiday)
        {
            _repository.Update(holiday);
            await _repository.SaveChangesAsync();
        }

        public async Task DeleteHolidayAsync(int id)
        {
            var holiday = await _repository.GetByIdAsync(id);
            if (holiday != null)
            {
                _repository.Delete(holiday);
                await _repository.SaveChangesAsync();
            }
        }
    }
}
