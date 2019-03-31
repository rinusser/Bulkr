// Copyright 2019 Richard Nusser
// Licensed under GPLv3 (see http://www.gnu.org/licenses/)

using System.Reflection;
using NUnit.Framework;

using Bulkr.Gui.Forms.Field;
using Bulkr.Gui_Tests.TestTargets;

namespace Bulkr.Gui_Tests.Components
{
	public class TestCase
	{
		public string ID;
		public string IDMessage;
		public string RequiredString;
		public string RequiredStringMessage;
		public string OptionalString;
		public string OptionalStringMessage;
		public string RequiredFloat;
		public string RequiredFloatMessage;
		public string OptionalFloat;
		public string OptionalFloatMessage;
		public string RequiredEnum;
		public string RequiredEnumMessage;
		public string OptionalEnum;
		public string OptionalEnumMessage;

		public TargetModel Model;


		public static TestCase Empty()
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
				RequiredEnum=DropDown<TargetEnum>.NULL_LABEL,
				RequiredEnumMessage="'please select' entry should be selected by default",
				OptionalEnum=DropDown<TargetEnum>.NULL_LABEL,
				OptionalEnumMessage="'please select' entry should be selected by default",

				Model=new TargetModel()
			};
		}

		public static TestCase RequiredOnly()
		{
			return new TestCase
			{
				RequiredString="stringy",
				OptionalString="",
				RequiredFloat="0.12",
				OptionalFloat="",
				OptionalFloatMessage="field should be empty for optional floats set to null",
				RequiredEnum=TargetComponent.ENUM_LABEL_ONE,
				OptionalEnum=DropDown<TargetEnum>.NULL_LABEL,
				OptionalEnumMessage="'please select' entry should be selected for null values",

				Model=new TargetModel
				{
					RequiredString="stringy",
					RequiredFloat=0.12F,
					RequiredEnum=TargetEnum.One
				}
			};
		}

		public static TestCase Full1()
		{
			return new TestCase
			{
				RequiredString="E",
				OptionalString="B",
				RequiredFloat="99999.99",
				OptionalFloat="0.01",
				RequiredEnum=TargetComponent.ENUM_LABEL_TWO,
				OptionalEnum=TargetComponent.ENUM_LABEL_ONE,

				Model=new TargetModel
				{
					RequiredString="E",
					OptionalString="B",
					RequiredFloat=99999.99F,
					OptionalFloat=0.01F,
					RequiredEnum=TargetEnum.Two,
					OptionalEnum=TargetEnum.One
				}
			};
		}

		public static TestCase Full2()
		{
			return new TestCase
			{
				RequiredString="xx",
				OptionalString="yy",
				RequiredFloat="0.1",
				OptionalFloat="2.3",
				RequiredEnum=TargetComponent.ENUM_LABEL_ONE,
				OptionalEnum=TargetComponent.ENUM_LABEL_TWO,

				Model=new TargetModel
				{
					RequiredString="xx",
					OptionalString="yy",
					RequiredFloat=0.1F,
					OptionalFloat=2.3F,
					RequiredEnum=TargetEnum.One,
					OptionalEnum=TargetEnum.Two,
				}
			};
		}

		public void TestForm(TargetWindow window)
		{
			if(ID!=null)
				Assert.AreEqual(ID,window.targetmodel_id_value.Text,IDMessage);

			if(RequiredString!=null)
				Assert.AreEqual(RequiredString,window.targetmodel_requiredstring_value.Text,RequiredStringMessage);

			if(OptionalString!=null)
				Assert.AreEqual(OptionalString,window.targetmodel_optionalstring_value.Text,OptionalStringMessage);

			if(RequiredFloat!=null)
				Assert.AreEqual(RequiredFloat,window.targetmodel_requiredfloat_value.Text,RequiredFloatMessage);

			if(OptionalFloat!=null)
				Assert.AreEqual(OptionalFloat,window.targetmodel_optionalfloat_value.Text,OptionalFloatMessage);

			if(RequiredEnum!=null)
				Assert.AreEqual(RequiredEnum,window.targetmodel_requiredenum_value.ActiveText,RequiredEnumMessage);

			if(OptionalEnum!=null)
				Assert.AreEqual(OptionalEnum,window.targetmodel_optionalenum_value.ActiveText,OptionalEnumMessage);
		}

		public void Enter(TargetWindow window)
		{
			window.targetmodel_requiredstring_value.Text=RequiredString??"";
			window.targetmodel_optionalstring_value.Text=OptionalString??"";
			window.targetmodel_requiredfloat_value.Text=RequiredFloat??"";
			window.targetmodel_optionalfloat_value.Text=OptionalFloat??"";
			DropDown<TargetEnum>.SelectLabelIn(window.targetmodel_requiredenum_value,RequiredEnum??DropDown<TargetEnum>.NULL_LABEL);
			DropDown<TargetEnum>.SelectLabelIn(window.targetmodel_optionalenum_value,OptionalEnum??DropDown<TargetEnum>.NULL_LABEL);
		}

		public void TestModel(TargetModel candidate)
		{
			foreach(PropertyInfo property in candidate.GetType().GetProperties())
				if(property.Name!="ID")
					Assert.AreEqual(property.GetValue(Model),property.GetValue(candidate),property.Name);
		}
	}
}
