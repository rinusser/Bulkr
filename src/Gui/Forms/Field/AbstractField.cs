// Copyright 2019 Richard Nusser
// Licensed under GPLv3 (see http://www.gnu.org/licenses/)

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Bulkr.Gui.Forms.Field
{
	/// <summary>
	///   Base class for form fields.
	///   <para>
	///     Each field instance maps a Gtk widget to a model property bidirectionally. Arbitrary mappings can be created,
	///     to e.g. input date and time in separate widgets.
	///   </para>
	/// </summary>
	abstract public class AbstractField<MODEL, PROPERTY> : Field<MODEL> where MODEL : class
	{
		/// <summary>
		///   The name of the model property to store values in.
		/// </summary>
		public string PropertyName { get; }

		/// <summary>
		///   The GTK widget to enter data into.
		/// </summary>
		public Gtk.Widget Widget { get; }

		/// <summary>
		///   The GTK label for this field.
		/// </summary>
		public Gtk.Label Label { get; }

		/// <summary>
		///   The last validity style set: <c>true</c> if there were no errors, <c>false</c> otherwise.
		/// </summary>
		protected bool LastValidityStyleSet { get; set; }

		/// <summary>
		///   The list of validation errors.
		/// </summary>
		public IList<ValidationError> ValidationErrors { get; }

		/// <summary>
		///   Cache for values parsed during validation.
		/// </summary>
		protected PROPERTY ParsedValue { get; set; }

		/// <summary>
		///   This field's active options.
		/// </summary>
		protected IList<Option> Options { get; set; }

		/// <summary>
		///   The regular background color for the input widget.
		/// </summary>
		protected Gdk.Color RegularInputBackgroundColor { get; set; }

		/// <summary>
		///   The regular base color for the input widget.
		/// </summary>
		protected Gdk.Color RegularInputBaseColor { get; set; }

		/// <summary>
		///   The regular foreground color for the label widget.
		/// </summary>
		protected Gdk.Color RegularLabelForegroundColor { get; set; }

		/// <summary>
		///   The error background color for the input widget.
		/// </summary>
		protected Gdk.Color ErrorInputBackgroundColor { get; set; } = new Gdk.Color(255,128,128);

		/// <summary>
		///   The error base color for the input widget.
		/// </summary>
		protected Gdk.Color ErrorInputBaseColor { get; set; } = new Gdk.Color(255,192,192);

		/// <summary>
		///   The error foreground color for the label widget.
		/// </summary>
		protected Gdk.Color ErrorLabelForegroundColor { get; set; } = new Gdk.Color(255,0,0);


		/// <summary>
		///   Base constructor, handles basics for 1-to-1 mappings.
		/// </summary>
		/// <param name="propertyName">The model property name.</param>
		/// <param name="widget">The GTK input widget.</param>
		/// <param name="labelWidget">The GTK label widget for this field.</param>
		/// <param name="options">Any options for this field.</param>
		protected AbstractField(string propertyName,Gtk.Widget widget,Gtk.Label labelWidget,Option[] options = null)
		{
			PropertyName=propertyName;
			Widget=widget;
			Label=labelWidget;
			ValidationErrors=new List<ValidationError>();
			ParsedValue=default(PROPERTY);
			Options=options!=null ? options.ToList() : new List<Option>();

			RegularInputBackgroundColor=widget.Style.Background(Gtk.StateType.Normal);
			RegularInputBaseColor=widget.Style.Base(Gtk.StateType.Normal);
			if(labelWidget!=null)
				RegularLabelForegroundColor=labelWidget.Style.Foreground(Gtk.StateType.Normal);
		}


		/// <summary>
		///   Implement this: this method should take data from the passed model instance and put it into input widgets.
		/// </summary>
		/// <remarks>
		///   This gets called by <see cref="PopulateFrom(MODEL)"/> which resets the widget's styling for you.
		/// </remarks>
		/// <param name="model">The model instance to take data from.</param>
		abstract protected void PerformPopulateFrom(MODEL model);

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
		///   Fills the mapped GTK widget with data from the model.
		/// </summary>
		/// <param name="model">The model instance to take data from.</param>
		public void PopulateFrom(MODEL model)
		{
			StyleWidgetWithValidity(true);
			PerformPopulateFrom(model);
		}

		/// <summary>
		///   Styles the input and label widgets with the given validity state.
		/// </summary>
		/// <param name="valid"><c>true</c> if input is valid, <c>false</c>otherwise.</param>
		protected virtual void StyleWidgetWithValidity(bool valid = true)
		{
			Widget.ModifyBg(Gtk.StateType.Normal,valid ? RegularInputBackgroundColor : ErrorInputBackgroundColor);
			Widget.ModifyBase(Gtk.StateType.Normal,valid ? RegularInputBaseColor : ErrorInputBaseColor);

			UpdateTooltip(Widget,valid);
			UpdateTooltip(Label,valid);

			Label?.ModifyFg(Gtk.StateType.Normal,valid ? RegularLabelForegroundColor : ErrorLabelForegroundColor);

			LastValidityStyleSet=valid;
		}

		/// <summary>
		///   Sets the target widget's tooltip, or clears it, depending on whether there are any input errors.
		/// </summary>
		/// <param name="target">The input/label widget to set a tooltip for.</param>
		/// <param name="valid">Whether the input is valid.</param>
		protected virtual void UpdateTooltip(Gtk.Widget target,bool valid)
		{
			if(target==null)
				return;
			target.HasTooltip=!valid;
			if(!valid)
				target.TooltipMarkup="<span foreground='red'>"+string.Join("\n",ValidationErrors.Select(e => e.Reason))+"</span>";
		}

		/// <summary>
		///   Validates mapped input widgets' contents.
		/// </summary>
		/// <returns>The list of validation errors, empty if input is valid.</returns>
		public IList<ValidationError> Validate()
		{
			ValidationErrors.Clear();
			ParsedValue=default(PROPERTY);
			string errorMessage=PerformValidation();
			if(errorMessage!=null)
				ValidationErrors.Add(new ValidationError(PropertyName,errorMessage));

			StyleWidgetWithValidity(ValidationErrors.Count<1);

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
		/// <remarks>
		///   Careful when calling this with a primitive type or struct, it'll return that type's default.
		/// </remarks>
		/// <param name="model">The model instance, may be be <c>null</c>.</param>
		/// <returns>The property value, or the type's default if <paramref name="model"/> is null.</returns>
		protected T GetModelValue<T>(MODEL model)
		{
			if(model==null)
				return default(T);
			var value=GetModelProperty().GetValue(model);
			return value!=null ? (T)value : default(T);
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

		/// <summary>
		///   Checks if the <see cref="Option.Required"/> option was set.
		/// </summary>
		/// <returns><c>true</c> if it was.</returns>
		protected bool IsRequired()
		{
			return Options.Contains(Option.Required);
		}

		/// <summary>
		///   Reloads whatever data this field is referencing. Override this method if there's something to reload.
		/// </summary>
		public virtual void Reload()
		{
		}

		/// <summary>
		///   Returns this field's <see cref="PropertyName"/>.
		/// </summary>
		/// <returns>The name.</returns>
		public string GetName()
		{
			return PropertyName;
		}

		/// <summary>
		///   Gets the last validity style set.
		/// </summary>
		/// <returns><c>true</c>, if last styling was for valid data, <c>false</c> if data was invalid.</returns>
		public bool GetLastValidityStyleSet()
		{
			return LastValidityStyleSet;
		}
	}
}