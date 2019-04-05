// Copyright 2019 Richard Nusser
// Licensed under GPLv3 (see http://www.gnu.org/licenses/)

using System.Collections.Generic;

namespace Bulkr.Gui.Forms.Field
{
	/// <summary>
	///   Field interface for type safety in <see cref="Form.Fields"/>.
	/// </summary>
	public interface Field<MODEL>
	{
		/// <summary>
		///   Fills the mapped Gtk widget with data from the model.
		/// </summary>
		/// <param name="model">The model instance to take data from.</param>
		void PopulateFrom(MODEL model);

		/// <summary>
		///   See <see cref="AbstractField{MODEL,PROPERTY}.Validate()"/>.
		/// </summary>
		/// <returns>See <see cref="AbstractField{MODEL, PROPERTY}.Validate()"/>.</returns>
		IList<ValidationError> Validate();

		/// <summary>
		///   See <see cref="AbstractField{MODEL,PROPERTY}.Validate()"/>.
		/// </summary>
		/// <param name="model">See <see cref="AbstractField{MODEL,PROPERTY}.WriteIntoModel(MODEL)"/>.</param>
		void WriteIntoModel(MODEL model);

		/// <summary>
		///   See <see cref="AbstractField{MODEL,PROPERTY}.Reload()"/>.
		/// </summary>
		void Reload();
	}

}
