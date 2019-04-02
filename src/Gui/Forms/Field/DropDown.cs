// Copyright 2019 Richard Nusser
// Licensed under GPLv3 (see http://www.gnu.org/licenses/)

using System;
using System.Collections.Generic;

using Bulkr.Gui.Utils;

namespace Bulkr.Gui.Forms.Field
{
	/// <summary>
	///   Field class for entering enums in a Gtk ComboBox.
	/// </summary>
	public class DropDown<MODEL, TYPE> : Field<MODEL> where MODEL : class
	{
		/// <summary>
		///   The label for <c>null</c> values in dropdown fields.
		/// </summary>
		public static readonly string NULL_LABEL="Please select...";


		/// <summary>
		///   The mapping between enum values to displayed dropdown options.
		///   <para>
		///     The "Please select..." option will be added automatically, don't include it here.
		///   </para>
		/// </summary>
		protected IDictionary<TYPE,string> Map { get; set; }


		/// <summary>
		///   Constructor for enum &lt;-&gt; ComboBox mappings.
		/// </summary>
		/// <param name="propertyName">The model property name.</param>
		/// <param name="widget">The Gtk ComboBox.</param>
		/// <param name="map">The enum to ComboBox label mapping, without any "Please select..." entries.</param>
		public DropDown(string propertyName,Gtk.ComboBox widget,IDictionary<TYPE,string> map) : base(propertyName,widget)
		{
			Map=map;
			FillComboBox();
		}


		/// <summary>
		///   Populates the mapped Gtk ComboBox with all entries the user should see.
		///   <para>
		///     This method won't set the active entry.
		///   </para>
		/// </summary>
		protected void FillComboBox()
		{
			Gtk.ComboBox comboBox=(Gtk.ComboBox)Widget;
			Gtk.ListStore model=new Gtk.ListStore(typeof(string));

			model.AppendValues(NULL_LABEL);

			foreach(KeyValuePair<TYPE,string> entry in Map)
				model.AppendValues(entry.Value);

			comboBox.Model=model;
		}

		/// <summary>
		///   Performs validation for enum dropdowns.
		/// </summary>
		/// <returns>The error message, if any.</returns>
		protected override string PerformValidation()
		{
			string input=((Gtk.ComboBox)Widget).ActiveText;

			if(input==NULL_LABEL)
			{
				if(!IsNullable())
					return "cannot be empty";
				return null;
			}

			foreach(KeyValuePair<TYPE,string> entry in Map)
			{
				if(entry.Value==input)
				{
					ParsedValue=entry.Key;
					return null;
				}
			}

			return "invalid selection"; //this shouldn't happen
		}

		/// <summary>
		///   Changes the ComboBox's current selection to the model's mapped property value.
		/// </summary>
		/// <param name="model">The model to read from.</param>
		public override void PopulateFrom(MODEL model)
		{
			object value=GetModelValue(model);
			if(value!=null&&!(value is TYPE))
				throw new Exception("wrong data type"); //TODO: improve
			((Gtk.ComboBox)Widget).SelectLabel(value!=null ? Map[(TYPE)value] : NULL_LABEL);
		}
	}
}
