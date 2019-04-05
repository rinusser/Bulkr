// Copyright 2019 Richard Nusser
// Licensed under GPLv3 (see http://www.gnu.org/licenses/)

using NUnit.Framework;
using System;

using Bulkr.Gui.Forms.Field;
using Bulkr.Gui.Utils;
using Bulkr.Gui_Tests.TestTargets;

namespace Bulkr.Gui_Tests.Components
{
	[TestFixture]
	public class SaveTest : ComponentTestBase
	{
		[Test]
		public void TestAddingValidItem()
		{
			Component.New();
			var full2=TestCaseFactory.Full2();

			full2.Enter(Window);
			Assert.AreEqual("",Window.targetmodel_id_value.Text,"(internal check)");
			SaveAndAssertSuccess();

			Assert.Greater(int.Parse(Window.targetmodel_id_value.Text),0,"item should have gotten an ID when saving");
			var items=CreateService().GetAll();
			Assert.AreEqual(1,items.Count);
			full2.TestModel(items[0]);
		}

		[Test]
		public void TestAddingMinimalItem()
		{
			var requiredOnly=TestCaseFactory.RequiredOnly();

			Component.New();
			requiredOnly.Enter(Window);
			SaveAndAssertSuccess();

			var item=CreateService().GetAll()[0];
			requiredOnly.TestModel(item);
		}

		[Test]
		public void TestSavingWithOptionalsRemoved()
		{
			var requiredOnly=TestCaseFactory.RequiredOnly();
			Service.Add(TestCaseFactory.Full1().Model);

			Component.NavTo(1);
			requiredOnly.Enter(Window);
			SaveAndAssertSuccess();

			var items=CreateService().GetAll();
			Assert.AreEqual(1,items.Count,"saving existing item shouldn't have added another");
			requiredOnly.TestModel(items[0]);
		}

		[Test]
		public void TestSavingWithOptionalsAdded()
		{
			var full1=TestCaseFactory.Full1();
			Service.Add(TestCaseFactory.RequiredOnly().Model);

			Component.NavTo(1);
			full1.Enter(Window);
			SaveAndAssertSuccess();

			var items=CreateService().GetAll();
			full1.TestModel(items[0]);
		}

		[Test]
		public void TestSavingWithOptionalsOverwritten()
		{
			Service.Add(TestCaseFactory.Full1().Model);

			Component.NavTo(1);
			var full2=TestCaseFactory.Full2();
			full2.Enter(Window);
			SaveAndAssertSuccess();

			var items=CreateService().GetAll();
			full2.TestModel(items[0]);
		}

		[Test]
		public void TestSavingNewTwiceCreatesOnlyOneEntry()
		{
			Component.New();
			TestCaseFactory.RequiredOnly().Enter(Window);
			SaveAndAssertSuccess();
			SaveAndAssertSuccess();

			Assert.AreEqual(1,CreateService().GetAll().Count);
		}

		private void SaveAndAssertSuccess()
		{
			var result=Component.Save();
			if(!result)
				Assert.Fail("Unexpected failure of Component.Save():\n  "+string.Join("\n  ",Window.LogMessages));

			Assert.True(true);  //increase assertion count by 1
		}


		[Test]
		public void TestRequiredStringIsRequired()
		{
			RunValidationFailureTestWithInputAs(Window.targetmodel_requiredstring_value,"");
		}

		[Test]
		public void TestRequiredFloatIsRequired()
		{
			RunValidationFailureTestWithInputAs(Window.targetmodel_requiredfloat_value,"");
		}

		[Test]
		public void TestFloatsCantBeNegative()
		{
			RunValidationFailureTestWithInputAs(Window.targetmodel_requiredfloat_value,"-0.1");
		}

		[Test]
		public void TestFloatsMustBeNumeric()
		{
			RunValidationFailureTestWithInputAs(Window.targetmodel_requiredfloat_value,"a");
		}

		[Test]
		public void TestRequiredEnumIsRequired()
		{
			RunValidationFailureTestWithInputAs(Window.targetmodel_requiredenum_value,null);
		}

		[Test]
		public void TestRequiredServiceDropDownIsRequired()
		{
			RunValidationFailureTestWithInputAs(Window.targetmodel_requiredservicedropdown_value,null);
		}

		[Test]
		public void TestRequiredDateTimeHourIsRequiredAndRangeLimited()
		{
			foreach(var input in new string[] { "","-1","24" })
				RunValidationFailureTestWithInputAs(Window.targetmodel_requireddatetime_hour_value,input);
		}

		[Test]
		public void TestRequiredDateTimeMinuteIsRequiredAndRangeLimited()
		{
			foreach(var input in new string[] { "","-1","60" })
				RunValidationFailureTestWithInputAs(Window.targetmodel_requireddatetime_minute_value,input);
		}

		private void RunValidationFailureTestWithInputAs(Gtk.ComboBox field,string input)
		{
			RunValidationFailureTestWith(() => field.SelectLabel(input??DropDown<TargetModel,TargetEnum>.NULL_LABEL));
		}

		private void RunValidationFailureTestWithInputAs(Gtk.Entry field,string input)
		{
			RunValidationFailureTestWith(() => field.Text=input);
		}

		private void RunValidationFailureTestWith(Action setup)
		{
			Component.New();
			TestCaseFactory.Full2().Enter(Window);
			setup();
			Assert.AreEqual(false,Component.Save(),"saving item should have failed");
			Assert.AreEqual(0,CreateService().GetAll().Count,"no item should have been added");
		}
	}
}
