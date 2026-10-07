using Microsoft.EntityFrameworkCore;
using ProductManagement.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagement.DataAccess.Interfaces
{
    public interface IBaseRepository<TEntity> : IBaseRepository<TEntity, int>
    where TEntity : class, IEntity;

    public interface IBaseRepository<TEntity, in TType>
        where TEntity : class, IBaseEntity<TType>
    {
        Task<TEntity> InsertAsync(TEntity entity);
        Task<TEntity> InsertAsync(DbContext context, TEntity entity);
        Task<bool> DeleteAsync(TEntity entityToDelete);
        Task<bool> DeleteAsync(DbContext context, TEntity entity);
        Task<TEntity> UpdateAsync(TEntity entityToUpdate);
        Task<TEntity> UpdateAsync(DbContext context, TEntity entity);
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<IEnumerable<TEntity>> GetAllAsync(DbContext context);
        Task<IEnumerable<TEntity>> GetAllWhereAsync(Expression<Func<TEntity, bool>> where);
        Task<IEnumerable<TEntity>> GetAllWhereAsync(DbContext context, Expression<Func<TEntity, bool>> where);
        Task<TEntity> GetByIdAsync(TType id);
        Task<TEntity> GetByIdAsync(DbContext context, TType id);
        Task<TEntity> GetSingleOrDefaultAsync(Expression<Func<TEntity, bool>> where);
        Task<TEntity> GetSingleOrDefaultAsync(DbContext context, Expression<Func<TEntity, bool>> where);
        Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> where);
        Task<TEntity> FirstOrDefaultAsync(DbContext context, Expression<Func<TEntity, bool>> where);

        Task<bool> AnyAsync(DbContext context, Expression<Func<TEntity, bool>> where);
        Task<bool> AnyAsync(Expression<Func<TEntity, bool>> where);
    }
}
