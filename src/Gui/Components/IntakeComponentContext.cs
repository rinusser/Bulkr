// Copyright 2019 Richard Nusser
// Licensed under GPLv3 (see http://www.gnu.org/licenses/)

using Bulkr.Core.Services;

namespace Bulkr.Gui.Components
{
	/// <summary>
	///   Container class for IntakeComponent's constructor.
	/// </summary>
	/// <remarks>
	///   This class is used to make sure all required references are available in the virtual function call from
	///   the Component base constructor.
	/// </remarks>
	public class IntakeComponentContext : ComponentContext
	{
		/// <summary>
		///   The FoodService to populate the "Food" dropdown from.
		/// </summary>
		public FoodService FoodService { get; set; }
	}
}

