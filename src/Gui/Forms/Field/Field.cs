// Copyright 2019 Richard Nusser
// Licensed under GPLv3 (see http://www.gnu.org/licenses/)

using System;

namespace Bulkr.Gui.Forms.Field
{
	abstract public class Field
	{
		public string PropertyName { get; }
		public Gtk.Widget Widget { get; }


		protected Field(string propertyName,Gtk.Widget widget)
		{
			PropertyName=propertyName;
			Widget=widget;
		}


		abstract public void PopulateFrom(object model);

		abstract public void ParseInto(object model);


		protected object GetModelValue(object model)
		{
			return model.GetType().GetProperty(PropertyName).GetValue(model);
		}

		protected bool IsNullableIn(object model)
		{
			return Nullable.GetUnderlyingType(model.GetType().GetProperty(PropertyName).PropertyType)!=null;
		}
	}
}
