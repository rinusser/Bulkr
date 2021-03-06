﻿// Copyright 2019 Richard Nusser
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

			TestCaseFactory.Empty().TestForm(Window);

			Component.New();
			AssertNavigationIsAt("1/1","clicking 'New' twice shouldn't change navigation position");
		}

		[Test]
		public void TestClickingNewTwiceShowsCorrectNavigation()
		{
			Service.Add(TestCaseFactory.RequiredOnly().Model);
			Component.NavTo(1);

			Component.New();
			AssertNavigationIsAt("2/2","navigation should be at 2 of 2 items if an item already exists");

			Component.New();
			AssertNavigationIsAt("2/2","clicking 'New' twice shouldn't change navigation position");
		}

		[Test]
		public void TestInitialDisplayWithoutItemsShouldBeEmpty()
		{
			Component.NavTo(1);
			TestCaseFactory.Empty().TestForm(Window);
			AssertNavigationIsAt("1/1","loading an empty list should default to adding item 1/1");
		}
	}
}
