// Copyright 2019 Richard Nusser
// Licensed under GPLv3 (see http://www.gnu.org/licenses/)

using System;
using Microsoft.Extensions.DependencyInjection;
using FluentMigrator.Runner;

using Bulkr.Core.Persistence.Migrations;

namespace Bulkr.Core.Persistence
{
	/// <summary>
	///   Runner for FluentMigrator-based database migrations.
	/// </summary>
	public static class MigrationRunner
	{
		/// <summary>
		///   Entry point for running database migrations.
		/// </summary>
		/// <remarks>
		///   Only SQLite migrations are implemented.
		/// </remarks>
		/// <param name="connectionString">The connection string to use.</param>
		public static void Run(string connectionString)
		{
			var serviceProvider=CreateServices(connectionString);

			using(var scope = serviceProvider.CreateScope())
			{
				UpdateDatabase(scope.ServiceProvider);
			}
		}

		/// <summary>
		///   Sets up FluentMigrator.
		/// </summary>
		/// <param name="connectionString">The connection string to use.</param>
		/// <returns>The DI provider.</returns>
		private static IServiceProvider CreateServices(string connectionString)
		{
			return new ServiceCollection()
				.AddFluentMigratorCore()
				.ConfigureRunner(rb => rb
					.AddSQLite()
					.WithGlobalConnectionString(connectionString)
					.ScanIn(typeof(AddFoods).Assembly).For.Migrations())
				.AddLogging(lb => lb.AddFluentMigratorConsole())
				.BuildServiceProvider(false);
		}

		/// <summary>
		///   Upgrades database schema.
		/// </summary>
		/// <param name="serviceProvider">The DI provider.</param>
		private static void UpdateDatabase(IServiceProvider serviceProvider)
		{
			serviceProvider.GetRequiredService<IMigrationRunner>().MigrateUp();
		}
	}
}
