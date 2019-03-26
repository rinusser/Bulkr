// Copyright 2019 Richard Nusser
// Licensed under GPLv3 (see http://www.gnu.org/licenses/)

using System;
using Bulkr.Core;

namespace Bulkr.Gui.Form.Element
{
	public class ReferenceSize
	{
		public static void SelectAt(Gtk.ComboBox widget,ReferenceSizeType? referenceSize)
		{
			string displayValue=null;
			switch(referenceSize)
			{
				case ReferenceSizeType._100g:
					displayValue="100 g";
					break;
				case ReferenceSizeType._100ml:
					displayValue="100 ml";
					break;
				case ReferenceSizeType._1pc:
					displayValue="1 piece";
					break;
				case null:
					displayValue="Please select...";
					break;
				default:
					throw new NotImplementedException(string.Format("invalid reference size '{0}'",referenceSize));
			}
			SelectComboBoxEntry(widget,displayValue);
		}

		protected static void SelectComboBoxEntry(Gtk.ComboBox widget,string value)
		{
			Gtk.TreeIter iter;
			widget.Model.GetIterFirst(out iter);
			do
			{
				GLib.Value current=new GLib.Value();
				widget.Model.GetValue(iter,0,ref current);
				if((string)current.Val==value)
				{
					widget.SetActiveIter(iter);
					return;
				}
			}
			while(widget.Model.IterNext(ref iter));

			throw new ArgumentException(string.Format("value {0} not found in ComboBox {1}",value,widget.Name));
		}

		public static ReferenceSizeType Parse(string input)
		{
			switch(input)
			{
				case "100 g":
					return ReferenceSizeType._100g;
				case "100 ml":
					return ReferenceSizeType._100ml;
				case "1 piece":
					return ReferenceSizeType._1pc;
				default:
					throw new InputException("reference size","missing or invalid");
			}
		}
	}
}
