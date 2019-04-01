// Copyright 2019 Richard Nusser
// Licensed under GPLv3 (see http://www.gnu.org/licenses/)

using System;

namespace Bulkr.Gui.Forms.Field
{
	/// <summary>
	///   Base class for form fields.
	///   <para>
	///     Each field instance maps a Gtk widget to a model property bidirectionally. Arbitrary mappings can be created,
	///     to e.g. input date and time in separate widgets.
	///   </para>
	///   <para>
	///     There's currently no generic type for e.g. a model class. This means that, when required, the model info needs
	///     to be passed in an argument. As a benefit though, field instances could be reused for multiple models.
	///   </para>
	/// </summary>
	abstract public class Field
	{
		/// <summary>
		///   The name of the model property to store values in.
		/// </summary>
		public string PropertyName { get; }

		/// <summary>
		///   The Gtk widget to enter data into.
		/// </summary>
		public Gtk.Widget Widget { get; }


		/// <summary>
		///   Base constructor, handles basics for 1-to-1 mappings.
		/// </summary>
		/// <param name="propertyName">The model property name.</param>
		/// <param name="widget">The Gtk widget.</param>
		protected Field(string propertyName,Gtk.Widget widget)
		{
			PropertyName=propertyName;
			Widget=widget;
		}


		/// <summary>
		///   Fills the mapped Gtk widget with data from the model.
		/// </summary>
		/// <param name="model">The model instance to take data from.</param>
		abstract public void PopulateFrom(object model);

		/// <summary>
		///   Takes data from the mapped Gtk widget and adds it to the model.
		/// </summary>
		/// <param name="model">The model instance to put data into.</param>
		abstract public void ParseInto(object model);


		/// <summary>
		///   Fetches the mapped property's value from a model instance.
		///   <para>
		///     Only handles mappings with 1 model property.
		///   </para>
		/// </summary>
		/// <param name="model">The model instance, may be be <c>null</c>.</param>
		/// <returns>The property value, or <c>null</c> if <paramref name="model"/> is null.</returns>
		protected object GetModelValue(object model)
		{
			return model?.GetType().GetProperty(PropertyName).GetValue(model);
		}

		/// <summary>
		///   Checks if mapped model property is nullable.
		///   <para>
		///     Only handles mappings with 1 model property.
		///   </para>
		/// </summary>
		/// <param name="model">The model instance to check.</param>
		/// <returns><c>true</c> if property is nullable, <c>false</c> otherwise.</returns>
		protected bool IsNullableIn(object model)
		{
			return Nullable.GetUnderlyingType(model.GetType().GetProperty(PropertyName).PropertyType)!=null;
		}
	}
}
