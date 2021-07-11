using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Core.Entity.Abstracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace Core.Data
{
    /// <summary>
    /// Represents a default generic repository implements the <see cref="GenericRepository{TEntity}"/> abstract class.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    public sealed class GenericRepository<TEntity> where TEntity : BaseEntity
    {
        private readonly DbContext _dbContext;
        private readonly DbSet<TEntity> _dbSet;

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericRepository{TEntity}"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        public GenericRepository(DbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _dbSet = _dbContext.Set<TEntity>();
        }

        /// <summary>
        /// Gets the first or default entity based on a selector predicate, include delegate. This method default no-tracking query.
        /// </summary>
        /// <param name="selector">A function to take which property from <see cref="TEntity"/></param>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <param name="include">A function to include navigation properties</param>
        /// <returns>An <see cref="IQueryable{TEntity}"/> that contains elements that satisfy the condition specified by <paramref name="predicate"/>.</returns>
        /// <remarks>This method default no-tracking query.</remarks>
        public IQueryable<TEntity> GetFirstOrDefaultAsQueryable(Expression<Func<TEntity, TEntity>> selector = null,
            Expression<Func<TEntity, bool>> predicate = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null)
        {
            var query = _dbSet.AsQueryable();
            
            query = query.AsNoTracking();

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            if (selector != null)
            {
                query = query.Select(selector);
            }

            if (include != null)
            {
                query = include(query);
            }

            return query;
        }

        /// <summary>
        /// Gets the first or default entity based on a id predicate. This method default no-tracking query.
        /// </summary>
        /// <param name="id">A id to test for match <see cref="long"/></param>
        /// <returns>An <see cref="IQueryable{TEntity}"/> that contains elements that satisfy the id condition specified by <paramref name="id"/>.</returns>
        /// <remarks>This method default no-tracking query.</remarks>
        public IQueryable<TEntity> GetFirstOrDefaultAsQueryable(long id)
        {
            return GetFirstOrDefaultAsQueryable(null, t => t.Id == id);
        }

        /// <summary>
        /// Inserts a new entity asynchronously.
        /// </summary>
        /// <param name="entity">The entity to insert.</param>
        public async Task Add(TEntity entity)
        {
            await _dbSet.AddAsync(entity);
        }

        /// <summary>
        /// Inserts a range of entities asynchronously.
        /// </summary>
        /// <param name="entities">The entities to insert.</param>
        public async Task Add(IEnumerable<TEntity> entities)
        {
            await _dbSet.AddRangeAsync(entities);
        }

        /// <summary>
        /// Inserts a range of entities asynchronously.
        /// </summary>
        /// <param name="entities">The entities to insert.</param>
        public async Task Add(TEntity[] entities)
        {
            await _dbSet.AddRangeAsync(entities);
        }

        /// <summary>
        /// Updates the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public void Update(TEntity entity)
        {
            _dbSet.Update(entity);
        }

        /// <summary>
        /// Updates the specified entities.
        /// </summary>
        /// <param name="entities">The entities.</param>
        public void Update(IEnumerable<TEntity> entities)
        {
            _dbSet.UpdateRange(entities);
        }

        /// <summary>
        /// Updates the specified entities.
        /// </summary>
        /// <param name="entities">The entities.</param>
        public void Update(TEntity[] entities)
        {
            _dbSet.UpdateRange(entities);
        }

        /// <summary>
        /// Remove one entity <see cref="TEntity"/>.
        /// </summary>
        /// <param name="entity">The entity to remove.</param>
        public void Remove(TEntity entity)
        {
            _dbSet.Remove(entity);
        }

        /// <summary>
        /// Remove the specified entities
        /// </summary>
        /// <param name="entities">The entities to remove.</param>
        public void Remove(IEnumerable<TEntity> entities)
        {
            _dbSet.RemoveRange(entities);
        }

        /// <summary>
        /// Remove the specified entities
        /// </summary>
        /// <param name="entities">The entities to remove.</param>
        public void Remove(TEntity[] entities)
        {
            _dbSet.RemoveRange(entities);
        }

        /// <summary>
        /// Find and remove entity which has <see cref="long"/>.
        /// </summary>
        /// <param name="id">The id of data to remove.</param>
        public async void Remove(long id)
        {
            var firstOrDefaultAsync = await GetFirstOrDefaultAsQueryable(null, t => t.Id == id).FirstOrDefaultAsync();

            if (firstOrDefaultAsync != null)
            {
                Remove(firstOrDefaultAsync);
            }
        }

        /// <summary>
        /// Find and remove entities which has <see cref="IEnumerable{Long}"/>.
        /// </summary>
        /// <param name="ids">The ids of data's to remove.</param>
        public async void Remove(IEnumerable<long> ids)
        {
            var contains = await GetAllAsQueryable(null, t => ids.Contains(t.Id)).ToListAsync();

            if (contains != null)
            {
                Remove(contains);
            }
        }

        /// <summary>
        /// Find and remove entity which match with the condition <see cref="Expression{TDelegate}"/>.
        /// </summary>
        /// <param name="predicate">The condition to remove data.</param>
        public async void Remove(Expression<Func<TEntity, bool>> predicate)
        {
            var all = await GetAllAsQueryable(null, predicate).ToListAsync();

            if (all != null)
            {
                Remove(all);
            }
        }

        /// <summary>
        /// Gets the count based on a predicate.
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns>Number of data count</returns>
        public async Task<int> Count(Expression<Func<TEntity, bool>> predicate = null)
        {
            return predicate == null ? await _dbSet.CountAsync() : await _dbSet.CountAsync(predicate);
        }

        /// <summary>
        /// Gets the exists based on a predicate.
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns>Check if the data exist</returns>
        public async Task<bool> Exist(Expression<Func<TEntity, bool>> predicate)
        {
            return await _dbSet.AnyAsync(predicate);
        }

        /// <summary>
        /// Gets the exists based on a id. <see cref="long"/>
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Check id if the data exist</returns>
        public async Task<bool> ExistById(long id)
        {
            return await _dbSet.AnyAsync(t => t.Id == id);
        }

        /// <summary>
        /// Gets all entities. This method default no-tracking query.
        /// </summary>
        /// <param name="selector">A function to take which property from <see cref="TEntity"/></param>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <param name="orderBy">A function to order elements.</param>
        /// <param name="include">A function to include navigation properties</param>
        /// <returns>An <see cref="IQueryable{TEntity}"/> that contains elements that satisfy the condition specified by <paramref name="predicate"/>.</returns>
        /// <remarks>Ex: This method defaults to a read-only, no-tracking query.</remarks>
        public IQueryable<TEntity> GetAllAsQueryable(Expression<Func<TEntity, TEntity>> selector = null,
            Expression<Func<TEntity, bool>> predicate = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null)
        {
            var query = _dbSet.AsQueryable();
            
            query = query.AsNoTracking();

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            if (selector != null)
            {
                query = query.Select(selector);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            if (include != null)
            {
                query = include(query);
            }

            return query;
        }

        /// <summary>
        /// Gets all entities which contains in <paramref name="ids"/>. This method default no-tracking query.
        /// </summary>
        /// <param name="ids">A list of long to test contains condition.</param>
        /// <returns>An <see cref="IQueryable{TEntity}"/> that contains elements that satisfy the id contains condition specified by <paramref name="ids"/>.</returns>
        /// <remarks>Ex: This method defaults to a read-only, no-tracking query.</remarks>
        public IQueryable<TEntity> GetAllAsQueryable(IEnumerable<long> ids)
        {
            return GetAllAsQueryable(null, t => ids.Contains(t.Id));
        }

        /// <summary>
        /// Uses raw SQL queries to fetch the specified <typeparamref name="TEntity" /> data.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="sql">The raw SQL.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>An <see cref="IQueryable{TEntity}" /> that contains elements that satisfy the condition specified by raw SQL.</returns>
        public IQueryable<TEntity> FromSqlAsQueryable(string sql, params object[] parameters) =>
            _dbContext.Set<TEntity>().FromSqlRaw(sql, parameters);
    }
}