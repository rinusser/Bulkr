// Copyright 2019 Richard Nusser
// Licensed under GPLv3 (see http://www.gnu.org/licenses/)

namespace Bulkr.Gui.Utils
{
	public interface WidgetFinder
	{
		Gtk.Widget GetWidget(string name);
	}
}
