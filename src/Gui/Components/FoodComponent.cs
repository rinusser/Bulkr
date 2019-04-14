// Copyright 2019 Richard Nusser
// Licensed under GPLv3 (see http://www.gnu.org/licenses/)

using Bulkr.Core;
using Bulkr.Core.Models;
using Bulkr.Core.Services;
using Bulkr.Gui.Forms;

namespace Bulkr.Gui.Components
{
	/// <summary>
	///   UI component for managing food items.
	/// </summary>
	public class FoodComponent : CRUDComponent<Food>
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
		protected override DatabaseCRUDService<Food> CreateService()
		{
			return FoodService.Create();
		}

		/// <summary>
		///   Creates the Form object for food items. Called automatically by parent constructor.
		/// </summary>
		/// <returns>The Form instance with all mapped fields added.</returns>
		protected override Form<Food> CreateForm()
		{
			return new FormBuilder<Food>()
				.InComponent(this)
				.AddIDField("ID")
				.AddTextField("Name")
					.Required()
				.AddTextField("Brand")
				.AddDropDownField<ReferenceSizeType,ReferenceSizeType>("ReferenceSize")
					.WithLabelMapper(GetReferenceSizeTypeLabel)
					.Required()
				.AddNumberField("Energy")
				.AddNumberField("TotalFat")
				.AddNumberField("SaturatedFat")
				.AddNumberField("TotalCarbohydrates")
				.AddNumberField("Sugar")
				.AddNumberField("Protein")
				.AddNumberField("Salt")
				.AddNumberField("Fiber")
				.AddTextField("Comments")
				.Build();
		}

		/// <summary>
		///   Returns the label to display for a given reference size.
		/// </summary>
		/// <param name="size">The reference size.</param>
		/// <returns>The label to display.</returns>
		public static string GetReferenceSizeTypeLabel(ReferenceSizeType size)
		{
			switch(size)
			{
				case ReferenceSizeType._100g:
					return "100 g";
				case ReferenceSizeType._100ml:
					return "100 ml";
				case ReferenceSizeType._1pc:
					return "1 piece";
			}
			throw new System.NotImplementedException("unhandled reference size");
		}
	}
}