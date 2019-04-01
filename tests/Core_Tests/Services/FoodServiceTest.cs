// Copyright 2019 Richard Nusser
// Licensed under GPLv3 (see http://www.gnu.org/licenses/)

using NUnit.Framework;

using Bulkr.Core.Models;
using Bulkr.Core.Persistence;
using Bulkr.Core.Services;

namespace Bulkr.Core_Tests.Services
{
	[TestFixture]
	public class FoodServiceTest
	{
		protected FoodService service;

		protected FoodService CreateServiceInstanceForCurrentTest()
		{
			return FoodService.Create(TestContext.CurrentContext.Test.FullName);
		}

		[SetUp]
		public void SetUp()
		{
			service=CreateServiceInstanceForCurrentTest();
		}

		[Test]
		public void TestAddingOneWorks()
		{
			RunAddOneTest("first");
		}

		[Test]
		public void TestAddingAnotherIsIsolated()
		{
			RunAddOneTest("second");
		}

		private void RunAddOneTest(string name)
		{
			FoodService otherService=CreateServiceInstanceForCurrentTest();
			Food food=new Food { Name=name };
			Assert.AreEqual(0,otherService.GetAll().Count);
			service.Add(food);
			Assert.AreEqual(1,otherService.GetAll().Count);
			Assert.AreEqual(name,otherService.GetAll()[0].Name);
		}
	}
}
