// Copyright 2019 Richard Nusser
// Licensed under GPLv3 (see http://www.gnu.org/licenses/)

using System;
using System.Linq;

using Bulkr.Core.Models;
using Bulkr.Core.Services;
using Bulkr.Gui.Utils;

namespace Bulkr.Gui.Forms.Field
{
	/// <summary>
	///   Field class for entering object references.
	/// </summary>
	public class ServiceDropDown<MODEL, TYPE> : DropDown<MODEL,TYPE> where MODEL : class where TYPE : Model, new()
	{
		/// <summary>
		///   The service to read items from.
		/// </summary>
		protected Service<TYPE> Service { get; }

		/// <summary>
		///   Mapper function to determine item labels.
		/// </summary>
		protected Func<TYPE,string> Mapper { get; }


		/// <summary>
		///   Constructor for database table &lt; - &gt; ComboBox mappings.
		/// </summary>
		/// <param name="propertyName">The model property name.</param>
		/// <param name="widget">The Gtk ComboBox.</param>
		/// <param name="service">The service to load data from.</param>
		/// <param name="mapper">Mapper function turning items into text labels for the dropdown.</param>
		/// <param name="options">Any options for this field (currently <see cref="Option.Required"/> only.</param>
		public ServiceDropDown(string propertyName,Gtk.ComboBox widget,Service<TYPE> service,Func<TYPE,string> mapper,params Option[] options) : base(propertyName,widget,null,options)
		{
			Service=service;
			Mapper=mapper;
			Reload();
		}


		/// <summary>
		///   Reloads item list from service.
		/// </summary>
		public override void Reload()
		{
			var comboBox=(Gtk.ComboBox)Widget;
			var previousLabel=comboBox.ActiveText;  //XXX go by model ID instead?
			Map.Clear();
			foreach(var entry in Service.GetAll())
				Map.Add(entry,Mapper(entry));
			FillComboBox();
			comboBox.SelectLabel(NULL_LABEL);  //XXX if this causes additional flickering, make it conditional
			comboBox.SelectLabel(previousLabel,true);
		}

		/// <summary>
		///   Finds item label in map.
		/// </summary>
		/// <remarks>
		///   This is required since there might be multiple instances of the same item in use.
		/// </remarks>
		/// <param name="item">The item.</param>
		/// <returns>The item label to display.</returns>
		protected override string LookUpMappedValueFor(TYPE item)
		{
			return Map.First(i => i.Key.ID==item.ID).Value;
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
