// Copyright 2019 Richard Nusser
// Licensed under GPLv3 (see http://www.gnu.org/licenses/)

namespace Bulkr.Gui.Utils
{
	/// <summary>
	///   Interface for windows with a logger widget.
	/// </summary>
	public interface WidgetLogger
	{
		/// <summary>
		///   Implementations are expected to log the given message.
		/// </summary>
		/// <param name="text">The log message to add.</param>
		void AddLogEntry(string text);
	}
}
