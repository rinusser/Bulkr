// Copyright 2019 Richard Nusser
// Licensed under GPLv3 (see http://www.gnu.org/licenses/)

using System.Reflection;

namespace Bulkr.Gui.Utils
{
	/// <summary>
	///   Utility functions related to Gtk widgets.
	/// </summary>
	public static class WidgetUtils
	{
		/// <summary>
		///   Finds a widget by name in a target object, usually a window.
		/// </summary>
		/// <param name="target">The widget container to look in, e.g. an ApplicationWindow instance.</param>
		/// <param name="name">The widget's name.</param>
		/// <returns>The widget field.</returns>
		public static Gtk.Widget GetWidgetFieldByName(object target,string name)
		{
			return (Gtk.Widget)target.GetType().GetField(name,BindingFlags.Instance|BindingFlags.Public|BindingFlags.NonPublic).GetValue(target);
		}
	}
}
