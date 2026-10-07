using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using ProductManagement.Data.Context;
using ProductManagement.Data.Interfaces;
using ProductManagement.DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagement.DataAccess.Repositories
{
    public abstract class BaseRepository<TEntity, TIdType>(IDbContextFactory<ApplicationDbContext> dbContextFactory)
    where TEntity : class, IBaseEntity<TIdType>
    {
        protected readonly IDbContextFactory<ApplicationDbContext> DbContextFactory = dbContextFactory;

        /// <summary>
        /// Insert a entity in database
        /// </summary>
        /// <param name="entity">Entity</param>
        /// <returns>A entity</returns>
        public virtual async Task<TEntity> InsertAsync(TEntity entity)
        {
            await using var dbContext = await DbContextFactory?.CreateDbContextAsync();
            EntityEntry<TEntity> result = await dbContext.AddAsync(entity);
            await dbContext.SaveChangesAsync();
            return result.Entity;
        }

        /// <summary>
        /// Insert a entity in database
        /// </summary>
        /// <param name="context">DbContext instance</param>
        /// <param name="entity">Entity</param>
        /// <returns>A entity</returns>
        public virtual async Task<TEntity> InsertAsync(DbContext context, TEntity entity)
        {
            EntityEntry<TEntity> result = await context.AddAsync(entity);
            return result.Entity;
        }


        /// <summary>
        /// Delete a entity from database
        /// </summary>
        /// <param name="entity">Entity</param>
        /// <returns>True if success</returns>
        public virtual async Task<bool> DeleteAsync(TEntity entity)
        {
            try
            {
                await using var dbContext = await DbContextFactory?.CreateDbContextAsync();
                dbContext.Remove(entity);
                await dbContext.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Delete a entity from database
        /// </summary>
        /// <param name="context">DbContext instance</param>
        /// <param name="entity">Entity</param>
        /// <returns>True if success</returns>
        public virtual Task<bool> DeleteAsync(DbContext context, TEntity entity)
        {
            try
            {
                context.Remove(entity);
                return Task.FromResult(true);
            }
            catch
            {
                return Task.FromResult(false);
            }
        }

        /// <summary>
        /// Update a entity from database
        /// </summary>
        /// <param name="entity">Entity</param>
        /// <returns>True if success</returns>
        public virtual async Task<TEntity> UpdateAsync(TEntity entity)
        {
            await using var dbContext = await DbContextFactory?.CreateDbContextAsync();
            dbContext.Update(entity);
            await dbContext.SaveChangesAsync();
            return entity;
        }

        /// <summary>
        /// Update a entity from database
        /// </summary>
        /// <param name="context">DbContext instance</param>
        /// <param name="entity">Entity</param>
        /// <returns>True if success</returns>
        public virtual Task<TEntity> UpdateAsync(DbContext context, TEntity entity)
        {
            context.Update(entity);
            return Task.FromResult(entity);
        }

        /// <summary>
        /// Get all values from entity
        /// </summary>
        /// <returns>A IQueryable with all values from entities</returns>
        public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            await using var dbContext = await DbContextFactory?.CreateDbContextAsync();
            IQueryable<TEntity> dbQuery = dbContext.Set<TEntity>();

            return await dbQuery.AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// Get all entity values
        /// </summary>
        /// <param name="context">Database context</param>
        /// <returns>A IEnumerable with entity values</returns>
        public virtual async Task<IEnumerable<TEntity>> GetAllAsync(DbContext context)
        {
            IQueryable<TEntity> dbQuery = context.Set<TEntity>();

            return await dbQuery.AsNoTracking().ToListAsync(); //Don't track any changes for the selected item
        }

        /// <summary>
        /// Get all entity values where lambda expression
        /// </summary>
        /// <param name="where">Where expression</param>
        /// <returns>A IQueryable with entity values</returns>
        public virtual async Task<IEnumerable<TEntity>> GetAllWhereAsync(
            Expression<Func<TEntity, bool>> where)
        {
            await using var dbContext = await DbContextFactory?.CreateDbContextAsync();
            IQueryable<TEntity> dbQuery = dbContext.Set<TEntity>();

            return await dbQuery.Where(where).AsNoTracking()
                .ToListAsync(); //Don't track any changes for the selected item
        }

        /// <summary>
        /// Get all entity values where lambda expression
        /// </summary>
        /// <param name="context">DbContext instance</param>
        /// <param name="where">Where expression</param>
        /// <returns>A IQueryable with entity values</returns>
        public virtual async Task<IEnumerable<TEntity>> GetAllWhereAsync(DbContext context,
            Expression<Func<TEntity, bool>> where)
        {
            IQueryable<TEntity> dbQuery = context.Set<TEntity>();

            return await dbQuery.Where(where).AsNoTracking()
                .ToListAsync(); //Don't track any changes for the selected item
        }

        /// <summary>
        /// Get an entity by identifier
        /// </summary>
        /// <param name="context">DbContext instance</param>
        /// <param name="id">Identifier</param>
        /// <returns>An entity</returns>
        public virtual async Task<TEntity> GetByIdAsync(DbContext context, TIdType id)
        {
            IQueryable<TEntity> dbQuery = context.Set<TEntity>();

            return await dbQuery.Where(p => p.Id.Equals(id))
                .FirstOrDefaultAsync(); //Don't track any changes for the selected item
        }

        /// <summary>
        /// Get an entity by identifier
        /// </summary>
        /// <param name="id">Identifier</param>
        /// <returns>An entity</returns>
        public virtual async Task<TEntity> GetByIdAsync(TIdType id)
        {
            await using var dbContext = await DbContextFactory?.CreateDbContextAsync();
            IQueryable<TEntity> dbQuery = dbContext.Set<TEntity>();

            return await dbQuery.Where(p => p.Id.Equals(id))
                .FirstOrDefaultAsync(); //Don't track any changes for the selected item
        }

        /// <summary>
        /// Get a single entity where lambda expression
        /// </summary>
        /// <param name="where">Where expression</param>
        /// <returns>A entity</returns>
        public virtual async Task<TEntity> GetSingleOrDefaultAsync(Expression<Func<TEntity, bool>> where)
        {
            await using var dbContext = await DbContextFactory?.CreateDbContextAsync();
            IQueryable<TEntity> dbQuery = dbContext.Set<TEntity>();

            return await dbQuery.Where(where).AsNoTracking().SingleOrDefaultAsync();
        }

        /// <summary>
        /// Get a single entity where lambda expression
        /// </summary>
        /// <param name="context">DbContext instance</param>
        /// <param name="where">Where expression</param>
        /// <returns>A entity</returns>
        public virtual async Task<TEntity> GetSingleOrDefaultAsync(DbContext context,
            Expression<Func<TEntity, bool>> where)
        {
            IQueryable<TEntity> dbQuery = context.Set<TEntity>();

            return await dbQuery.Where(where).AsNoTracking().SingleOrDefaultAsync();
        }

        /// <summary>
        /// Get the first entity where lambda expression
        /// </summary>
        /// <param name="where">Where expression</param>
        /// <returns></returns>
        public virtual async Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> where)
        {
            await using DbContext context = await DbContextFactory.CreateDbContextAsync();
            return await FirstOrDefaultAsync(context, where);
        }

        /// <summary>
        /// Get the first entity where lambda expression
        /// </summary>
        /// <param name="context">DbContext instance</param>
        /// <param name="where">Where expression</param>
        /// <returns></returns>
        public virtual async Task<TEntity> FirstOrDefaultAsync(DbContext context, Expression<Func<TEntity, bool>> where)
        {
            IQueryable<TEntity> dbQuery = context.Set<TEntity>();

            return await dbQuery.Where(where).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Check if the entity exists
        /// </summary>
        /// <param name="context">DbContext instance</param>
        /// <param name="where">Where expression</param>
        /// <returns>A IQueryable with entity values</returns>
        public virtual async Task<bool> AnyAsync(DbContext context, Expression<Func<TEntity, bool>> where)
        {
            IQueryable<TEntity> dbQuery = context.Set<TEntity>();
            return await dbQuery.Where(where).AnyAsync();
        }

        /// <summary>
        /// Check if the entity exists
        /// </summary>
        /// <param name="where">Where expression</param>
        /// <returns>A boolean</returns>
        public virtual async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> where)
        {
            await using var dbContext = await DbContextFactory?.CreateDbContextAsync();
            IQueryable<TEntity> dbQuery = dbContext.Set<TEntity>();
            return await dbQuery.Where(where).AsNoTracking()
                .AnyAsync();
        }
    }
}
