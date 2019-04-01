// Copyright 2019 Richard Nusser
// Licensed under GPLv3 (see http://www.gnu.org/licenses/)

using System;

namespace Bulkr.Gui.Forms.Field
{
	/// <summary>
	///   Field mapping for numeric input.
	///   <para>
	///     Only handles <c>float</c> type for now.
	///   </para>
	/// </summary>
	public class Number : Field
	{
		/// <summary>
		///   Constructor for number fields.
		/// </summary>
		/// <param name="propertyName">The model property name.</param>
		/// <param name="widget">The Entry widget.</param>
		public Number(string propertyName,Gtk.Entry widget) : base(propertyName,widget)
		{
		}


		/// <summary>
		///   Takes the Entry widget's contents and puts them into the mapped model property.
		/// </summary>
		/// <param name="model">The model to update.</param>
		/// <exception cref="InputException">If the widget's contents are invalid.</exception>
		public override void ParseInto(object model)
		{
			string input=((Gtk.Entry)Widget).Text.Trim();
			float? parsed;
			var property=model.GetType().GetProperty(PropertyName);
			if(input.Length<1&&IsNullableIn(model))
				parsed=null;
			else
			{
				try
				{
					parsed=float.Parse(input);
				}
				catch(FormatException)
				{
					throw new InputException(PropertyName,"not a valid number");
				}

				if(parsed<0)
					throw new InputException(PropertyName,"cannot be negative");
			}
			property.SetValue(model,parsed,null);
		}

		/// <summary>
		///   Puts the model's mapped property value into the Entry widget.
		/// </summary>
		/// <param name="model">The model to read from.</param>
		public override void PopulateFrom(object model)
		{
			object value=GetModelValue(model);
			if(value!=null&&!(value is float))
				throw new Exception("wrong data type"); //TODO: improve
			((Gtk.Entry)Widget).Text=value!=null ? string.Format("{0:0.##}",value) : "";
		}
	}
}