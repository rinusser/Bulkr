// Copyright 2019 Richard Nusser
// Licensed under GPLv3 (see http://www.gnu.org/licenses/)

namespace Bulkr.Gui.Utils
{
	/// <summary>
	///   Interface for application windows. Implementations can be used as containers of UI components.
	/// </summary>
	public interface ApplicationWindow
	{
		/// <summary>
		///   Implementations are expected to log the given message.
		/// </summary>
		/// <param name="text">The log message to add.</param>
		void AddLogEntry(string text);
	}
}
