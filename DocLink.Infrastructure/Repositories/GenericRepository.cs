using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocLink.Domain.Entities;
using DocLink.Domain.Interfaces.Repositories;
using DocLink.Domain.Specifications;
using DocLink.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DocLink.Infrastructure.Repositories
{
    public class GenericRepository<TEntity, TKey> : IGenericRepository<TEntity, TKey> where TEntity : BaseEntity<TKey>
    {
        private readonly DocLinkDbContext _dbContext;
        private readonly DbSet<TEntity> _dbSet;
        public GenericRepository(DocLinkDbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<TEntity>();
        }
        public async Task AddAsync(TEntity item)
        => await _dbSet.AddAsync(item);

        public void Remove(TEntity item)
        => _dbSet.Remove(item);

        public async Task<IReadOnlyList<TEntity>> GetAllAsync()
        => await _dbSet.ToListAsync();

        public async Task<IReadOnlyList<TEntity>> GetAllWithSpecAsync(ISpecifications<TEntity, TKey> spec)
        => await ApplySpecification(spec).ToListAsync();

        public async Task<TEntity> GetByIdAsync(TKey id)
        => (await _dbSet.FindAsync(id))!;

        public async Task<TEntity> GetEntityWithSpecAsync(ISpecifications<TEntity, TKey> spec)
        => (await ApplySpecification(spec).FirstOrDefaultAsync())!;

        private IQueryable<TEntity> ApplySpecification(ISpecifications<TEntity, TKey> Spec)
        => SpacificationEvaluator<TEntity, TKey>.GetQuery(_dbSet, Spec);

        public void Update(TEntity item)
        => _dbSet.Update(item);
    }
}
