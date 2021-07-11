using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entity.Abstracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace Core.Data
{
    /// <summary>
    /// Represents the default implementation of the <see cref="UnitOfWork{TContext}"/> class.
    /// </summary>
    /// <typeparam name="TContext">The type of the db context.</typeparam>
    public class UnitOfWork<TContext> where TContext : DbContext, IDisposable
    {
        private readonly TContext _dbContext;
        private bool _disposed;
        private Dictionary<Type, object> _repositories;

        /// <summary>
        /// Initializes a new instance of the <see cref="UnitOfWork{TContext}"/> class.
        /// </summary>
        /// <param name="dbContext">The context.</param>
        public UnitOfWork(TContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        /// <summary>
        /// Gets the specified repository for the <typeparamref name="TEntity"/>.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <returns>An instance of type inherited from <see cref="GenericRepository{TEntity}"/> abstract class.</returns>
        public GenericRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity
        {
            if (_repositories == null)
            {
                _repositories = new Dictionary<Type, object>();
            }

            var entityType = typeof(TEntity);

            if (_repositories.Keys.Contains(entityType)) return (GenericRepository<TEntity>) _repositories[entityType];

            var repositoryType = typeof(GenericRepository<>);
            var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(entityType), _dbContext);
            _repositories.Add(entityType, repositoryInstance);

            return (GenericRepository<TEntity>) _repositories[entityType];
        }

        /// <summary>
        /// Executes the specified raw SQL command.
        /// </summary>
        /// <param name="sql">The raw SQL.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>The data reader for the result of SQL.</returns>
        public async Task<RelationalDataReader> ExecuteSqlCommand(string sql, params object[] parameters)
        {
            var database = _dbContext.Database;
            
            using (database.GetService<IConcurrencyDetector>().EnterCriticalSection())
            {
                var rawSqlCommand = database.GetService<IRawSqlCommandBuilder>().Build(sql, parameters);
                var paramObject = new RelationalCommandParameterObject(database.GetService<IRelationalConnection>(), rawSqlCommand.ParameterValues, null, null, null);
                
                return await rawSqlCommand.RelationalCommand.ExecuteReaderAsync(paramObject);
            }
        }

        public async Task Commit()
        {
            await _dbContext.SaveChangesAsync();
        }
        
        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }
        
        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <param name="disposing">The disposing.</param>
        private void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _repositories?.Clear();

                    _dbContext.Dispose();
                }
            }

            _disposed = true;
        }
    }
}