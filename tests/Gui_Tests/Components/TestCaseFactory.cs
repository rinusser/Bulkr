// Copyright 2019 Richard Nusser
// Licensed under GPLv3 (see http://www.gnu.org/licenses/)

using System;

using Bulkr.Gui.Forms.Field;
using Bulkr.Gui_Tests.TestTargets;

namespace Bulkr.Gui_Tests.Components
{
	public class TestCaseFactory
	{
		private static readonly string DROPDOWN_NULL_LABEL=DropDown<TargetModel,TargetEnum,TargetEnum>.NULL_LABEL;


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
			var now=DateTime.Now;
			return new TestCase
			{
				ID="",
				IDMessage="ID should be empty for new items",
				RequiredString="",
				OptionalString="",
				RequiredFloat="",
				RequiredFloatMessage="non-nullable floats should get empty input fields for new items",
				OptionalFloat="",
				RequiredEnum=DROPDOWN_NULL_LABEL,
				RequiredEnumMessage="'please select' entry should be selected by default",
				OptionalEnum=DROPDOWN_NULL_LABEL,
				OptionalEnumMessage="'please select' entry should be selected by default",
				RequiredServiceDropDown=DROPDOWN_NULL_LABEL,
				RequiredServiceDropDownMessage="'please select' entry should be selected by default",
				OptionalServiceDropDown=DROPDOWN_NULL_LABEL,
				OptionalServiceDropDownMessage="'please select' entry should be selected by default",
				RequiredDateTimeDate=now.ToString("yyyy-MM-dd"),
				RequiredDateTimeDateMessage="datetime date field should default to today",
				RequiredDateTimeHour=now.ToString("HH"),
				RequiredDateTimeHourMessage="datetime hour field should default to current hour",
				RequiredDateTimeMinute=now.ToString("mm"),
				RequiredDateTimeMinuteMessage="datetime minute field should default to current minute",

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
				RequiredEnumID=TargetEnum.One,
				OptionalEnum=DROPDOWN_NULL_LABEL,
				OptionalEnumMessage="'please select' entry should be selected for null values",
				RequiredServiceDropDown=ReferencedItem1.Title,
				RequiredServiceDropDownID=ReferencedItem1.ID,
				OptionalServiceDropDown=DROPDOWN_NULL_LABEL,
				OptionalServiceDropDownMessage="'please select' entry should be selected for null values",
				RequiredDateTimeDate="2019-04-05",
				RequiredDateTimeHour="14",
				RequiredDateTimeMinute="13",

				Model=new TargetModel
				{
					RequiredString="stringy",
					RequiredFloat=0.12F,
					RequiredEnum=TargetEnum.One,
					RequiredServiceDropDown=ReferencedItem1,
					RequiredDateTime=new DateTime(2019,4,5,14,13,0),
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
				RequiredEnumID=TargetEnum.Two,
				OptionalEnum=TargetComponent.ENUM_LABEL_ONE,
				OptionalEnumID=TargetEnum.One,
				RequiredServiceDropDown=ReferencedItem2.Title,
				RequiredServiceDropDownID=ReferencedItem2.ID,
				OptionalServiceDropDown=ReferencedItem1.Title,
				OptionalServiceDropDownID=ReferencedItem1.ID,
				RequiredDateTimeDate="2017-12-31",
				RequiredDateTimeHour="23",
				RequiredDateTimeMinute="59",

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
					RequiredDateTime=new DateTime(2017,12,31,23,59,0),
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
				RequiredEnumID=TargetEnum.One,
				OptionalEnum=TargetComponent.ENUM_LABEL_TWO,
				OptionalEnumID=TargetEnum.Two,
				RequiredServiceDropDown=ReferencedItem1.Title,
				RequiredServiceDropDownID=ReferencedItem1.ID,
				OptionalServiceDropDown=ReferencedItem2.Title,
				OptionalServiceDropDownID=ReferencedItem2.ID,
				RequiredDateTimeDate="2020-01-01",
				RequiredDateTimeHour="00",
				RequiredDateTimeMinute="00",

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
					RequiredDateTime=new DateTime(2020,1,1,0,0,0),
				}
			};
		}

		public TestCase AllInvalid()
		{
			return new TestCase
			{
				RequiredString="",
				OptionalString="",                 //can't be invalid
				RequiredFloat="a",
				OptionalFloat="b",
				RequiredEnumID=null,
				OptionalEnumID=null,               //can't be invalid
				RequiredServiceDropDownID=null,
				OptionalServiceDropDownID=null,    //can't be invalid
				RequiredDateTimeDate="2019-04-05", //can't be invalid
				RequiredDateTimeHour="14",         //can't be invalid
				RequiredDateTimeMinute="13",       //can't be invalid
				Model=null,
			};
		}
	}
}