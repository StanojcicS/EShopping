using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using UserManagement.Repositories.Contracts;

namespace UserManagement.Repositories
{
    public class GenericRepository<TEntity, TDbContext> : IGenericRepository<TEntity>
        where TEntity : class
        where TDbContext : DbContext
    {
        protected readonly TDbContext _context;

        public GenericRepository(TDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TEntity>> GetAll()
        {
            return await _context.Set<TEntity>().ToListAsync();
        }

        public async Task<TEntity> GetById(object id)
        {
            return await _context.Set<TEntity>().FindAsync(id);
        }

        public async Task<TEntity> Insert(TEntity entity)
        {
            return (await _context.Set<TEntity>().AddAsync(entity)).Entity;
        }

        public async Task<TEntity> Update(TEntity entity, object id)
        {
            TEntity currentEntity = await _context.Set<TEntity>().FindAsync(id);
            _context.Entry(currentEntity).CurrentValues.SetValues(entity);
            return await _context.Set<TEntity>().FindAsync(id);
        }

        public Task<TEntity> Delete(TEntity entity)
        {
            _context.Set<TEntity>().Remove(entity);
            return null;
        }

        public async Task<TEntity?> FindAsync(Expression<Func<TEntity, bool>> filter)
        {
            return await _context.Set<TEntity>().FirstOrDefaultAsync(filter);
        }
    }
}
