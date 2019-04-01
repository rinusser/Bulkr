﻿// Copyright 2019 Richard Nusser
// Licensed under GPLv3 (see http://www.gnu.org/licenses/)

using System.Collections.Generic;

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


		public TargetComponent(TargetWindow window) : base(window)
		{
		}


		protected override Form<TargetModel> CreateForm()
		{
			var window=(TargetWindow)Window;

			var enumOptions=new Dictionary<TargetEnum,string>
			{
				{ TargetEnum.One,ENUM_LABEL_ONE },
				{ TargetEnum.Two,ENUM_LABEL_TWO },
			};

			return new Form<TargetModel>()
				.AddField(new ID("ID",window.targetmodel_id_value))
				.AddField(new Text("RequiredString",window.targetmodel_requiredstring_value,Text.Option.Required))
				.AddField(new Text("OptionalString",window.targetmodel_optionalstring_value))
				.AddField(new Number("RequiredFloat",window.targetmodel_requiredfloat_value))
				.AddField(new Number("OptionalFloat",window.targetmodel_optionalfloat_value))
				.AddField(new DropDown<TargetEnum>("RequiredEnum",window.targetmodel_requiredenum_value,enumOptions))
				.AddField(new DropDown<TargetEnum>("OptionalEnum",window.targetmodel_optionalenum_value,enumOptions));
		}

		protected override Service<TargetModel> CreateService()
		{
			return TargetService.Create(NUnit.Framework.TestContext.CurrentContext.Test.FullName);
		}
	}
}