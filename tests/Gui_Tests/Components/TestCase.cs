// Copyright 2019 Richard Nusser
// Licensed under GPLv3 (see http://www.gnu.org/licenses/)

using System;
using System.Globalization;
using System.Reflection;
using NUnit.Framework;

using Bulkr.Gui.Forms.Field;
using Bulkr.Gui.Utils;
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
		public string RequiredServiceDropDown;
		public string RequiredServiceDropDownMessage;
		public string OptionalServiceDropDown;
		public string OptionalServiceDropDownMessage;
		public string RequiredDateTimeDate;
		public string RequiredDateTimeDateMessage;
		public string RequiredDateTimeHour;
		public string RequiredDateTimeHourMessage;
		public string RequiredDateTimeMinute;
		public string RequiredDateTimeMinuteMessage;

		public TargetModel Model;


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

			if(RequiredServiceDropDown!=null)
				Assert.AreEqual(RequiredServiceDropDown,window.targetmodel_requiredservicedropdown_value.ActiveText,RequiredServiceDropDownMessage);

			if(OptionalServiceDropDown!=null)
				Assert.AreEqual(OptionalServiceDropDown,window.targetmodel_optionalservicedropdown_value.ActiveText,OptionalServiceDropDownMessage);

			if(RequiredDateTimeDate!=null)
				Assert.AreEqual(RequiredDateTimeDate,window.targetmodel_requireddatetime_date_value.Date.ToString("yyyy-MM-dd"),RequiredDateTimeDateMessage);

			if(RequiredDateTimeHour!=null)
				Assert.AreEqual(RequiredDateTimeHour,window.targetmodel_requireddatetime_hour_value.Text,RequiredDateTimeHourMessage);

			if(RequiredDateTimeMinute!=null)
				Assert.AreEqual(RequiredDateTimeMinute,window.targetmodel_requireddatetime_minute_value.Text,RequiredDateTimeMinuteMessage);
		}


		public void Enter(TargetWindow window)
		{
			window.targetmodel_requiredstring_value.Text=RequiredString??"";
			window.targetmodel_optionalstring_value.Text=OptionalString??"";
			window.targetmodel_requiredfloat_value.Text=RequiredFloat??"";
			window.targetmodel_optionalfloat_value.Text=OptionalFloat??"";
			window.targetmodel_requiredenum_value.SelectLabel(RequiredEnum??DropDown<TargetModel,TargetEnum>.NULL_LABEL);
			window.targetmodel_optionalenum_value.SelectLabel(OptionalEnum??DropDown<TargetModel,TargetEnum>.NULL_LABEL);
			window.targetmodel_requiredservicedropdown_value.SelectLabel(RequiredServiceDropDown??DropDown<TargetModel,ReferencedModel>.NULL_LABEL);
			window.targetmodel_optionalservicedropdown_value.SelectLabel(OptionalServiceDropDown??DropDown<TargetModel,ReferencedModel>.NULL_LABEL);
			window.targetmodel_requireddatetime_date_value.Date=RequiredDateTimeDate!=null ? DateTime.ParseExact(RequiredDateTimeDate,"yyyy-MM-dd",CultureInfo.InvariantCulture) : DateTime.Today;
			window.targetmodel_requireddatetime_hour_value.Text=RequiredDateTimeHour??"";
			window.targetmodel_requireddatetime_minute_value.Text=RequiredDateTimeMinute??"";
		}

		public void TestModel(TargetModel candidate)
		{
			foreach(PropertyInfo property in candidate.GetType().GetProperties())
			{
				if(property.Name=="ID")
					continue;
				object expected=property.GetValue(Model);
				object actual=property.GetValue(candidate);

				if(property.PropertyType==typeof(ReferencedModel))
				{
					expected=((ReferencedModel)expected)?.ID;
					actual=((ReferencedModel)actual)?.ID;
				}
				Assert.AreEqual(expected,actual,property.Name);
			}
		}
	}
}
