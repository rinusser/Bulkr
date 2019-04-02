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
	public class Number<MODEL> : Field<MODEL> where MODEL : class
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
		///   Handles validation for number inputs.
		/// </summary>
		/// <returns>The error message, if any.</returns>
		protected override string PerformValidation()
		{
			string input=((Gtk.Entry)Widget).Text.Trim();
			var property=GetModelProperty();
			if(input.Length<1)
			{
				if(!IsNullable())
					return "cannot be empty";
				return null;
			}

			try
			{
				var parsed=float.Parse(input);
				if(parsed<0)
					return "cannot be negative";
				ParsedValue=parsed;
			}
			catch(FormatException)
			{
				return "not a valid number";
			}

			return null;
		}

		/// <summary>
		///   Puts the model's mapped property value into the Entry widget.
		/// </summary>
		/// <param name="model">The model to read from.</param>
		public override void PopulateFrom(MODEL model)
		{
			object value=GetModelValue(model);
			if(value!=null&&!(value is float))
				throw new Exception("wrong data type"); //TODO: improve
			((Gtk.Entry)Widget).Text=value!=null ? string.Format("{0:0.##}",value) : "";
		}
	}
}