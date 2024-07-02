using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface ILmsRepository<Tentity> where Tentity : class
    {
        void Create(Tentity entity);
        void CreateRange(List<Tentity> entities);
        void Delete(Tentity entity);
        void DeleteRange(List<Tentity> entities);
        IQueryable<Tentity> GetAll();
        IQueryable<Tentity> GetByCondition(Expression<Func<Tentity, bool>> expression);
        Task<Tentity> GetByIdAsync(int id); // Use async for fetching by id
        Task SaveChangesAsync();
        void Update(Tentity entity);
        void UpdateRange(List<Tentity> entities);
    }
}
