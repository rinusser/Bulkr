// Copyright 2019 Richard Nusser
// Licensed under GPLv3 (see http://www.gnu.org/licenses/)

using System;
using System.Collections.Generic;
using System.Linq;

using Bulkr.Core.Models;
using Bulkr.Gui.Components;

namespace Bulkr.Gui.Forms.Field
{
	/// <summary>
	///   Base class for field builders.
	/// </summary>
	abstract public class AbstractFieldBuilder<BUILDER, MODEL, WIDGET> : FieldBuilderFactory<MODEL>, FieldBuilder<MODEL> where BUILDER : AbstractFieldBuilder<BUILDER,MODEL,WIDGET> where MODEL : Model, new() where WIDGET : Gtk.Widget
	{
		/// <summary>
		///   The model's property name for the field.
		/// </summary>
		public string PropertyName { get; set; }


		/// <summary>
		///   The input widget's name to use if no <see cref="Widget"/> was set.
		/// </summary>
		public string WidgetName { get; set; }

		/// <summary>
		///   The input widget to use.
		/// </summary>
		public Gtk.Widget Widget { get; set; }


		/// <summary>
		///   The label widget's name to use if no <see cref="Label"/> was set.
		/// </summary>
		public string LabelName { get; set; }

		/// <summary>
		///   The label widget to use.
		/// </summary>
		public Gtk.Label Label { get; set; }


		/// <summary>
		///   The field options to use.
		/// </summary>
		protected IList<Option> Options { get; set; }


		/// <summary>
		///   Basic constructor.
		/// </summary>
		/// <param name="formBuilder">The form builder to use.</param>
		/// <param name="name">The field name.</param>
		protected AbstractFieldBuilder(FormBuilder<MODEL> formBuilder,string name) : base(formBuilder)
		{
			PropertyName=name;
			WidgetName=name;
			LabelName=name;

			Options=new List<Option>();
		}


		/// <summary>
		///   Sets the CRUD component to take default widgets from.
		/// </summary>
		/// <param name="component">The component.</param>
		/// <returns><c>this</c></returns>
		public new FieldBuilder<MODEL> InComponent(CRUDComponent<MODEL> component)
		{
			CRUDComponent=component;
			return (BUILDER)this;
		}

		/// <summary>
		///   Explicitly marks the field as required.
		/// </summary>
		/// <returns><c>this</c></returns>
		public BUILDER Required()
		{
			Options.Add(Option.Required);
			return (BUILDER)this;
		}


		/// <summary>
		///   Instantiates the field.
		/// </summary>
		/// <returns>The field.</returns>
		abstract public Field<MODEL> BuildField();


		/// <summary>
		///   Gets the default widget via <see cref="Widget"/> or <see cref="WidgetName"/>. Override if needed.
		/// </summary>
		/// <returns>The widget.</returns>
		/// <exception cref="ArgumentException">If widget could not be determined.</exception>
		protected virtual WIDGET GetWidget()
		{
			return GetWidget<WIDGET>(Widget,WidgetName);
		}

		/// <summary>
		///   Looks up a widget by reference or name: if reference is set, return it, if not, find widget by name.
		/// </summary>
		/// <param name="widget">The widget reference.</param>
		/// <param name="widgetName">The widget name.</param>
		/// <returns>The widget.</returns>
		/// <typeparam name="T">The widget type.</typeparam>
		/// <exception cref="ArgumentException">If widget could not be determined.</exception>
		protected T GetWidget<T>(Gtk.Widget widget,string widgetName) where T : Gtk.Widget
		{
			if(widget!=null)
				return (T)widget;
			if(CRUDComponent!=null)
				return CRUDComponent.GetFieldValueWidget<T>(widgetName);
			throw Missing("widget");
		}

		/// <summary>
		///   Gets the default label widget via <see cref="Label"/> or <see cref="LabelName"/>.
		/// </summary>
		/// <returns>The label widget, or <c>null</c> if it couldn't be determined.</returns>
		protected virtual Gtk.Label GetLabel()
		{
			if(Label!=null)
				return Label;
			if(CRUDComponent!=null&&LabelName!=null)
				return CRUDComponent.GetFieldLabelWidget(LabelName);
			return null;
		}

		/// <summary>
		///   Returns the options to use for the field.
		/// </summary>
		/// <returns>The options.</returns>
		protected virtual Option[] GetOptions()
		{
			return Options.ToArray();
		}


		/// <summary>
		///   Creates an <see cref="ArgumentException"/> instance. Used to throw consistent errors in builders.
		/// </summary>
		/// <param name="description">A short description of what's missing.</param>
		/// <returns>The exception to throw.</returns>
		protected Exception Missing(string description)
		{
			return new ArgumentException(string.Format("{0}: there's no way to determine {1}, can't build field.",PropertyName,description));
		}
	}
}
