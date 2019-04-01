// Copyright 2019 Richard Nusser
// Licensed under GPLv3 (see http://www.gnu.org/licenses/)

using System.Collections.Generic;
using Bulkr.Core;
using Bulkr.Core.Models;
using Bulkr.Core.Services;
using Bulkr.Gui.Forms;
using Bulkr.Gui.Forms.Field;
using Bulkr.Gui.Utils;

namespace Bulkr.Gui.Components
{
	/// <summary>
	///   UI component for managing food items.
	/// </summary>
	public class FoodComponent : Component<Food>
	{
		/// <summary>
		///   Basic constructor.
		/// </summary>
		/// <param name="window">The application window inputs widgets are in.</param>
		public FoodComponent(ApplicationWindow window) : base(window)
		{
		}

		/// <summary>
		///   Creates a new FoodService instance. Called automatically by parent constructor.
		/// </summary>
		/// <returns>The FoodService instance.</returns>
		protected override Service<Food> CreateService()
		{
			return FoodService.CreatePersistentInstance();
		}

		/// <summary>
		///   Creates the Form object for food items. Called automatically by parent constructor.
		/// </summary>
		/// <returns>The Form instance with all mapped fields added.</returns>
		protected override Form<Food> CreateForm()
		{
			var referenceSizes=new Dictionary<ReferenceSizeType,string>
			{
				{ ReferenceSizeType._100g,"100 g" },
				{ ReferenceSizeType._100ml,"100 ml" },
				{ ReferenceSizeType._1pc,"1 piece" }
			};

			return new Form<Food>()
				.AddField(new ID("ID",GetFieldValueLabel("ID")))
				.AddField(new Text("Name",GetFieldValueInput("Name"),Text.Option.Required))
				.AddField(new Text("Brand",GetFieldValueInput("Brand")))
				.AddField(new DropDown<ReferenceSizeType>("ReferenceSize",GetFieldValueComboBox("ReferenceSize"),referenceSizes))
				.AddField(new Number("Energy",GetFieldValueInput("Energy")))
				.AddField(new Number("TotalFat",GetFieldValueInput("TotalFat")))
				.AddField(new Number("SaturatedFat",GetFieldValueInput("SaturatedFat")))
				.AddField(new Number("TotalCarbohydrates",GetFieldValueInput("Carbohydrates")))
				.AddField(new Number("Sugar",GetFieldValueInput("Sugar")))
				.AddField(new Number("Protein",GetFieldValueInput("Protein")))
				.AddField(new Number("Fiber",GetFieldValueInput("Fiber")));
		}
	}
}