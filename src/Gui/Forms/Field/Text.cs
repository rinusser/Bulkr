// Copyright 2019 Richard Nusser
// Licensed under GPLv3 (see http://www.gnu.org/licenses/)

using System;
using System.Collections.Generic;

namespace Bulkr.Gui.Forms.Field
{
	public class Text : Field
	{
		public enum Option
		{
			Required,
		}


		private IList<Option> Options { get; set; }


		public Text(string propertyName,Gtk.Entry widget,params Option[] options) : base(propertyName,widget)
		{
			Options=options;
		}


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

		public override void PopulateFrom(object model)
		{
			object value=GetModelValue(model);
			if(value!=null&&!(value is string))
				throw new Exception("wrong data type"); //TODO: improve
			((Gtk.Entry)Widget).Text=value!=null ? (string)value : "";
		}
	}
}
