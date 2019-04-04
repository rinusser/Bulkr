// Copyright 2019 Richard Nusser
// Licensed under GPLv3 (see http://www.gnu.org/licenses/)

using Bulkr.Gui.Forms.Field;
using Bulkr.Gui_Tests.TestTargets;

namespace Bulkr.Gui_Tests.Components
{
	public class TestCaseFactory
	{
		public ReferencedModel ReferencedItem1 { get; }
		public ReferencedModel ReferencedItem2 { get; }

		protected ReferencedService ReferencedService { get; }


		public TestCaseFactory(ReferencedService referencedService)
		{
			ReferencedService=referencedService;
			ReferencedItem1=ReferencedService.Add(new ReferencedModel { Title="first referenced item" });
			ReferencedItem2=ReferencedService.Add(new ReferencedModel { Title="second referenced item" });
		}

		public TestCase Empty()
		{
			return new TestCase
			{
				ID="",
				IDMessage="ID should be empty for new items",
				RequiredString="",
				OptionalString="",
				RequiredFloat="",
				RequiredFloatMessage="non-nullable floats should get empty input fields for new items",
				OptionalFloat="",
				RequiredEnum=DropDown<TargetModel,TargetEnum>.NULL_LABEL,
				RequiredEnumMessage="'please select' entry should be selected by default",
				OptionalEnum=DropDown<TargetModel,TargetEnum>.NULL_LABEL,
				OptionalEnumMessage="'please select' entry should be selected by default",
				RequiredServiceDropDown=DropDown<TargetModel,ReferencedModel>.NULL_LABEL,
				RequiredServiceDropDownMessage="'please select' entry should be selected by default",
				OptionalServiceDropDown=DropDown<TargetModel,ReferencedModel>.NULL_LABEL,
				OptionalServiceDropDownMessage="'please select' entry should be selected by default",

				Model=new TargetModel()
			};
		}

		public TestCase RequiredOnly()
		{
			return new TestCase
			{
				RequiredString="stringy",
				OptionalString="",
				RequiredFloat="0.12",
				OptionalFloat="",
				OptionalFloatMessage="field should be empty for optional floats set to null",
				RequiredEnum=TargetComponent.ENUM_LABEL_ONE,
				OptionalEnum=DropDown<TargetModel,TargetEnum>.NULL_LABEL,
				OptionalEnumMessage="'please select' entry should be selected for null values",
				RequiredServiceDropDown=ReferencedItem1.Title,
				OptionalServiceDropDown=DropDown<TargetModel,TargetEnum>.NULL_LABEL,
				OptionalServiceDropDownMessage="'please select' entry should be selected for null values",

				Model=new TargetModel
				{
					RequiredString="stringy",
					RequiredFloat=0.12F,
					RequiredEnum=TargetEnum.One,
					RequiredServiceDropDown=ReferencedItem1,
				}
			};
		}

		public TestCase Full1()
		{
			return new TestCase
			{
				RequiredString="E",
				OptionalString="B",
				RequiredFloat="99999.99",
				OptionalFloat="0.01",
				RequiredEnum=TargetComponent.ENUM_LABEL_TWO,
				OptionalEnum=TargetComponent.ENUM_LABEL_ONE,
				RequiredServiceDropDown=ReferencedItem2.Title,
				OptionalServiceDropDown=ReferencedItem1.Title,

				Model=new TargetModel
				{
					RequiredString="E",
					OptionalString="B",
					RequiredFloat=99999.99F,
					OptionalFloat=0.01F,
					RequiredEnum=TargetEnum.Two,
					OptionalEnum=TargetEnum.One,
					RequiredServiceDropDown=ReferencedItem2,
					OptionalServiceDropDown=ReferencedItem1,
				}
			};
		}

		public TestCase Full2()
		{
			return new TestCase
			{
				RequiredString="xx",
				OptionalString="yy",
				RequiredFloat="0.1",
				OptionalFloat="2.3",
				RequiredEnum=TargetComponent.ENUM_LABEL_ONE,
				OptionalEnum=TargetComponent.ENUM_LABEL_TWO,
				RequiredServiceDropDown=ReferencedItem1.Title,
				OptionalServiceDropDown=ReferencedItem2.Title,

				Model=new TargetModel
				{
					RequiredString="xx",
					OptionalString="yy",
					RequiredFloat=0.1F,
					OptionalFloat=2.3F,
					RequiredEnum=TargetEnum.One,
					OptionalEnum=TargetEnum.Two,
					RequiredServiceDropDown=ReferencedItem1,
					OptionalServiceDropDown=ReferencedItem2,
				}
			};
		}
	}
}
