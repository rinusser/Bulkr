// Copyright 2019 Richard Nusser
// Licensed under GPLv3 (see http://www.gnu.org/licenses/)

using Bulkr.Core.Models;
using Bulkr.Gui.Components;

namespace Bulkr.Gui.Forms.Field
{
	/// <summary>
	///   Required methods for form field builders.
	/// </summary>
	public interface FieldBuilder<MODEL> where MODEL : Model, new()
	{
		/// <summary>
		///   Create the field instance.
		/// </summary>
		/// <returns>The field instance.</returns>
		Field<MODEL> BuildField();

		/// <summary>
		///   Registers a CRUD component to look up default widgets in.
		/// </summary>
		/// <param name="component">The component to take widgets from.</param>
		/// <returns>The builder, fluent interface.</returns>
		FieldBuilder<MODEL> InComponent(CRUDComponent<MODEL> component);
	}
}
