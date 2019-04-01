// Copyright 2019 Richard Nusser
// Licensed under GPLv3 (see http://www.gnu.org/licenses/)

using Bulkr.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Bulkr.Core.Persistence
{
	/// <summary>
	///   Application's database context for Entity Framework.
	/// </summary>
	public class BulkrContext : DbContext
	{
		/// <summary>
		///   Persistance instance, singleton pattern.
		/// </summary>
		private static BulkrContext PersistentInstance { get; set; }


		/// <summary>
		///   Creates an instance with in-memory storage, e.g. for testing.
		/// </summary>
		/// <param name="name">An identifier for the instance, use this to e.g. create separate contexts for tests.</param>
		/// <returns>The instance.</returns>
		public static BulkrContext CreateInMemoryInstance(string name)
		{
			var options=new DbContextOptionsBuilder<BulkrContext>().UseInMemoryDatabase(databaseName:name).Options;
			return new BulkrContext(options);
		}

		/// <summary>
		///   Creates an instance with persistent storage.
		///   <para>
		///     This is currently hard-coded to use a SQLite backend.
		///   </para>
		/// </summary>
		/// <returns>The instance.</returns>
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


		/// <summary>
		///   Entity Framework repository for foods.
		/// </summary>
		public DbSet<Food> Foods { get; set; }


		/// <summary>
		///   Basic constructor.
		/// </summary>
		/// <param name="options">Options for Entity Framework.</param>
		private BulkrContext(DbContextOptions<BulkrContext> options) : base(options)
		{
		}
	}
}
