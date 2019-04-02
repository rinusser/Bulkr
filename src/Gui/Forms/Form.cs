// Copyright 2019 Richard Nusser
// Licensed under GPLv3 (see http://www.gnu.org/licenses/)

using System.Collections.Generic;

namespace Bulkr.Gui.Forms
{
	/// <summary>
	///   Bidirectional mappings between input widgets and item model fields.
	///   <para>
	///     Each mapping is contained in one of the <see cref="T:Bulkr.Gui.Forms.Field"/> classes.
	///   </para>
	/// </summary>
	public class Form<MODEL> where MODEL : class, new()
	{
		/// <summary>
		///   The list of mapped fields.
		/// </summary>
		protected IList<Field.Field<MODEL>> Fields { get; }


		/// <summary>
		///   Basic constructor.
		/// </summary>
		public Form()
		{
			Fields=new List<Field.Field<MODEL>>();
		}


		/// <summary>
		///   Adds a field mapping.
		/// </summary>
		/// <param name="field">Field.</param>
		/// <returns>The field.</returns>
		public Form<MODEL> AddField(Field.Field<MODEL> field)
		{
			Fields.Add(field);
			return this;
		}

		/// <summary>
		///   Fills input widgets with the item model's values.
		/// </summary>
		/// <param name="model">The model to display.</param>
		public void Populate(MODEL model)
		{
			foreach(var field in Fields)
				field.PopulateFrom(model);
		}

		/// <summary>
		///   Validates mapped input widgets' contents.
		/// </summary>
		/// <returns>The list of validation errors, empty if input is valid.</returns>
		public IList<ValidationError> Validate()
		{
			var errors=new List<ValidationError>();
			foreach(var field in Fields)
				errors.AddRange(field.Validate());
			return errors;
		}

		/// <summary>
		///   Copies input widgets' contents into a new model instance.
		///   <para>
		///     The returned instance might be in conflict with an already tracked instance in Entity Framework,
		///     you'll have to take special care if you're planning on e.g. saving the item.
		///   </para>
		///   <para>
		///     You can use <see cref="ToModel(MODEL)"/> to reuse an existing (and tracked) model instance.
		///   </para>
		/// </summary>
		/// <returns>The entered data turned into a model instance.</returns>
		public MODEL ToModel()
		{
			return ToModel(new MODEL());
		}

		/// <summary>
		///   Copies input widgets' contents into an existing model instance.
		/// </summary>
		/// <param name="model">The passed model instance, with input values set.</param>
		/// <returns>The same model.</returns>
		public MODEL ToModel(MODEL model)
		{
			foreach(var field in Fields)
				field.WriteIntoModel(model);
			return model;
		}
	}
}
