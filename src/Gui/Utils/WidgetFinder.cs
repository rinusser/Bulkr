// Copyright 2019 Richard Nusser
// Licensed under GPLv3 (see http://www.gnu.org/licenses/)

namespace Bulkr.Gui.Utils
{
	/// <summary>
	///   Interface for widget containers supporting widget lookup.
	/// </summary>
	public interface WidgetFinder
	{
		/// <summary>
		///   Implementations are expected to return a widget by its name.
		/// </summary>
		/// <param name="name">The widget's name.</param>
		/// <returns>The widget.</returns>
		Gtk.Widget GetWidget(string name);
	}
}
