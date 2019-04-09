// Copyright 2019 Richard Nusser
// Licensed under GPLv3 (see http://www.gnu.org/licenses/)

using Bulkr.Gui.Utils;

namespace Bulkr.Gui.Components
{
	/// <summary>
	///   Base class for UI components.
	/// </summary>
	abstract public class Component
	{
		/// <summary>Container for all references the component needs.</summary>
		protected ComponentContext Context { get; }

		/// <summary>The application window related widgets are in.</summary>
		protected ApplicationWindow Window { get; }


		/// <summary>
		///   Base constructor. Subclasses classes generally should invoke this.
		/// </summary>
		/// <param name="context">The component context to use.</param>
		protected Component(ComponentContext context)
		{
			Context=context;
			Window=context.Window;
		}


		/// <summary>
		///   Implement this: when called, the component should assume other components changed their data and any
		///   referenced data needs to be reloaded.
		/// </summary>
		abstract public void Reload();
	}
}