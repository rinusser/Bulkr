// Copyright 2019 Richard Nusser
// Licensed under GPLv3 (see http://www.gnu.org/licenses/)

using NUnit.Framework;

namespace Bulkr.Gui_Tests.Components
{
	[TestFixture]
	public class StylingTest : ComponentTestBase
	{
		protected class ErrorStylingTestCase
		{
			public bool RequiredString;
			public bool OptionalString;
			public bool RequiredFloat;
			public bool OptionalFloat;
			public bool RequiredEnum;
			public bool OptionalEnum;
			public bool RequiredServiceDropDown;
			public bool OptionalServiceDropDown;


			public static ErrorStylingTestCase Valid()
			{
				return new ErrorStylingTestCase();
			}

			public static ErrorStylingTestCase AllInvalid()
			{
				return new ErrorStylingTestCase
				{
					RequiredString=true,
					RequiredFloat=true,
					OptionalFloat=true,
					RequiredEnum=true,
					RequiredServiceDropDown=true,
				};
			}
		}

		[Test]
		public void TestAddingItemSetsAndClearsErrors()
		{
			Component.New();
			AssertStyling(ErrorStylingTestCase.Valid(),"new items should start without errors");

			TestCaseFactory.AllInvalid().Enter(Window);
			Component.Save();
			AssertStyling(ErrorStylingTestCase.AllInvalid(),"saving with errors should fail");

			Component.Save();
			AssertStyling(ErrorStylingTestCase.AllInvalid(),"saving erroneous data twice should still fail");

			TestCaseFactory.RequiredOnly().Enter(Window);
			Component.Save();
			AssertStyling(ErrorStylingTestCase.Valid(),"saving valid data should clear all errors");
		}

		[Test]
		public void TestRevertingShouldClearErrors()
		{
			PrepareErrorState();
			Component.Revert();
			AssertStyling(ErrorStylingTestCase.Valid(),"reverting should clear all errors");
		}

		[Test]
		public void TestClickingNewShouldClearErrors()
		{
			PrepareErrorState();
			Component.New();
			AssertStyling(ErrorStylingTestCase.Valid(),"reverting should clear all errors");
		}

		[Test]
		public void TestNavigatingAwayShouldClearErrors()
		{
			PrepareErrorState();
			Component.Next();
			AssertStyling(ErrorStylingTestCase.Valid(),"navigating away should clear all errors");
		}

		[Test]
		public void TestDeletingNewShouldClearErrors()
		{
			PrepareErrorState();
			Component.Delete();
			AssertStyling(ErrorStylingTestCase.Valid(),"deleting new item should clear all errors");
		}

		protected void PrepareErrorState()
		{
			Component.New();
			TestCaseFactory.AllInvalid().Enter(Window);
			Component.Save();
			AssertStyling(ErrorStylingTestCase.AllInvalid(),"(internal check)");
		}


		[Test]
		public void TestDeletingSavedShouldClearErrors()
		{
			Service.Add(TestCaseFactory.Full1().Model);
			Component.NavTo(1);
			TestCaseFactory.AllInvalid().Enter(Window);
			Component.Save();
			AssertStyling(ErrorStylingTestCase.AllInvalid(),"(internal check)");

			Component.Delete();
			AssertStyling(ErrorStylingTestCase.Valid(),"deleting stored item should clear all errors");
		}

		[Test]
		public void TestErrorsAreIsolated()
		{
			Component.New();

			var partial1=TestCaseFactory.RequiredOnly();
			partial1.RequiredString="";
			partial1.OptionalFloat="a";
			partial1.RequiredServiceDropDownID=null;
			partial1.Enter(Window);
			Component.Save();
			var expected1=new ErrorStylingTestCase { RequiredString=true,OptionalFloat=true,RequiredServiceDropDown=true };
			AssertStyling(expected1,"only erroneous input should be marked");

			var partial2=TestCaseFactory.RequiredOnly();
			partial2.RequiredFloat="";
			partial2.RequiredEnumID=null;
			partial2.Enter(Window);
			Component.Save();
			var expected2=new ErrorStylingTestCase { RequiredFloat=true,RequiredEnum=true };
			AssertStyling(expected2,"errors should have moved to other fields");

			TestCaseFactory.RequiredOnly().Enter(Window);
			Component.Save();
			AssertStyling(ErrorStylingTestCase.Valid(),"all errors should be gone after saving valid data");
		}


		protected void AssertStyling(ErrorStylingTestCase testCase,string message)
		{
			foreach(var field in typeof(ErrorStylingTestCase).GetFields())
			{
				var expected=(bool)field.GetValue(testCase);
				var formField=Component.GetForm().GetField(field.Name);
				var actual=!formField.GetLastValidityStyleSet();
				Assert.AreEqual(GetErrorStateString(expected),GetErrorStateString(actual),string.Format("{0}, {1}",field.Name,message));

				AssertHasTooltip(expected,Component.GetFieldValueWidget<Gtk.Widget>(field.Name),"input",field.Name,message);
				AssertHasTooltip(expected,Component.GetFieldLabelWidget(field.Name),"label",field.Name,message);
			}
		}

		protected void AssertHasTooltip(bool expected,Gtk.Widget subject,string description,string fieldName,string message)
		{
			Assert.AreEqual(expected,subject.HasTooltip,string.Format("{0} {1} tooltip, {2}",fieldName,description,message));
		}

		protected string GetErrorStateString(bool errorState)
		{
			return errorState ? "error" : "valid";
		}
	}
}
