using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocLink.Domain.Entities;
using DocLink.Domain.Specifications;

namespace DocLink.Domain.Interfaces.Repositories
{
    public interface IGenericRepository<TEntity, TKey> where TEntity : BaseEntity<TKey>
    {
        #region Without Specification
        Task<IReadOnlyList<TEntity>> GetAllAsync();

        Task<TEntity> GetByIdAsync(int id);
        #endregion

        #region With Specification
        Task<IReadOnlyList<TEntity>> GetAllWithSpecAsync(ISpecifications<TEntity, TKey> spec);
        Task<TEntity> GetEntityWithSpecAsync(ISpecifications<TEntity, TKey> spec);
        #endregion

        Task AddAsync(TEntity item);
        void Remove(TEntity item);
        void Update(TEntity item);
    }
}
