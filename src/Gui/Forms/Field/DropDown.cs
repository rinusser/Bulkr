// Copyright 2019 Richard Nusser
// Licensed under GPLv3 (see http://www.gnu.org/licenses/)

using System;
using System.Collections.Generic;

namespace Bulkr.Gui.Forms.Field
{
	public class DropDown<TYPE> : Field
	{
		public static readonly string NULL_LABEL="Please select...";


		public static void ForEach(Gtk.ComboBox widget,Func<GLib.Value,Gtk.TreeIter,bool> func)
		{
			Gtk.TreeIter iter;
			widget.Model.GetIterFirst(out iter);
			do
			{
				GLib.Value current=new GLib.Value();
				widget.Model.GetValue(iter,0,ref current);
				if(!func(current,iter))
					return;
			}
			while(widget.Model.IterNext(ref iter));
		}


		public static void SelectLabelIn(Gtk.ComboBox widget,string value)
		{
			bool found=false;
			ForEach(widget,(entry,iter) =>
			{
				if((string)entry.Val!=value)
					return true;
				widget.SetActiveIter(iter);
				found=true;
				return false;
			});

			if(!found)
				throw new ArgumentException(string.Format("value {0} not found in ComboBox {1}",value,widget.Name));
		}


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
			SelectLabel(value!=null ? Map[(TYPE)value] : NULL_LABEL);
		}

		protected void SelectLabel(string value)
		{
			SelectLabelIn((Gtk.ComboBox)Widget,value);
		}
	}
}
