// Copyright 2019 Richard Nusser
// Licensed under GPLv3 (see http://www.gnu.org/licenses/)

using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Bulkr.Core.Models;

namespace Bulkr.Core.Services
{
	/// <summary>
	///   Base class for CRUD services.
	///   <para>
	///     These services implement the common "repository" pattern for simple item management UIs.
	///     All operations will commit changes immediately.
	///   </para>
	/// </summary>
	public abstract class Service<MODEL> where MODEL : Model, new()
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
		protected Service(DbContext databaseContext,DbSet<MODEL> dbSet)
		{
			DatabaseContext=databaseContext;
			DbSet=dbSet;
		}


		/// <summary>
		///   Finds an item by ID.
		/// </summary>
		/// <param name="id">The ID to look up.</param>
		/// <returns>The item.</returns>
		public MODEL GetByID(int id)
		{
			return DbSet.Find(id);
		}

		/// <summary>
		///   Retrieves all stored items.
		/// </summary>
		/// <returns>The list of stored items.</returns>
		public IList<MODEL> GetAll()
		{
			return DbSet.ToList();
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
			DatabaseContext.Remove(item);
			DatabaseContext.SaveChanges();
		}
	}
}
