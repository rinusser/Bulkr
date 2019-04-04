// Copyright 2019 Richard Nusser
// Licensed under GPLv3 (see http://www.gnu.org/licenses/)

using System.Collections.Generic;
using NUnit.Framework;

using Bulkr.Gui.Forms.Field;
using Bulkr.Gui.Utils;
using Bulkr.Gui_Tests.TestTargets;

namespace Bulkr.Gui_Tests.Components
{
	[TestFixture]
	public class FieldsTest : ComponentTestBase
	{
		[Test]
		public void TestDropDownsArePopulated()
		{
			var expected=new List<string>
			{
				DropDown<TargetModel,TargetEnum>.NULL_LABEL,
				TargetComponent.ENUM_LABEL_ONE,
				TargetComponent.ENUM_LABEL_TWO
			};

			RunDropDownValuesTest(Window.targetmodel_requiredenum_value,expected);
			RunDropDownValuesTest(Window.targetmodel_optionalenum_value,expected);
		}

		[Test]
		public void TestServiceDropDownsArePopulated()
		{
			var expected=new List<string>
			{
				DropDown<TargetModel,TargetEnum>.NULL_LABEL,
				TestCaseFactory.ReferencedItem1.Title,
				TestCaseFactory.ReferencedItem2.Title,
			};

			RunDropDownValuesTest(Window.targetmodel_requiredservicedropdown_value,expected);
			RunDropDownValuesTest(Window.targetmodel_optionalservicedropdown_value,expected);
		}

		private void RunDropDownValuesTest(Gtk.ComboBox widget,IList<string> expected)
		{
			var actual=new List<string>();
			widget.ForEach((v,i) =>
			{
				actual.Add((string)v.Val);
				return true;
			});

			Assert.AreEqual(expected,actual);
		}

		[Test]
		public void TestServiceDropDownReload()
		{
			Gtk.ComboBox dropdown=Window.targetmodel_requiredservicedropdown_value;
			dropdown.SelectLabel(TestCaseFactory.ReferencedItem1.Title);
			ReferencedService.Add(new ReferencedModel { Title="newly added item" });
			Component.Reload();
			Assert.AreEqual(TestCaseFactory.ReferencedItem1.Title,dropdown.ActiveText);

			var expected=new List<string>
			{
				DropDown<TargetModel,TargetEnum>.NULL_LABEL,
				TestCaseFactory.ReferencedItem1.Title,
				TestCaseFactory.ReferencedItem2.Title,
				"newly added item"
			};
			RunDropDownValuesTest(dropdown,expected);

			ReferencedService.Delete(TestCaseFactory.ReferencedItem1);
			Component.Reload();
			expected.RemoveAt(1);
			RunDropDownValuesTest(dropdown,expected);

			Assert.AreEqual(DropDown<TargetModel,ReferencedModel>.NULL_LABEL,dropdown.ActiveText);
		}

		[Test]
		public void TestDisplayingItemWithOptionalFieldsNull()
		{
			TestCase requiredOnly=TestCaseFactory.RequiredOnly();
			Service.Add(requiredOnly.Model);
			Service.Add(TestCaseFactory.Full1().Model);

			Component.NavTo(1);
			requiredOnly.TestForm(Window);

			AssertIDIsPositive();
			AssertNavigationIsAt("1/2","navigation should be at 1 of 2 items");
		}

		[Test]
		public void TestDisplayingItemWithAllFields()
		{
			TestCase filled=TestCaseFactory.Full1();
			Service.Add(TestCaseFactory.RequiredOnly().Model);
			Service.Add(filled.Model);

			Component.NavTo(2);
			filled.TestForm(Window);

			AssertIDIsPositive();
			AssertNavigationIsAt("2/2","navigation should be at 2 of 2 items");
		}

		private void AssertIDIsPositive()
		{
			Assert.Greater(int.Parse(Window.targetmodel_id_value.Text),0,"ID should be positive");
		}
	}
}
