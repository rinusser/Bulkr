// Copyright 2019 Richard Nusser
// Licensed under GPLv3 (see http://www.gnu.org/licenses/)

using Bulkr.Core.Models;

namespace Bulkr.Gui.Forms.Field
{
	/// <summary>
	///   Builder for ID fields.
	/// </summary>
	public class IDBuilder<MODEL> : AbstractFieldBuilder<IDBuilder<MODEL>,MODEL,Gtk.Label> where MODEL : Model, new()
	{
		/// <summary>
		///   Basic constructor.
		/// </summary>
		/// <param name="formBuilder">The form builder to use.</param>
		/// <param name="name">The field name.</param>
		public IDBuilder(FormBuilder<MODEL> formBuilder,string name) : base(formBuilder,name)
		{
		}


		/// <summary>
		///   Instantiates the field.
		/// </summary>
		/// <returns>The field.</returns>
		public override Field<MODEL> BuildField()
		{
			return new ID<MODEL>(PropertyName,GetWidget());
		}
	}
}
