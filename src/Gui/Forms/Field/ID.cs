// Copyright 2019 Richard Nusser
// Licensed under GPLv3 (see http://www.gnu.org/licenses/)

using System;

namespace Bulkr.Gui.Forms.Field
{
	/// <summary>
	///   Field class for (readonly) ID labels.
	/// </summary>
	public class ID : Field
	{
		/// <summary>
		///   Creates a new ID label instance.
		/// </summary>
		/// <param name="propertyName">The ID property's name.</param>
		/// <param name="widget">The ID label widget.</param>
		public ID(string propertyName,Gtk.Label widget) : base(propertyName,widget)
		{
		}

		/// <summary>
		///   Does nothing.
		/// </summary>
		/// <param name="model">ignored</param>
		public override void ParseInto(object model)
		{
		}

		/// <summary>
		///   Displays the model's ID.
		/// </summary>
		/// <param name="model">The model.</param>
		public override void PopulateFrom(object model)
		{
			object value=GetModelValue(model);
			if(value!=null&&!(value is int))
				throw new Exception("wrong data type"); //TODO: improve
			int id=value!=null ? (int)value : 0;
			((Gtk.Label)Widget).Text=id>0 ? id.ToString() : "";
		}
	}
}
