// Copyright 2019 Richard Nusser
// Licensed under GPLv3 (see http://www.gnu.org/licenses/)

using System.Collections.Generic;

namespace Bulkr.Core.Services
{
	/// <summary>
	///   Base interface to identify CRUD services.
	/// </summary>
	public interface CRUDService<MODEL, ID>
	{
		/// <summary>
		///   Finds an item by ID.
		/// </summary>
		/// <param name="id">The ID to look up.</param>
		/// <returns>The item.</returns>
		MODEL GetByID(ID id);

		/// <summary>
		///   Retrieves all stored items.
		/// </summary>
		/// <returns>The list of stored items.</returns>
		IList<MODEL> GetAll();

		/// <summary>
		///   Adds a new item.
		/// </summary>
		/// <note>The item's .ID value will be ignored and overwritten.</note>
		/// <param name="item">The item to add.</param>
		/// <returns>The item, with its new ID.</returns>
		MODEL Add(MODEL item);

		/// <summary>
		///   Updates an existing item.
		/// </summary>
		/// <param name="item">The item to save.</param>
		/// <returns>The same item.</returns>
		MODEL Update(MODEL item);

		/// <summary>
		///   Deletes an existing item.
		/// </summary>
		/// <param name="item">The item to delete.</param>
		void Delete(MODEL item);
	}
}
