// Copyright 2019 Richard Nusser
// Licensed under GPLv3 (see http://www.gnu.org/licenses/)

using System.Reflection;

namespace Bulkr.Gui.Utils
{
	/// <summary>
	///   Utility functions related to Gtk widgets.
	/// </summary>
	public static class ApplicationWindowExtensions
	{
		/// <summary>
		///   Finds a widget in the window by its field name.
		/// </summary>
		/// <param name="target">The widget container to look in.</param>
		/// <param name="name">The widget's name.</param>
		/// <returns>The widget field.</returns>
		public static Gtk.Widget GetWidget(this ApplicationWindow target,string name)
		{
			return (Gtk.Widget)target.GetType().GetField(name,BindingFlags.Instance|BindingFlags.Public|BindingFlags.NonPublic).GetValue(target);
		}
	}
}
