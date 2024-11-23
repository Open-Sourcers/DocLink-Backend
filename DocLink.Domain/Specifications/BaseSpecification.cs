using DocLink.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DocLink.Domain.Specifications
{
    public class BaseSpecification<TEntity, Tkey> : ISpecifications<TEntity, Tkey> where TEntity : BaseEntity<Tkey>
    {
        public List<Expression<Func<TEntity, object>>> Includes { get; set; } = new List<Expression<Func<TEntity, object>>>();
        public Expression<Func<TEntity, bool>> Criteria { get; set; }
        public Expression<Func<TEntity, object>> OrderBy { get; set; }
        public Expression<Func<TEntity, object>> OrderByDescending { get; set; }    
        public int Skip { get; set; }
        public int Take { get; set; }
        public bool isPaginationEnable { get; set; }

        public BaseSpecification() { }

        public BaseSpecification(Expression<Func<TEntity, bool>> critertia)
        {
            Criteria = critertia;
        }

        public void AddOrder(Expression<Func<TEntity, object>> expression) { OrderBy = expression; }
        public void AddOrderDescending(Expression<Func<TEntity, object>> expression) { OrderByDescending = expression; }

        public void ApplayPagination(int skip, int take)
        {
            isPaginationEnable = true;
            Skip = skip;
            Take = take;
        }
    }
}
