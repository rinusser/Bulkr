// Copyright 2019 Richard Nusser
// Licensed under GPLv3 (see http://www.gnu.org/licenses/)

using System.Data;
using FluentMigrator;
using Microsoft.EntityFrameworkCore;

namespace Bulkr.Core.Persistence.Migrations
{
	/// <summary>
	///   Database migration adding "Intakes" table.
	/// </summary>
	[Migration(201904060210)]
	public class AddIntakes : Migration
	{
		/// <summary>
		///   Performs the migration.
		/// </summary>
		public override void Up()
		{
			Create.Table("Intakes")
				.WithColumn("ID").AsInt32().PrimaryKey().Identity()
				.WithColumn("When").AsString().NotNullable()
				.WithColumn("Amount").AsFloat().NotNullable()
				.WithColumn("FoodID").AsInt32().NotNullable();

			Create.Index("IX_Intakes_FoodID").OnTable("Intakes")
				.OnColumn("FoodID");

			Create.ForeignKey("FK_Intakes_Foods_FoodID")
				.FromTable("Intakes").ForeignColumn("FoodID")
				.ToTable("Foods").PrimaryColumn("ID")
				.OnDelete(Rule.None);
		}

		/// <summary>
		///   Reverts the migration.
		/// </summary>
		public override void Down()
		{
			Delete.Table("Intakes");
		}
	}
}
