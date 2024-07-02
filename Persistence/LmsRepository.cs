using LMS.Application.Interfaces;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Application.Interfaces;
using LMS.Persistence.Db;

namespace EcommercePersistence
{
    public class LmsRepository<Tentity> : ILmsRepository<Tentity> where Tentity : class
    {
        private readonly DatabaseService _dbContext;

        public LmsRepository(DatabaseService dbContext)
        {
            _dbContext = dbContext;
        }

        public void Create(Tentity entity)
        {
            _dbContext.Set<Tentity>().Add(entity);
        }

        public void CreateRange(List<Tentity> entities)
        {
            _dbContext.Set<Tentity>().AddRange(entities);
        }

        public void Delete(Tentity entity)
        {
            _dbContext.Set<Tentity>().Remove(entity);
        }

        public void DeleteRange(List<Tentity> entities)
        {
            _dbContext.Set<Tentity>().RemoveRange(entities);
        }

        public IQueryable<Tentity> GetAll()
        {
            return _dbContext.Set<Tentity>();
        }

        public IQueryable<Tentity> GetByCondition(Expression<Func<Tentity, bool>> expression)
        {
            return _dbContext.Set<Tentity>().Where(expression);
        }

        public async Task<Tentity> GetByIdAsync(int id)
        {
            return await _dbContext.Set<Tentity>().FindAsync(id);
        }

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        public void Update(Tentity entity)
        {
            _dbContext.Set<Tentity>().Update(entity);
        }

        public void UpdateRange(List<Tentity> entities)
        {
            _dbContext.Set<Tentity>().UpdateRange(entities);
        }
    }
}
