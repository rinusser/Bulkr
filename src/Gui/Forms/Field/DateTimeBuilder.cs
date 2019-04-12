// Copyright 2019 Richard Nusser
// Licensed under GPLv3 (see http://www.gnu.org/licenses/)

using Bulkr.Core.Models;

namespace Bulkr.Gui.Forms.Field
{
	/// <summary>
	///   Builder for DateTime fields.
	/// </summary>
	public class DateTimeBuilder<MODEL> : AbstractFieldBuilder<DateTimeBuilder<MODEL>,MODEL,Gtk.Calendar> where MODEL : Model, new()
	{
		/// <summary>
		///   Basic constructor.
		/// </summary>
		/// <param name="formBuilder">The form builder to use.</param>
		/// <param name="name">The field name.</param>
		public DateTimeBuilder(FormBuilder<MODEL> formBuilder,string name) : base(formBuilder,name)
		{
		}


		/// <summary>
		///   Instantiates the field.
		/// </summary>
		/// <returns>The field.</returns>
		public override Field<MODEL> BuildField()
		{
			return new DateTime<MODEL>(PropertyName,
				GetWidget<Gtk.Calendar>(Widget,WidgetName+"_date"),
				GetWidget<Gtk.SpinButton>(Widget,WidgetName+"_hour"),
				GetWidget<Gtk.SpinButton>(Widget,WidgetName+"_minute"));
		}
	}
}
