// Copyright 2019 Richard Nusser
// Licensed under GPLv3 (see http://www.gnu.org/licenses/)

using System;
using System.Collections.Generic;

using Bulkr.Gui.Utils;

namespace Bulkr.Gui.Forms.Field
{
	public class DropDown<TYPE> : Field
	{
		/// <summary>
		///   The label for <c>null</c> values in dropdown fields.
		/// </summary>
		public static readonly string NULL_LABEL="Please select...";


		protected IDictionary<TYPE,string> Map { get; set; }


		public DropDown(string propertyName,Gtk.ComboBox widget,IDictionary<TYPE,string> map) : base(propertyName,widget)
		{
			Map=map;
			FillComboBox();
		}


		protected void FillComboBox()
		{
			Gtk.ComboBox comboBox=(Gtk.ComboBox)Widget;
			Gtk.ListStore model=new Gtk.ListStore(typeof(string));

			model.AppendValues(NULL_LABEL);

			foreach(KeyValuePair<TYPE,string> entry in Map)
				model.AppendValues(entry.Value);

			comboBox.Model=model;
		}

		public override void ParseInto(object model)
		{
			var property=model.GetType().GetProperty(PropertyName);
			string input=((Gtk.ComboBox)Widget).ActiveText;

			if(input==NULL_LABEL)
			{
				if(!IsNullableIn(model))
					throw new InputException(PropertyName,"cannot be empty");
				property.SetValue(model,null,null);
				return;
			}

			foreach(KeyValuePair<TYPE,string> entry in Map)
			{
				if(entry.Value==input)
				{
					property.SetValue(model,entry.Key,null);
					return;
				}
			}

			throw new InputException(PropertyName,"invalid selection"); //this shouldn't happen
		}

		public override void PopulateFrom(object model)
		{
			object value=GetModelValue(model);
			if(value!=null&&!(value is TYPE))
				throw new Exception("wrong data type"); //TODO: improve
			((Gtk.ComboBox)Widget).SelectLabel(value!=null ? Map[(TYPE)value] : NULL_LABEL);
		}
	}
}
