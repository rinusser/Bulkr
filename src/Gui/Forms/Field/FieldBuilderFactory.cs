// Copyright 2019 Richard Nusser
// Licensed under GPLv3 (see http://www.gnu.org/licenses/)

using Bulkr.Core.Models;
using Bulkr.Gui.Components;

namespace Bulkr.Gui.Forms.Field
{
	/// <summary>
	///   Factory base for creating field builders.
	/// </summary>
	/// <remarks>
	///   This class is used as a base for form and field builders. This way, new field builders can be added to the form
	///   without explicitly telling the previous field builder we're done. For example:
	///   <code>
	///     new FormBuilder&lt;SomeModel&gt;()
	///       .AddTextField("SomeText")      //this method is in FormBuilder
	///         .Required()
	///       .AddTextField("AnotherText");  //this method is in TextBuilder
	///   </code>
	/// </remarks>
	public abstract class FieldBuilderFactory<MODEL> where MODEL : Model, new()
	{
		/// <summary>
		///   Reference to the form builder.
		/// </summary>
		protected FormBuilder<MODEL> FormBuilder { get; set; }

		/// <summary>
		///   The CRUD component to look for default widgets in.
		/// </summary>
		protected CRUDComponent<MODEL> CRUDComponent { get; set; }


		/// <summary>
		///   Basic constructor.
		/// </summary>
		/// <param name="formBuilder">The form builder instance.</param>
		protected FieldBuilderFactory(FormBuilder<MODEL> formBuilder)
		{
			FormBuilder=formBuilder;
		}


		/// <summary>
		///   Sets the CRUD component to look for widgets in.
		/// </summary>
		/// <param name="component">The component.</param>
		/// <returns><c>this</c>, fluent interface.</returns>
		public FieldBuilderFactory<MODEL> InComponent(CRUDComponent<MODEL> component)
		{
			CRUDComponent=component;
			return this;
		}

		/// <summary>
		///   Creates a new ID field builder.
		/// </summary>
		/// <param name="name">The field name.</param>
		/// <returns>The field builder.</returns>
		public IDBuilder<MODEL> AddIDField(string name)
		{
			return FormBuilder.AddBuilder(new IDBuilder<MODEL>(FormBuilder,name));
		}

		/// <summary>
		///   Creates a new text field builder.
		/// </summary>
		/// <param name="name">The field name.</param>
		/// <returns>The field builder.</returns>
		public TextBuilder<MODEL> AddTextField(string name)
		{
			return FormBuilder.AddBuilder(new TextBuilder<MODEL>(FormBuilder,name));
		}

		/// <summary>
		///   Creates a new number field builder.
		/// </summary>
		/// <param name="name">The field name.</param>
		/// <returns>The field builder.</returns>
		public NumberBuilder<MODEL> AddNumberField(string name)
		{
			return FormBuilder.AddBuilder(new NumberBuilder<MODEL>(FormBuilder,name));
		}

		/// <summary>
		///   Creates a new drop down field builder.
		/// </summary>
		/// <param name="name">The field name.</param>
		/// <returns>The field builder.</returns>
		public DropDownBuilder<MODEL,TYPE,ID> AddDropDownField<TYPE, ID>(string name) where ID : struct
		{
			return FormBuilder.AddBuilder(new DropDownBuilder<MODEL,TYPE,ID>(FormBuilder,name));
		}

		/// <summary>
		///   Creates a new datetime field builder.
		/// </summary>
		/// <param name="name">The field name.</param>
		/// <returns>The field builder.</returns>
		public DateTimeBuilder<MODEL> AddDateTimeField(string name)
		{
			return FormBuilder.AddBuilder(new DateTimeBuilder<MODEL>(FormBuilder,name));
		}


		/// <summary>
		///   Builds the form.
		/// </summary>
		/// <returns>The form instance.</returns>
		public virtual Form<MODEL> Build()
		{
			return FormBuilder.Build();
		}
	}
}
