using DocLink.Domain.Entities;
using DocLink.Domain.Specifications;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocLink.Infrastructure
{
    public static class SpacificationEvaluator<TEntity , TKey> where TEntity : BaseEntity<TKey>
    {
        public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> dbset, ISpecifications<TEntity,TKey> specifications)
        {
            IQueryable<TEntity> query = dbset;

            if (specifications.Criteria is not null)
                query = query.Where(specifications.Criteria);


            if (specifications.OrderBy is not null)
                query = query.OrderBy(specifications.OrderBy);

            if (specifications.OrderByDescending is not null)
                query = query.OrderByDescending(specifications.OrderByDescending);

            if (specifications.isPaginationEnable)
                query = query.Skip(specifications.Skip).Take(specifications.Take);

            if (specifications.Includes.Count > 0)
                query = specifications.Includes.Aggregate(query, (curQuery, include) => curQuery.Include(include));

            return query;
        }
    }
}
