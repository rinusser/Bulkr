// Copyright 2019 Richard Nusser
// Licensed under GPLv3 (see http://www.gnu.org/licenses/)

using System.Collections.Generic;
using Bulkr.Core.Models;
using Bulkr.Core.Persistence;

namespace Bulkr.Core.Services
{
	public class FoodService : Service<Food>
	{
		public static FoodService CreateInMemoryInstance(string name)
		{
			return new FoodService(BulkrContext.CreateInMemoryInstance(name));
		}

		public static FoodService CreatePersistentInstance()
		{
			return new FoodService(BulkrContext.GetPersistentInstance());
		}


		public FoodService(BulkrContext databaseContext) : base(databaseContext,databaseContext.Foods)
		{
		}
	}
}
