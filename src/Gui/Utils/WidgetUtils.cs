// Copyright 2019 Richard Nusser
// Licensed under GPLv3 (see http://www.gnu.org/licenses/)

using System.Reflection;

namespace Bulkr.Gui.Utils
{
	public static class WidgetUtils
	{
		public static Gtk.Widget GetWidgetFieldByName(object target,string name)
		{
			return (Gtk.Widget)target.GetType().GetField(name,BindingFlags.Instance|BindingFlags.Public|BindingFlags.NonPublic).GetValue(target);
		}
	}
}
