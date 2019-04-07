// Copyright 2019 Richard Nusser
// Licensed under GPLv3 (see http://www.gnu.org/licenses/)

using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

using Bulkr.Core.Models;

namespace Bulkr.Core.Services
{
	/// <summary>
	///   Base class for database CRUD services.
	///   <para>
	///     These services implement the common "repository" pattern for simple item management UIs.
	///     All operations will commit changes immediately.
	///   </para>
	/// </summary>
	public abstract class DatabaseCRUDService<MODEL> : CRUDService<MODEL,int> where MODEL : Model, new()
	{
		/// <summary>
		///   The application's database context.
		/// </summary>
		public DbContext DatabaseContext { get; }

		/// <summary>
		///   The Entity Framework repository for this service.
		/// </summary>
		public DbSet<MODEL> DbSet;


		/// <summary>
		///   Basic constructor.
		/// </summary>
		/// <param name="databaseContext">The database context to use.</param>
		/// <param name="dbSet">The repository to handle.</param>
		protected DatabaseCRUDService(DbContext databaseContext,DbSet<MODEL> dbSet)
		{
			DatabaseContext=databaseContext;
			DbSet=dbSet;
		}


		/// <summary>
		///   Returns the DbSet, ready for actual use. Override this method if you e.g. need to eager load references.
		/// </summary>
		/// <returns>An <see cref="IQueryable"/> instance to use for read accesses.</returns>
		protected virtual IQueryable<MODEL> GetConfiguredDbSet()
		{
			return DbSet;
		}

		/// <summary>
		///   Finds an item by ID.
		/// </summary>
		/// <param name="id">The ID to look up.</param>
		/// <returns>The item.</returns>
		public MODEL GetByID(int id)
		{
			return GetConfiguredDbSet().First(i => i.ID==id);
		}

		/// <summary>
		///   Retrieves all stored items.
		/// </summary>
		/// <returns>The list of stored items.</returns>
		public IList<MODEL> GetAll()
		{
			return GetConfiguredDbSet().ToList();
		}

		/// <summary>
		///   Adds a new item.
		/// </summary>
		/// <note>The item's .ID value will be ignored and overwritten.</note>
		/// <param name="item">The item to add.</param>
		/// <returns>The item, with its new ID.</returns>
		public MODEL Add(MODEL item)
		{
			item.ID=0;
			DetachReferences(item);
			DatabaseContext.Add(item);
			DatabaseContext.SaveChanges();
			return item;
		}

		/// <summary>
		///   Updates an existing item.
		/// </summary>
		/// <param name="item">The item to save.</param>
		/// <returns>The same item.</returns>
		public MODEL Update(MODEL item)
		{
			DetachReferences(item);
			DatabaseContext.Update(item);
			DatabaseContext.SaveChanges();
			return item;
		}

		/// <summary>
		///   Deletes an existing item.
		/// </summary>
		/// <param name="item">The item to delete.</param>
		public void Delete(MODEL item)
		{
			DetachReferences(item);
			DatabaseContext.Remove(item);
			DatabaseContext.SaveChanges();
		}

		/// <summary>
		///   Override this method if you need to detach entity references before writing an item to the database.
		/// </summary>
		/// <param name="item">The item to update.</param>
		protected virtual void DetachReferences(MODEL item)
		{
		}
	}
}
