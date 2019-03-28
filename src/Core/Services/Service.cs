// Copyright 2019 Richard Nusser
// Licensed under GPLv3 (see http://www.gnu.org/licenses/)

using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Bulkr.Core.Models;

namespace Bulkr.Core.Services
{
	public abstract class Service<MODEL> where MODEL : Model, new()
	{
		public DbContext DatabaseContext { get; }
		public DbSet<MODEL> DbSet;


		protected Service(DbContext databaseContext,DbSet<MODEL> dbSet)
		{
			DatabaseContext=databaseContext;
			DbSet=dbSet;
		}


		public MODEL GetByID(int id)
		{
			return DbSet.Find(id);
		}

		public IList<MODEL> GetAll()
		{
			return DbSet.ToList();
		}

		public MODEL Add(MODEL item)
		{
			item.ID=0;
			DatabaseContext.Add(item);
			DatabaseContext.SaveChanges();
			return item;
		}

		public MODEL Update(MODEL item)
		{
			DatabaseContext.Update(item);
			DatabaseContext.SaveChanges();
			return item;
		}

		public void Delete(MODEL item)
		{
			DatabaseContext.Remove(item);
			DatabaseContext.SaveChanges();
		}
	}
}
