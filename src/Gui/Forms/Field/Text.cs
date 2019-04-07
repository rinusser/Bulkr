// Copyright 2019 Richard Nusser
// Licensed under GPLv3 (see http://www.gnu.org/licenses/)

using System;
using System.Collections.Generic;

namespace Bulkr.Gui.Forms.Field
{
	/// <summary>
	///   Field class for string input.
	///   <para>
	///     C# doesn't differentiate between "string" and "nullable string" types, so if you want a string field
	///     to be required you'll need to pass the <see cref="Option.Required"/> option in the constructor.
	///   </para>
	/// </summary>
	public class Text<MODEL> : AbstractField<MODEL,string> where MODEL : class
	{
		/// <summary>
		///   Constructor for string fields.
		/// </summary>
		/// <param name="propertyName">The model property name.</param>
		/// <param name="widget">The Entry widget.</param>
		/// <param name="options">Mapping options.</param>
		public Text(string propertyName,Gtk.Entry widget,params Option[] options) : base(propertyName,widget,options)
		{
		}


		/// <summary>
		///   Performs validation for string inputs.
		/// </summary>
		/// <returns>The error message, if any.</returns>
		protected override string PerformValidation()
		{
			string parsed=((Gtk.Entry)Widget).Text.Trim();

			if(parsed.Length<1)
			{
				if(Options.Contains(Option.Required))
					return "cannot be empty";
				parsed=null;
			}

			ParsedValue=parsed;
			return null;
		}

		/// <summary>
		///   Puts the model's mapped property value into the Entry widget.
		/// </summary>
		/// <param name="model">The model to read from.</param>
		public override void PopulateFrom(MODEL model)
		{
			string value=GetModelValue<string>(model);
			((Gtk.Entry)Widget).Text=value??"";
		}
	}
}
