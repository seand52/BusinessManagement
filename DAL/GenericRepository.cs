using System.Linq.Expressions;
using BusinessManagement.DAL;
using BusinessManagement.Database;
using BusinessManagement.Filter;
using BusinessManagement.Helpers;
using Microsoft.EntityFrameworkCore;

namespace ContosoUniversity.DAL
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        internal ApplicationContext _context;
        internal DbSet<TEntity> dbSet;

        public GenericRepository(ApplicationContext context)
        {
            _context = context;
            dbSet = context.Set<TEntity>();
        }

        public virtual async Task<PagedList<TEntity>> GetAllBy(
            Expression<Func<TEntity, bool>> filter,
            PaginationFilter paginationFilter,
            Expression<Func<TEntity, bool>>? searchQuery = null,
            string includeProperties  = ""
            )
        {
            IQueryable<TEntity> query = dbSet.Where(filter);

            if (searchQuery != null)
            {
                query = query.Where(searchQuery);
            }
            
            foreach (var includeProperty in includeProperties.Split
                         (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            return await PagedList<TEntity>.CreateAsync(query, paginationFilter.PageNumber, paginationFilter.PageSize);
        }

        public virtual async Task<TEntity?> GetBy(Expression<Func<TEntity, bool>> filter = null)
        {
            return await dbSet.AsNoTracking().Where(filter).FirstOrDefaultAsync();
        }

        public virtual async Task Insert(TEntity entity)
        {
            await dbSet.AddAsync(entity);
        }

        public virtual void Delete(TEntity entity)
        {
            dbSet.Remove(entity);
        }

        public virtual void Update(TEntity entityToUpdate)
        {
            _context.Entry(entityToUpdate).State = EntityState.Modified;
        }
    }
}