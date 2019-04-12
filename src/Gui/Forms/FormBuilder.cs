// Copyright 2019 Richard Nusser
// Licensed under GPLv3 (see http://www.gnu.org/licenses/)

using System.Collections.Generic;

using Bulkr.Core.Models;
using Bulkr.Gui.Forms.Field;

namespace Bulkr.Gui.Forms
{
	/// <summary>
	///   Builder for form objects.
	/// </summary>
	/// <remarks>
	///   There's no need to use this, form/field objects can be instantiated manually. This mechanism just makes
	///   the form/field creation easier to read and adds sensible defaults.
	/// </remarks>
	public class FormBuilder<MODEL> : FieldBuilderFactory<MODEL> where MODEL : Model, new()
	{
		/// <summary>
		///   The list of added field builders for the form.
		/// </summary>
		protected IList<FieldBuilder<MODEL>> FieldBuilders { get; }


		/// <summary>
		///   Basic constructor.
		/// </summary>
		public FormBuilder() : base(null)
		{
			FormBuilder=this;
			FieldBuilders=new List<FieldBuilder<MODEL>>();
		}


		/// <summary>
		///   Build the form and all requested fields.
		/// </summary>
		/// <returns>The form.</returns>
		/// <exception cref="System.ArgumentException">If there was an issue building the fields.</exception>
		public override Form<MODEL> Build()
		{
			var form=new Form<MODEL>();
			foreach(var fieldBuilders in FieldBuilders)
				form.AddField(fieldBuilders.BuildField());
			return form;
		}

		/// <summary>
		///   Adds a field builder instance and registers the currently targeted component in it, if any.
		/// </summary>
		/// <param name="builder">The field builder.</param>
		/// <returns>The same builder instance.</returns>
		/// <typeparam name="T">The field builder type, for a fluent interface.</typeparam>
		public T AddBuilder<T>(T builder) where T : FieldBuilder<MODEL>
		{
			builder.InComponent(CRUDComponent);
			FieldBuilders.Add(builder);
			return builder;
		}
	}
}
