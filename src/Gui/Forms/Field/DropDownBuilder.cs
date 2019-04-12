// Copyright 2019 Richard Nusser
// Licensed under GPLv3 (see http://www.gnu.org/licenses/)

using System;

using Bulkr.Core.Models;
using Bulkr.Core.Services;

namespace Bulkr.Gui.Forms.Field
{
	/// <summary>
	///   Builder for DropDown fields.
	/// </summary>
	public class DropDownBuilder<MODEL, TYPE, ID> : AbstractFieldBuilder<DropDownBuilder<MODEL,TYPE,ID>,MODEL,Gtk.ComboBox> where MODEL : Model, new() where ID : struct
	{
		/// <summary>
		///   The service to use for the field.
		/// </summary>
		public CRUDService<TYPE,ID> Service { get; set; }

		/// <summary>
		///   The model-&gt;ID mapper function to use.
		/// </summary>
		public Func<TYPE,ID> IDMapper { get; set; }

		/// <summary>
		///   The model-&gt;label mapper function to use.
		/// </summary>
		public Func<TYPE,string> LabelMapper { get; set; }


		/// <summary>
		///   Basic constructor.
		/// </summary>
		/// <param name="formBuilder">The form builder to use.</param>
		/// <param name="name">The field name.</param>
		public DropDownBuilder(FormBuilder<MODEL> formBuilder,string name) : base(formBuilder,name)
		{
		}


		/// <summary>
		///   Sets the service to use.
		/// </summary>
		/// <param name="service">The service.</param>
		/// <returns><c>this</c>.</returns>
		public DropDownBuilder<MODEL,TYPE,ID> WithService(CRUDService<TYPE,ID> service)
		{
			Service=service;
			return this;
		}

		/// <summary>
		///   Sets the ID mapper function to use.
		/// </summary>
		/// <param name="idMapper">The mapper function.</param>
		/// <returns><c>this</c>.</returns>
		public DropDownBuilder<MODEL,TYPE,ID> WithIDMapper(Func<TYPE,ID> idMapper)
		{
			IDMapper=idMapper;
			return this;
		}

		/// <summary>
		///   Sets the label mapper function to use.
		/// </summary>
		/// <param name="labelMapper">The mapper function.</param>
		/// <returns><c>this</c>.</returns>
		public DropDownBuilder<MODEL,TYPE,ID> WithLabelMapper(Func<TYPE,string> labelMapper)
		{
			LabelMapper=labelMapper;
			return this;
		}


		/// <summary>
		///   Instantiates the field.
		/// </summary>
		/// <returns>The field.</returns>
		public override Field<MODEL> BuildField()
		{
			return new DropDown<MODEL,TYPE,ID>(PropertyName,GetWidget(),GetLabel(),GetService(),GetIDMapper(),GetLabelMapper(),GetOptions());
		}


		/// <summary>
		///   Determines the service to use.
		///   If <typeparamref name="TYPE"/> is an enum this will automatically create a suitable fallback service instance.
		/// </summary>
		/// <returns>The service.</returns>
		/// <exception cref="ArgumentException">If no service was registered and there's no fallback.</exception>
		protected CRUDService<TYPE,ID> GetService()
		{
			if(Service!=null)
				return Service;
			if(IsEnum())
				return new EnumService<TYPE,ID>();
			throw Missing("service");
		}

		/// <summary>
		///   Determines the ID mapper function to use.
		///   This will create fallback mappers for <c>enum</c> and <c>Model</c> <typeparamref name="TYPE"/>s.
		/// </summary>
		/// <returns>The ID mapper.</returns>
		/// <exception cref="ArgumentException">If no mapper function was registered and there's no fallback.</exception>
		protected Func<TYPE,ID> GetIDMapper()
		{
			if(IDMapper!=null)
				return IDMapper;
			if(IsEnum())
				return (TYPE i) => (dynamic)i;
			if(typeof(Model).IsAssignableFrom(typeof(TYPE)))
				return (TYPE i) => (ID)(dynamic)((Model)(dynamic)i).ID;
			throw Missing("ID mapper");
		}

		/// <summary>
		///   Determines the label mapper function to use.
		/// </summary>
		/// <returns>The label mapper.</returns>
		/// <exception cref="ArgumentException">If no mapper function was registered.</exception>
		protected Func<TYPE,string> GetLabelMapper()
		{
			if(LabelMapper!=null)
				return LabelMapper;
			throw Missing("label mapper");
		}

		/// <summary>
		///   Checks whether <typeparamref name="TYPE"/> is an enum.
		/// </summary>
		/// <returns><c>true</c> if generic type is an enum, regardless of nullability.</returns>
		protected bool IsEnum()
		{
			Type type=typeof(TYPE);
			var nullableBaseType=Nullable.GetUnderlyingType(type);
			if(nullableBaseType!=null)
				type=nullableBaseType;
			return type.IsEnum;
		}
	}
}
