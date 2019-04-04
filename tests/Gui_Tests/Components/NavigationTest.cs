// Copyright 2019 Richard Nusser
// Licensed under GPLv3 (see http://www.gnu.org/licenses/)

using System.Collections.Generic;
using NUnit.Framework;

namespace Bulkr.Gui_Tests.Components
{
	[TestFixture]
	public class NavigationTest : ComponentTestBase
	{
		private List<int> StoredIDs;

		[SetUp]
		public void SetUpRepository()
		{
			Service.Add(TestCaseFactory.Full1().Model);
			Service.Add(TestCaseFactory.RequiredOnly().Model);
			Service.Add(TestCaseFactory.Full1().Model);
			Service.Add(TestCaseFactory.RequiredOnly().Model);
			StoredIDs=GetStoredIDs();
		}

		[Test]
		public void TestNavTo()
		{
			Component.NavTo(3);
			AssertIsAt(3);

			Component.NavTo(1);
			AssertIsAt(1);
		}

		[Test]
		public void TestPrevNextButtons()
		{
			Component.NavTo(1); //initialize component

			Component.Previous();
			AssertIsAt(1);

			Component.Next();
			AssertIsAt(2);

			Component.NavTo(4);
			Component.Previous();
			AssertIsAt(3);

			Component.Next();
			Component.Next();
			AssertIsAt(4);
		}

		private void AssertIsAt(int number)
		{
			Assert.AreEqual(StoredIDs[number-1].ToString(),Window.targetmodel_id_value.Text);
			Assert.AreEqual(string.Format("{0}/4",number),Window.targetmodel_nav_label.Text);
		}
	}
}
