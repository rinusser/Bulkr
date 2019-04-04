// Copyright 2019 Richard Nusser
// Licensed under GPLv3 (see http://www.gnu.org/licenses/)

using System.Collections.Generic;
using NUnit.Framework;

namespace Bulkr.Gui_Tests.Components
{
	[TestFixture]
	public class DeleteTest : ComponentTestBase
	{
		[SetUp]
		public void SetUpRepository()
		{
			Service.Add(TestCaseFactory.Full1().Model);
			Service.Add(TestCaseFactory.RequiredOnly().Model);
			Service.Add(TestCaseFactory.Full1().Model);
		}

		[Test]
		public void TestDeleteExisting()
		{
			var ids=GetStoredIDs();

			Component.NavTo(2);
			AssertNavigationIsAt("2/3","(internal check, just to make sure setup is correct)");
			Component.Delete();

			ids.RemoveAt(1);
			Assert.AreEqual(ids,GetStoredIDs(),"the second element should be deleted");
			AssertNavigationIsOneOf(new List<string> { "1/2","2/2" },"navigation should have been updated");
		}

		[Test]
		public void TestDeleteNew()
		{
			var ids=GetStoredIDs();
			Component.NavTo(1); //initialize the component's internal nav state

			Component.New();
			Component.Delete();

			Assert.AreEqual(ids,GetStoredIDs(),"deleting new item should leave stored items intact");
			AssertNavigationIsAt("3/3","navigation should be at last stored item");
		}
	}
}
