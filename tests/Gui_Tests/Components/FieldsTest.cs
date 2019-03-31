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
			RunDropDownValuesTest(Window.targetmodel_requiredenum_value);
			RunDropDownValuesTest(Window.targetmodel_optionalenum_value);
		}

		private void RunDropDownValuesTest(Gtk.ComboBox widget)
		{
			var expected=new List<string>
			{
				DropDown<TargetEnum>.NULL_LABEL,
				TargetComponent.ENUM_LABEL_ONE,
				TargetComponent.ENUM_LABEL_TWO
			};

			var actual=new List<string>();
			widget.ForEach((v,i) =>
			{
				actual.Add((string)v.Val);
				return true;
			});

			Assert.AreEqual(expected,actual);
		}

		[Test]
		public void TestDisplayingItemWithOptionalFieldsNull()
		{
			TestCase requiredOnly=TestCase.RequiredOnly();
			Service.Add(requiredOnly.Model);
			Service.Add(TestCase.Full1().Model);

			Component.NavTo(1);
			requiredOnly.TestForm(Window);

			AssertIDIsPositive();
			AssertNavigationIsAt("1/2","navigation should be at 1 of 2 items");
		}

		[Test]
		public void TestDisplayingItemWithAllFields()
		{
			TestCase filled=TestCase.Full1();
			Service.Add(TestCase.RequiredOnly().Model);
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
