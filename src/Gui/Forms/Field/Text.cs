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
	public class Text : Field
	{
		/// <summary>
		///   Possible field options.
		/// </summary>
		public enum Option
		{
			Required,
		}


		/// <summary>
		///   This field's active options.
		/// </summary>
		private IList<Option> Options { get; set; }


		/// <summary>
		///   Constructor for string fields.
		/// </summary>
		/// <param name="propertyName">The model property name.</param>
		/// <param name="widget">The Entry widget.</param>
		/// <param name="options">Mapping options.</param>
		public Text(string propertyName,Gtk.Entry widget,params Option[] options) : base(propertyName,widget)
		{
			Options=options;
		}


		/// <summary>
		///   Takes the Entry widget's contents and puts them into the mapped model property.
		/// </summary>
		/// <param name="model">The model to update.</param>
		/// <exception cref="InputException">If the widget's contents are invalid.</exception>
		public override void ParseInto(object model)
		{
			string input=((Gtk.Entry)Widget).Text.Trim();
			var property=model.GetType().GetProperty(PropertyName);
			if(input.Length<1)
			{
				if(Options.Contains(Option.Required))
					throw new InputException(PropertyName,"cannot be empty");
				input=null;
			}
			property.SetValue(model,input,null);
		}

		/// <summary>
		///   Puts the model's mapped property value into the Entry widget.
		/// </summary>
		/// <param name="model">The model to read from.</param>
		public override void PopulateFrom(object model)
		{
			object value=GetModelValue(model);
			if(value!=null&&!(value is string))
				throw new Exception("wrong data type"); //TODO: improve
			((Gtk.Entry)Widget).Text=value!=null ? (string)value : "";
		}
	}
}
