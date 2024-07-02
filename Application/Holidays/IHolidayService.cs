using System.Collections.Generic;
using System.Threading.Tasks;
using LMS.Domain.Holidays;

namespace Application.Interfaces
{
    public interface IHolidayService
    {
        Task<IEnumerable<Holiday>> GetAllHolidaysAsync();
        Task<Holiday> GetHolidayByIdAsync(int id);
        Task AddHolidayAsync(Holiday holiday);
        Task UpdateHolidayAsync(Holiday holiday);
        Task DeleteHolidayAsync(int id);
    }
}
