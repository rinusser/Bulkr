// Copyright 2019 Richard Nusser
// Licensed under GPLv3 (see http://www.gnu.org/licenses/)

using Bulkr.Core.Models;
using Bulkr.Core.Services;
using Bulkr.Gui.Forms;
using Bulkr.Gui.Forms.Field;

namespace Bulkr.Gui.Components
{
	/// <summary>
	///   UI component for managing intake entries.
	/// </summary>
	public class IntakeComponent : CRUDComponent<Intake>
	{
		/// <summary>
		///   Basic constructor.
		/// </summary>
		/// <param name="context">The context for this component.</param>
		public IntakeComponent(IntakeComponentContext context) : base(context)
		{
		}


		/// <summary>
		///   Creates a new IntakeService instance. Called automatically by parent constructor.
		/// </summary>
		/// <returns>The IntakeService instance.</returns>
		protected override DatabaseCRUDService<Intake> CreateService()
		{
			return IntakeService.Create();
		}

		/// <summary>
		///   Creates the Form object for intake entries. Called automatically by parent constructor.
		/// </summary>
		/// <returns>The Form instance with all mapped fields added.</returns>
		protected override Form<Intake> CreateForm()
		{
			var foodService=((IntakeComponentContext)Context).FoodService;

			return new Form<Intake>()
				.AddField(new ID<Intake>("ID",GetFieldValueWidget<Gtk.Label>("ID")))
				.AddField(new DateTime<Intake>("When",
					GetFieldValueWidget<Gtk.Calendar>("When_Date"),
					GetFieldValueWidget<Gtk.SpinButton>("When_Hour"),
					GetFieldValueWidget<Gtk.SpinButton>("When_Minute")))
				.AddField(new Number<Intake>("Amount",GetFieldValueWidget<Gtk.Entry>("Amount"),GetFieldLabelWidget("Amount")))
				.AddField(new DropDown<Intake,Food,int>("Food",
					GetFieldValueWidget<Gtk.ComboBox>("Food"),
					GetFieldLabelWidget("Food"),
					foodService,
					i => i.ID,
					GetFoodDisplayString,
					Option.Required));
		}

		/// <summary>
		///   Gets the dropdown label for a given food item.
		/// </summary>
		/// <param name="item">The item to pretty-print.</param>
		/// <returns>The label to display.</returns>
		public static string GetFoodDisplayString(Food item)
		{
			return string.Format(item.Brand!=null ? "{0} ({1})" : "{0}",item.Name,item.Brand);
		}
	}
}