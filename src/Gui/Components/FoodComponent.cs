// Copyright 2019 Richard Nusser
// Licensed under GPLv3 (see http://www.gnu.org/licenses/)

using Bulkr.Core;
using Bulkr.Core.Models;
using Bulkr.Core.Services;
using Bulkr.Gui.Forms;
using Bulkr.Gui.Forms.Field;

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
			return new Form<Food>()
				.AddField(new ID<Food>("ID",GetFieldValueWidget<Gtk.Label>("ID")))
				.AddField(new Text<Food>("Name",GetFieldValueWidget<Gtk.Entry>("Name"),GetFieldLabelWidget("Name"),Option.Required))
				.AddField(new Text<Food>("Brand",GetFieldValueWidget<Gtk.Entry>("Brand"),GetFieldLabelWidget("Brand")))
				.AddField(new DropDown<Food,ReferenceSizeType,ReferenceSizeType>("ReferenceSize",
					GetFieldValueWidget<Gtk.ComboBox>("ReferenceSize"),
					GetFieldLabelWidget("ReferenceSize"),
					new EnumService<ReferenceSizeType,ReferenceSizeType>(),
					i => i,                     //ID mapper
					GetReferenceSizeTypeLabel,  //label mapper
					Option.Required))
				.AddField(new Number<Food>("Energy",GetFieldValueWidget<Gtk.Entry>("Energy"),GetFieldLabelWidget("Energy")))
				.AddField(new Number<Food>("TotalFat",GetFieldValueWidget<Gtk.Entry>("TotalFat"),GetFieldLabelWidget("TotalFat")))
				.AddField(new Number<Food>("SaturatedFat",GetFieldValueWidget<Gtk.Entry>("SaturatedFat"),GetFieldLabelWidget("SaturatedFat")))
				.AddField(new Number<Food>("TotalCarbohydrates",GetFieldValueWidget<Gtk.Entry>("Carbohydrates"),GetFieldLabelWidget("Carbohydrates")))
				.AddField(new Number<Food>("Sugar",GetFieldValueWidget<Gtk.Entry>("Sugar"),GetFieldLabelWidget("Sugar")))
				.AddField(new Number<Food>("Protein",GetFieldValueWidget<Gtk.Entry>("Protein"),GetFieldLabelWidget("Protein")))
				.AddField(new Number<Food>("Fiber",GetFieldValueWidget<Gtk.Entry>("Fiber"),GetFieldLabelWidget("Fiber")));
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