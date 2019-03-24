// Copyright 2019 Richard Nusser
// Licensed under GPLv3 (see http://www.gnu.org/licenses/)

using Bulkr.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Bulkr.Core.Persistence
{
	public class BulkrContext : DbContext
	{
		private static BulkrContext PersistentInstance { get; set; }


		public static BulkrContext CreateInMemoryInstance(string name)
		{
			var options=new DbContextOptionsBuilder<BulkrContext>().UseInMemoryDatabase(databaseName:name).Options;
			return new BulkrContext(options);
		}

		public static BulkrContext GetPersistentInstance()
		{
			if(BulkrContext.PersistentInstance==null)
			{
				SQLitePCL.raw.SetProvider(new SQLitePCL.SQLite3Provider_e_sqlite3());
				var options=new DbContextOptionsBuilder<BulkrContext>().UseSqlite("Data Source=bulkr.sqlite").Options;
				PersistentInstance=new BulkrContext(options);

				//TODO BUL-6: make migrations work. Adding MigrationAssembly() in the context builder didn't help, earlier.
				//PersistentInstance.Database.Migrate();

				PersistentInstance.Database.EnsureCreated();
			}
			return PersistentInstance;
		}


		public DbSet<Food> Foods { get; set; }


		private BulkrContext(DbContextOptions<BulkrContext> options) : base(options)
		{
		}
	}
}
