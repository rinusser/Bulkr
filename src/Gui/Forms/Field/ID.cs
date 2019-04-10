// Copyright 2019 Richard Nusser
// Licensed under GPLv3 (see http://www.gnu.org/licenses/)

namespace Bulkr.Gui.Forms.Field
{
	/// <summary>
	///   Field class for (readonly) ID labels.
	/// </summary>
	public class ID<MODEL> : AbstractField<MODEL,int?> where MODEL : class
	{
		/// <summary>
		///   Creates a new ID label instance.
		/// </summary>
		/// <param name="propertyName">The ID property's name.</param>
		/// <param name="widget">The GTK label widget to display IDs in.</param>
		public ID(string propertyName,Gtk.Label widget) : base(propertyName,widget,null)
		{
		}

		/// <summary>
		///   Does nothing, always succeeds.
		/// </summary>
		/// <returns><c>null</c>.</returns>
		protected override string PerformValidation()
		{
			return null;
		}

		/// <summary>
		///   Does nothing.
		/// </summary>
		/// <param name="model">Ignored.</param>
		public override void WriteIntoModel(MODEL model)
		{
		}

		/// <summary>
		///   Displays the model's ID.
		/// </summary>
		/// <param name="model">The model.</param>
		protected override void PerformPopulateFrom(MODEL model)
		{
			int? value=GetModelValue<int?>(model);
			int id=value!=null ? (int)value : 0;
			((Gtk.Label)Widget).Text=id>0 ? id.ToString() : "";
		}
	}
}
