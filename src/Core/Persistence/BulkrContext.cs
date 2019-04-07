// Copyright 2019 Richard Nusser
// Licensed under GPLv3 (see http://www.gnu.org/licenses/)

using System.Configuration;
using System;
using Microsoft.EntityFrameworkCore;

using Bulkr.Core.Models;

namespace Bulkr.Core.Persistence
{
	/// <summary>
	///   Application's database context for Entity Framework.
	/// </summary>
	public class BulkrContext : DbContext
	{
		/// <summary>
		///   Persistent instance, singleton pattern.
		/// </summary>
		private static BulkrContext PersistentInstance { get; set; }


		/// <summary>
		///   Creates an instance with in-memory storage, e.g. for testing.
		/// </summary>
		/// <param name="name">An identifier for the instance, use this to e.g. create separate contexts for tests.</param>
		/// <returns>The instance.</returns>
		private static BulkrContext CreateInMemoryInstance(string name)
		{
			var options=new DbContextOptionsBuilder<BulkrContext>().UseInMemoryDatabase(databaseName:name).Options;
			return new BulkrContext(options);
		}

		/// <summary>
		///   Returns an instance with SQLite storage. Uses singleton pattern: only one instance will be created.
		/// </summary>
		/// <param name="name">The database filename, e.g. "bulkr.sqlite".</param>
		/// <returns>The instance.</returns>
		private static BulkrContext GetSQLiteInstance(string name)
		{
			if(PersistentInstance==null)
			{
				var connectionString=string.Format("Data Source={0}",name);
				SQLitePCL.raw.SetProvider(new SQLitePCL.SQLite3Provider_e_sqlite3());

				MigrationRunner.Run(connectionString);

				var options=new DbContextOptionsBuilder<BulkrContext>().UseSqlite(connectionString).Options;
				PersistentInstance=new BulkrContext(options);

				//PersistentInstance.Database.EnsureCreated(); //run this in an empty schema to compare against migrations
			}
			return PersistentInstance;
		}


		/// <summary>
		///   Returns a <see cref="BulkrContext"/> instance.
		/// </summary>
		/// <remarks>
		///   Takes adapter and database name information from the application configuration, e.g. the target project's
		///   <c>app.config</c> file.
		/// </remarks>
		/// <remarks>
		///   Currently only SQLite (DbAdapter="sqlite") and in-memory (DbAdapter="inmemory") adapters are handled.
		/// </remarks>
		/// <returns>The instance.</returns>
		/// <param name="name">Optional: a database name with adapter-specific meaning, overriding the DbName setting in app.config.</param>
		public static BulkrContext GetInstance(string name = null)
		{
			string databaseAdapter=ConfigurationManager.AppSettings["DbAdapter"];
			string databaseName=ConfigurationManager.AppSettings["DbName"];

			if(name!=null)
				databaseName=name.Trim();

			if(string.IsNullOrWhiteSpace(databaseAdapter))
				throw new Exception("need to configure database adapter in app.config");

			if(string.IsNullOrWhiteSpace(databaseName))
				throw new Exception("need to configure database name in app.config or pass it in argument");

			switch(databaseAdapter.ToLower().Trim())
			{
				case "sqlite":
					return GetSQLiteInstance(databaseName);
				case "inmemory":
					return CreateInMemoryInstance(databaseName);
				default:
					throw new NotImplementedException(string.Format("unhandled database adapter '{0}'",databaseAdapter));
			}
		}


		/// <summary>
		///   Entity Framework repository for foods.
		/// </summary>
		public DbSet<Food> Foods { get; set; }

		/// <summary>
		///   Entity Framework repository for intake entries.
		/// </summary>
		public DbSet<Intake> Intakes { get; set; }


		/// <summary>
		///   Basic constructor.
		/// </summary>
		/// <param name="options">Options for Entity Framework.</param>
		private BulkrContext(DbContextOptions<BulkrContext> options) : base(options)
		{
		}
	}
}
