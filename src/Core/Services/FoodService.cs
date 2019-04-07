// Copyright 2019 Richard Nusser
// Licensed under GPLv3 (see http://www.gnu.org/licenses/)

using Bulkr.Core.Models;
using Bulkr.Core.Persistence;

namespace Bulkr.Core.Services
{
	/// <summary>
	///   CRUD Service for managing <see cref="Food"/> items.
	/// </summary>
	public class FoodService : DatabaseCRUDService<Food>
	{
		/// <summary>
		///   Instantiates a new <see cref="FoodService"/> instance.
		/// </summary>
		/// <remarks>
		///   If no name is supplied, the application configuration's default will be used.
		/// </remarks>
		/// <param name="name">Optional: a database name to use, to e.g. distinguish individual tests.</param>
		/// <returns>The instance.</returns>
		public static FoodService Create(string name = null)
		{
			return new FoodService(BulkrContext.GetInstance(name));
		}


		/// <summary>
		///   Basic constructor.
		/// </summary>
		/// <param name="databaseContext">The database context to use.</param>
		public FoodService(BulkrContext databaseContext) : base(databaseContext,databaseContext.Foods)
		{
		}
	}
}
