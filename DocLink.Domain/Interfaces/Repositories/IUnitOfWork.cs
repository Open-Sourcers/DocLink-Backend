using DocLink.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocLink.Domain.Interfaces.Repositories
{
	public interface IUnitOfWork:IAsyncDisposable
	{
		IGenericRepository<TEntity, TKey> Repository<TEntity, TKey>() where TEntity : BaseEntity<TKey>;
		Task<int> SaveAsync();
	}
}
