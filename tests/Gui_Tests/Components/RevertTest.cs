// Copyright 2019 Richard Nusser
// Licensed under GPLv3 (see http://www.gnu.org/licenses/)

using NUnit.Framework;

namespace Bulkr.Gui_Tests.Components
{
	[TestFixture]
	public class RevertTest : ComponentTestBase
	{
		[Test]
		public void TestRevertingNewItemEmptiesForm()
		{
			Component.New();
			TestCase.Full1().Enter(Window);
			Component.Revert();

			TestCase.Empty().TestForm(Window);
		}

		[Test]
		public void TestRevertingRequiredOnlyItemResetsForm()
		{
			TestCase requiredOnly=TestCase.RequiredOnly();
			Service.Add(requiredOnly.Model);

			Component.NavTo(1);
			TestCase.Full1().Enter(Window);
			Component.Revert();

			requiredOnly.TestForm(Window);
		}

		[Test]
		public void TestRevertingFilledItemResetsForm()
		{
			TestCase filled=TestCase.Full1();
			Service.Add(filled.Model);

			Component.NavTo(1);
			TestCase.Full2().Enter(Window);
			Component.Revert();

			filled.TestForm(Window);
		}

		[Test]
		public void TestRevertingEmptiedFormRestoresEverything()
		{
			TestCase filled=TestCase.Full1();
			Service.Add(filled.Model);

			Component.NavTo(1);
			ClearForm();
			Component.Revert();

			filled.TestForm(Window);
		}
	}
}
