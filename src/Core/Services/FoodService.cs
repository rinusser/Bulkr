// Copyright 2019 Richard Nusser
// Licensed under GPLv3 (see http://www.gnu.org/licenses/)

using Bulkr.Core.Models;
using Bulkr.Core.Persistence;

namespace Bulkr.Core.Services
{
	/// <summary>
	///   CRUD Service for managing <see cref="Food"/> items.
	/// </summary>
	public class FoodService : Service<Food>
	{
		/// <summary>
		///   TODO: change to app config
		/// </summary>
		/// <param name="name">name</param>
		/// <returns>in-memory instance</returns>
		public static FoodService CreateInMemoryInstance(string name)
		{
			return new FoodService(BulkrContext.CreateInMemoryInstance(name));
		}

		/// <summary>
		///   TODO: change to app config
		/// </summary>
		/// <returns>persistent instance</returns>
		public static FoodService CreatePersistentInstance()
		{
			return new FoodService(BulkrContext.GetPersistentInstance());
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
