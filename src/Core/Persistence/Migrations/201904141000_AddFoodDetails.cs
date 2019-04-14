// Copyright 2019 Richard Nusser
// Licensed under GPLv3 (see http://www.gnu.org/licenses/)

using System.Data;
using FluentMigrator;
using Microsoft.EntityFrameworkCore;

namespace Bulkr.Core.Persistence.Migrations
{
	/// <summary>
	///   Database migration adding "salt" and "comments" columns to Foods table.
	/// </summary>
	[Migration(201904141000)]
	public class AddFoodDetails : Migration
	{
		/// <summary>
		///   Performs the migration.
		/// </summary>
		public override void Up()
		{
			Alter.Table("Foods")
				.AddColumn("Salt").AsFloat().Nullable()
				.AddColumn("Comments").AsString().Nullable();
		}

		/// <summary>
		///   Reverts the migration.
		/// </summary>
		public override void Down()
		{
			Delete
				.Column("Salt")
				.Column("Comments")
				.FromTable("Foods");
		}
	}
}
