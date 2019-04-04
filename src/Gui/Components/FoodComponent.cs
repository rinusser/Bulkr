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
		/// <param name="context">The context for this component.</param>
		public FoodComponent(ComponentContext context) : base(context)
		{
		}

		/// <summary>
		///   Creates a new FoodService instance. Called automatically by parent constructor.
		/// </summary>
		/// <returns>The FoodService instance.</returns>
		protected override Service<Food> CreateService()
		{
			return FoodService.Create();
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
				.AddField(new ID<Food>("ID",GetFieldValueLabel("ID")))
				.AddField(new Text<Food>("Name",GetFieldValueInput("Name"),Option.Required))
				.AddField(new Text<Food>("Brand",GetFieldValueInput("Brand")))
				.AddField(new DropDown<Food,ReferenceSizeType>("ReferenceSize",GetFieldValueComboBox("ReferenceSize"),referenceSizes))
				.AddField(new Number<Food>("Energy",GetFieldValueInput("Energy")))
				.AddField(new Number<Food>("TotalFat",GetFieldValueInput("TotalFat")))
				.AddField(new Number<Food>("SaturatedFat",GetFieldValueInput("SaturatedFat")))
				.AddField(new Number<Food>("TotalCarbohydrates",GetFieldValueInput("Carbohydrates")))
				.AddField(new Number<Food>("Sugar",GetFieldValueInput("Sugar")))
				.AddField(new Number<Food>("Protein",GetFieldValueInput("Protein")))
				.AddField(new Number<Food>("Fiber",GetFieldValueInput("Fiber")));
		}
	}
}