// Copyright 2019 Richard Nusser
// Licensed under GPLv3 (see http://www.gnu.org/licenses/)

using System;
using System.Collections.Generic;
using System.Reflection;

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
	abstract public class Field<MODEL> where MODEL : class
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
		///   The list of validation errors.
		/// </summary>
		public IList<ValidationError> ValidationErrors { get; }

		/// <summary>
		///   Cache for values parsed during validation.
		/// </summary>
		protected object ParsedValue { get; set; }


		/// <summary>
		///   Base constructor, handles basics for 1-to-1 mappings.
		/// </summary>
		/// <param name="propertyName">The model property name.</param>
		/// <param name="widget">The Gtk widget.</param>
		protected Field(string propertyName,Gtk.Widget widget)
		{
			PropertyName=propertyName;
			Widget=widget;
			ValidationErrors=new List<ValidationError>();
			ParsedValue=null;
		}


		/// <summary>
		///   Fills the mapped Gtk widget with data from the model.
		/// </summary>
		/// <param name="model">The model instance to take data from.</param>
		abstract public void PopulateFrom(MODEL model);


		/// <summary>
		///   Implement this to perform input validation.
		/// </summary>
		/// <remarks>
		///   This gets called by <see cref="Validate()"/>, which takes care of a few preparations for you.
		/// </remarks>
		/// <remarks>
		///   If you return a string here it'll get turned into a <see cref="ValidationError"/> for your field.
		///   If you need more complex error messages (like when having multiple input widgets, or needing multiple
		///   error messages) your implementation will need to add errors itself.
		///   Return <c>null</c> if the caller shouldn't add a valiation error, e.g. if validation succeeded.
		/// </remarks>
		/// <returns>An error message, or <c>null</c>.</returns>
		abstract protected string PerformValidation();

		/// <summary>
		///   Validates mapped input widgets' contents.
		/// </summary>
		/// <returns>The list of validation errors, empty if input is valid.</returns>
		public IList<ValidationError> Validate()
		{
			ValidationErrors.Clear();
			ParsedValue=null;
			string errorMessage=PerformValidation();
			if(errorMessage!=null)
				ValidationErrors.Add(new ValidationError(PropertyName,errorMessage));
			return ValidationErrors;
		}

		/// <summary>
		///   Takes the Entry widget's contents and puts them into the mapped model property.
		/// </summary>
		/// <param name="model">The model to update.</param>
		public virtual void WriteIntoModel(MODEL model)
		{
			GetModelProperty().SetValue(model,ParsedValue,null);
		}


		/// <summary>
		///   Looks up the reflection information on the mapped model property.
		/// </summary>
		///   <remarks>
		///     This will only handle mappings with 1 model property, as referenced in <see cref="PropertyName"/>.
		///   </remarks>
		/// <returns>The model property's reflection info.</returns>
		protected PropertyInfo GetModelProperty()
		{
			return typeof(MODEL).GetProperty(PropertyName);
		}

		/// <summary>
		///   Fetches the mapped property's value from a model instance.
		///   <para>
		///     Only handles mappings with 1 model property.
		///   </para>
		/// </summary>
		/// <param name="model">The model instance, may be be <c>null</c>.</param>
		/// <returns>The property value, or <c>null</c> if <paramref name="model"/> is null.</returns>
		protected object GetModelValue(MODEL model)
		{
			return model!=null ? GetModelProperty().GetValue(model) : null;
		}

		/// <summary>
		///   Checks if mapped model property is nullable.
		///   <para>
		///     Only handles mappings with 1 model property.
		///   </para>
		/// </summary>
		/// <returns><c>true</c> if property is nullable, <c>false</c> otherwise.</returns>
		protected bool IsNullable()
		{
			return Nullable.GetUnderlyingType(GetModelProperty().PropertyType)!=null;
		}
	}
}
