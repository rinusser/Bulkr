// Copyright 2019 Richard Nusser
// Licensed under GPLv3 (see http://www.gnu.org/licenses/)

using System;

namespace Bulkr.Gui.Forms.Field
{
	public class ID : Field
	{
		public ID(string propertyName,Gtk.Label widget) : base(propertyName,widget)
		{
		}

		public override void ParseInto(object model)
		{
		}

		public override void PopulateFrom(object model)
		{
			object value=GetModelValue(model);
			if(!(value is int))
				throw new Exception("wrong data type"); //TODO: improve
			int id=(int)value;
			((Gtk.Label)Widget).Text=id>0 ? id.ToString() : "";
		}
	}
}
