// Copyright 2019 Richard Nusser
// Licensed under GPLv3 (see http://www.gnu.org/licenses/)

using System;

using Bulkr.Core.Services;
using Bulkr.Gui.Utils;

namespace Bulkr.Gui.Forms.Field
{
	/// <summary>
	///   Field class for selecting <see cref="CRUDService"/> entries in a ComboBox.
	/// </summary>
	/// <remarks>
	///   The GTK ComboBox's internal storage is set up with 3 columns:
	///     1. the ID, wrapped in a GLib.Value to allow for both `enum` and `int` (or other primitives) types
	///     2. the model
	///     3. the label to display
	/// </remarks>
	public class DropDown<MODEL, TYPE, ID> : AbstractField<MODEL,TYPE> where MODEL : class where ID : struct
	{
		/// <summary>
		///   The label for <c>null</c> values in dropdown fields.
		/// </summary>
		public static readonly string NULL_LABEL="Please select...";


		/// <summary>
		///   The service to read items from.
		/// </summary>
		protected CRUDService<TYPE,ID> Service { get; }

		/// <summary>
		///   Mapper function to determine item IDs.
		/// </summary>
		protected Func<TYPE,ID> IdMapper { get; }

		/// <summary>
		///   Mapper function to determine item labels.
		/// </summary>
		protected Func<TYPE,string> LabelMapper { get; }


		/// <summary>
		///   Constructor for CRUD service &lt; - &gt; ComboBox mappings.
		/// </summary>
		/// <param name="propertyName">The model property name.</param>
		/// <param name="widget">The Gtk ComboBox.</param>
		/// <param name="service">The service to load data from.</param>
		/// <param name="idMapper">Mapper function turning items into IDs for internal tracking.</param>
		/// <param name="labelMapper">Mapper function turning items into text labels for the dropdown.</param>
		/// <param name="options">Any options for this field (currently <see cref="Option.Required"/> only.</param>
		public DropDown(string propertyName,Gtk.ComboBox widget,CRUDService<TYPE,ID> service,Func<TYPE,ID> idMapper,Func<TYPE,string> labelMapper,params Option[] options) : base(propertyName,widget,options)
		{
			Service=service;
			IdMapper=idMapper;
			LabelMapper=labelMapper;
			Reload();

			widget.AddAttribute(widget.Cells[0],"text",2);  //tell ComboBox to render list store's 3rd column, i.e. the label
		}


		/// <summary>
		///   Populates the mapped Gtk ComboBox with all entries the user should see.
		/// </summary>
		/// <remarks>
		///   This method retains the current selection, if possible.
		/// </remarks>
		public override void Reload()
		{
			var comboBox=(Gtk.ComboBox)Widget;
			var previousID=comboBox.GetActiveID<ID>();

			bool wrapID=typeof(ID).IsPrimitive;
			Type idType=wrapID?typeof(GLib.Value):typeof(ID?);
			Gtk.ListStore model=new Gtk.ListStore(idType,typeof(MODEL),typeof(string));

			model.AppendValues(null,null,NULL_LABEL);
			foreach(var entry in Service.GetAll())
			{
				ID rawID=IdMapper(entry);
				var id=wrapID ? (object)new GLib.Value(rawID) : rawID;
				model.AppendValues(id,entry,LabelMapper(entry));
			}

			comboBox.Model=model;

			comboBox.SelectID<ID>(null);  //XXX if this causes additional flickering, make it conditional
			comboBox.SelectID(previousID,true);
		}

		/// <summary>
		///   Performs validation for enum dropdowns.
		/// </summary>
		/// <returns>The error message, if any.</returns>
		protected override string PerformValidation()
		{
			object selection=((Gtk.ComboBox)Widget).GetActiveModel();

			if(selection==null)
			{
				if(!IsNullable())
					return "cannot be empty";
				return null;
			}

			ParsedValue=(TYPE)selection;
			return null;  //anything found in the list store must be valid
		}

		/// <summary>
		///   Changes the ComboBox's current selection to the model's mapped property value.
		/// </summary>
		/// <param name="model">The model to read from.</param>
		public override void PopulateFrom(MODEL model)
		{
			object value=GetModelValue<object>(model);  //can't use generic TYPE parameter: this needs to be nullable, unconditionally
			((Gtk.ComboBox)Widget).SelectID(value!=null ? (ID?)IdMapper((TYPE)value) : null);
		}

		/// <summary>
		///   Custom nullable check for referenced items, since object references in C# are always nullable.
		/// </summary>
		/// <returns><c>true</c> if item can be <c>null</c>, <c>false</c> otherwise.</returns>
		protected override bool IsNullable()
		{
			return !Options.Contains(Option.Required);
		}
	}
}
