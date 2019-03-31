// Copyright 2019 Richard Nusser
// Licensed under GPLv3 (see http://www.gnu.org/licenses/)

using NUnit.Framework;

namespace Bulkr.Gui_Tests.Components
{
	[TestFixture]
	public class NewTest : ComponentTestBase
	{
		[Test]
		public void TestClickingNewShowsCorrectValues()
		{
			Component.New();
			AssertNavigationIsAt("1/1","navigation should be at 1 of 1 item if table is empty");

			TestCase.Empty().TestForm(Window);

			Component.New();
			AssertNavigationIsAt("1/1","clicking 'New' twice shouldn't change navigation position");
		}

		[Test]
		public void TestClickingNewTwiceShowsCorrectNavigation()
		{
			Service.Add(TestCase.RequiredOnly().Model);
			Component.NavTo(1);

			Component.New();
			AssertNavigationIsAt("2/2","navigation should be at 2 of 2 items if an item already exists");

			Component.New();
			AssertNavigationIsAt("2/2","clicking 'New' twice shouldn't change navigation position");
		}
	}
}
