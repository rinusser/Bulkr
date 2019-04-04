// Copyright 2019 Richard Nusser
// Licensed under GPLv3 (see http://www.gnu.org/licenses/)

using Bulkr.Gui.Utils;

namespace Bulkr.Gui.Components
{
	/// <summary>
	///   Container class for all references a <see cref="T:Bulkr.Gui.Components.Component"/> instance might need.
	///   Extend this class if your component needs additional data.
	/// </summary>
	public class ComponentContext
	{
		/// <summary>
		///   The application window form widgets are in.
		/// </summary>
		public ApplicationWindow Window { get; set; }
	}
}
