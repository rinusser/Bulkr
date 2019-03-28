// Copyright 2019 Richard Nusser
// Licensed under GPLv3 (see http://www.gnu.org/licenses/)

using System;

namespace Bulkr.Gui.Forms.Field
{
	public class Number : Field
	{
		public Number(string propertyName,Gtk.Entry widget) : base(propertyName,widget)
		{
		}

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

		public override void PopulateFrom(object model)
		{
			object value=GetModelValue(model);
			if(value!=null&&!(value is float))
				throw new Exception("wrong data type"); //TODO: improve
			((Gtk.Entry)Widget).Text=value!=null ? string.Format("{0:0.##}",value) : "";
		}
	}
}