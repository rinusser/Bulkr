// Copyright 2019 Richard Nusser
// Licensed under GPLv3 (see http://www.gnu.org/licenses/)

using System.Collections.Generic;
using System.Linq;
using Bulkr.Core.Models;
using Bulkr.Core.Persistence;

namespace Bulkr.Core.Services
{
	public class FoodService
	{
		public static FoodService CreateInMemoryInstance(string name)
		{
			return new FoodService(BulkrContext.CreateInMemoryInstance(name));
		}

		public static FoodService CreatePersistentInstance()
		{
			return new FoodService(BulkrContext.GetPersistentInstance());
		}


		public BulkrContext DatabaseContext { get; }


		public FoodService(BulkrContext databaseContext)
		{
			DatabaseContext=databaseContext;
		}

		public Food GetByID(int id)
		{
			return DatabaseContext.Foods.Find(id);
		}

		public IList<Food> GetAll()
		{
			return DatabaseContext.Foods.ToList();
		}

		public Food Add(Food food)
		{
			food.ID=0;
			DatabaseContext.Add(food);
			DatabaseContext.SaveChanges();
			return food;
		}

		public Food Update(Food food)
		{
			DatabaseContext.Update(food);
			DatabaseContext.SaveChanges();
			return food;
		}

		public void Delete(Food food)
		{
			DatabaseContext.Remove(food);
			DatabaseContext.SaveChanges();
		}
	}
}
