// Copyright 2019 Richard Nusser
// Licensed under GPLv3 (see http://www.gnu.org/licenses/)

using FluentMigrator;

namespace Bulkr.Core.Persistence.Migrations
{
	/// <summary>
	///   Database migration adding "Foods" table.
	/// </summary>
	[Migration(201904052142)]
	public class AddFoods : Migration
	{
		/// <summary>
		///   Performs the migration.
		/// </summary>
		public override void Up()
		{
			Create.Table("Foods")
				.WithColumn("ID").AsInt32().PrimaryKey().Identity()
				.WithColumn("Energy").AsFloat().NotNullable()
				.WithColumn("TotalFat").AsFloat().Nullable()
				.WithColumn("SaturatedFat").AsFloat().Nullable()
				.WithColumn("TotalCarbohydrates").AsFloat().Nullable()
				.WithColumn("Sugar").AsFloat().Nullable()
				.WithColumn("Protein").AsFloat().Nullable()
				.WithColumn("Fiber").AsFloat().Nullable()
				.WithColumn("Name").AsString().Nullable()
				.WithColumn("Brand").AsString().Nullable()
				.WithColumn("ReferenceSize").AsInt32().NotNullable();
		}

		/// <summary>
		///   Reverts the migration.
		/// </summary>
		public override void Down()
		{
			Delete.Table("Foods");
		}
	}
}
