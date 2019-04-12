// Copyright 2019 Richard Nusser
// Licensed under GPLv3 (see http://www.gnu.org/licenses/)

using System;

using Bulkr.Core.Services;
using Bulkr.Gui.Components;
using Bulkr.Gui.Forms;

namespace Bulkr.Gui_Tests.TestTargets
{
	public class TargetComponent : CRUDComponent<TargetModel>
	{
		public static readonly string ENUM_LABEL_ONE="oNe";
		public static readonly string ENUM_LABEL_TWO="twO";


		public TargetComponent(TargetComponentContext context) : base(context)
		{
		}


		protected override Form<TargetModel> CreateForm()
		{
			var referencedService=((TargetComponentContext)Context).ReferencedService;

			return new FormBuilder<TargetModel>()
				.InComponent(this)
				.AddIDField("ID")
				.AddTextField("RequiredString")
					.Required()
				.AddTextField("OptionalString")
				.AddNumberField("RequiredFloat")
				.AddNumberField("OptionalFloat")
				.AddDropDownField<TargetEnum,TargetEnum>("RequiredEnum")
					.WithLabelMapper(GetTargetEnumDisplayString)
					.Required()
				.AddDropDownField<TargetEnum?,TargetEnum>("OptionalEnum")
					.WithLabelMapper(GetTargetEnumDisplayString)
				.AddDropDownField<ReferencedModel,int>("RequiredServiceDropDown")
					.WithService(referencedService)
					.WithLabelMapper(GetReferencedModelDisplayString)
					.Required()
				.AddDropDownField<ReferencedModel,int>("OptionalServiceDropDown")
					.WithService(referencedService)
					.WithLabelMapper(GetReferencedModelDisplayString)
				.AddDateTimeField("RequiredDateTime")
				.Build();
		}

		public static string GetReferencedModelDisplayString(ReferencedModel model)
		{
			return model.Title;
		}


		public static string GetTargetEnumDisplayString(TargetEnum? value)
		{
			return value!=null ? GetTargetEnumDisplayString((TargetEnum)value) : "";
		}

		public static string GetTargetEnumDisplayString(TargetEnum value)
		{
			switch(value)
			{
				case TargetEnum.One:
					return ENUM_LABEL_ONE;
				case TargetEnum.Two:
					return ENUM_LABEL_TWO;
			}
			throw new NotImplementedException();
		}

		protected override DatabaseCRUDService<TargetModel> CreateService()
		{
			return TargetService.Create(NUnit.Framework.TestContext.CurrentContext.Test.FullName);
		}


		public Form<TargetModel> GetForm()
		{
			return Form;
		}
	}
}
