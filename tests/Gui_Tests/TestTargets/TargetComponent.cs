// Copyright 2019 Richard Nusser
// Licensed under GPLv3 (see http://www.gnu.org/licenses/)

using System;

using Bulkr.Core.Services;
using Bulkr.Gui.Forms.Field;
using Bulkr.Gui.Components;
using Bulkr.Gui.Forms;

namespace Bulkr.Gui_Tests.TestTargets
{
	public class TargetComponent : Component<TargetModel>
	{
		public static readonly string ENUM_LABEL_ONE="oNe";
		public static readonly string ENUM_LABEL_TWO="twO";


		public TargetComponent(TargetComponentContext context) : base(context)
		{
		}


		protected override Form<TargetModel> CreateForm()
		{
			var window=(TargetWindow)Window;

			var referencedService=((TargetComponentContext)Context).ReferencedService;

			return new Form<TargetModel>()
				.AddField(new ID<TargetModel>("ID",window.targetmodel_id_value))
				.AddField(new Text<TargetModel>("RequiredString",window.targetmodel_requiredstring_value,Option.Required))
				.AddField(new Text<TargetModel>("OptionalString",window.targetmodel_optionalstring_value))
				.AddField(new Number<TargetModel>("RequiredFloat",window.targetmodel_requiredfloat_value))
				.AddField(new Number<TargetModel>("OptionalFloat",window.targetmodel_optionalfloat_value))
				.AddField(new DropDown<TargetModel,TargetEnum,TargetEnum>("RequiredEnum",
					window.targetmodel_requiredenum_value,
					new EnumService<TargetEnum,TargetEnum>(),
					i => i,
					GetTargetEnumDisplayString,
					Option.Required))
				.AddField(new DropDown<TargetModel,TargetEnum?,TargetEnum>("OptionalEnum",
					window.targetmodel_optionalenum_value,
					new EnumService<TargetEnum?,TargetEnum>(),
					i => i??default(TargetEnum),
					GetTargetEnumDisplayString))
				.AddField(new DropDown<TargetModel,ReferencedModel,int>("RequiredServiceDropDown",
					window.targetmodel_requiredservicedropdown_value,
					referencedService,
					i => i.ID,
					GetReferencedModelDisplayString,
					Option.Required))
				.AddField(new DropDown<TargetModel,ReferencedModel,int>("OptionalServiceDropDown",
					window.targetmodel_optionalservicedropdown_value,
					referencedService,
					i => i.ID,
					GetReferencedModelDisplayString))
				.AddField(new DateTime<TargetModel>("RequiredDateTime",
					window.targetmodel_requireddatetime_date_value,
					window.targetmodel_requireddatetime_hour_value,
					window.targetmodel_requireddatetime_minute_value));
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
	}
}
