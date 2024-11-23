using DocLink.Domain.Entities;
using DocLink.Domain.Interfaces.Repositories;
using DocLink.Infrastructure.Data;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocLink.Infrastructure.Repositories
{
	public class UnitOfWork : IUnitOfWork
	{
		private readonly DocLinkDbContext _context;
		private readonly Hashtable _repositories;
		public UnitOfWork(DocLinkDbContext context)
		{
			_context = context;
			_repositories = new Hashtable();
		}
		public IGenericRepository<TEntity, TKey> Repository<TEntity, TKey>() where TEntity : BaseEntity<TKey>
		{
			var key = typeof(TEntity).Name;
			if (!_repositories.ContainsKey(key))
			{
				var repo = new GenericRepository<TEntity, TKey>(_context);
				_repositories.Add(key, repo);
			}
			return (GenericRepository<TEntity, TKey>) _repositories[key]!;
		}

		public async Task<int> SaveAsync()
		{
			return await _context.SaveChangesAsync();
		}

		public async ValueTask DisposeAsync()
		{
			await _context.DisposeAsync();
		}

		
	}
}
