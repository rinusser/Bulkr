// Copyright 2019 Richard Nusser
// Licensed under GPLv3 (see http://www.gnu.org/licenses/)

using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

using Bulkr.Core.Services;
using Bulkr.Core.Models;
using Bulkr.Core;

namespace Bulkr.Core_Tests.Services
{
	[TestFixture]
	public class TallyServiceTest
	{
		protected TallyService TallyService;
		protected FoodService FoodService;
		protected IntakeService IntakeService;

		protected Food Flour;
		protected Food Milk;
		protected Food Eggs;


		[SetUp]
		public void SetUp()
		{
			var testName=TestContext.CurrentContext.Test.FullName;
			TallyService=TallyService.Create(testName);
			FoodService=FoodService.Create(testName);
			IntakeService=IntakeService.Create(testName);

			Flour=FoodService.Add(new Food
			{
				Name="Flour",
				Energy=364,
				ReferenceSize=ReferenceSizeType._100g,
			});

			Milk=FoodService.Add(new Food
			{
				Name="Milk",
				Energy=68,
				ReferenceSize=ReferenceSizeType._100ml,
			});

			Eggs=FoodService.Add(new Food
			{
				Name="Egg",
				Energy=70,
				ReferenceSize=ReferenceSizeType._1pc,
			});

			IntakeService.Add(new Intake
			{
				When=new DateTime(2019,4,8),
				Amount=250,
				Food=Flour,
			});

			IntakeService.Add(new Intake
			{
				When=new DateTime(2019,4,8,12,12,0),
				Amount=500,
				Food=Milk,
			});

			IntakeService.Add(new Intake
			{
				When=new DateTime(2019,4,8,12,12,0),
				Amount=1,
				Food=Eggs,
			});

			IntakeService.Add(new Intake
			{
				When=new DateTime(2019,4,5,1,2,3),
				Amount=200,
				Food=Milk,
			});

			IntakeService.Add(new Intake
			{
				When=new DateTime(2019,4,8,23,59,59),
				Amount=1,
				Food=Eggs,
			});
		}


		[Test]
		public void TestGetAll()
		{
			var actual=TallyService.GetAll();
			Assert.AreEqual(2,actual.Count,"entries should have been grouped into 2 days");

			AssertDate("2019-04-05",actual[0],"earlier day should be listed first");
			Assert.AreEqual(136,actual[0].Energy,"expected 2*68 kcal");

			AssertDate("2019-04-08",actual[1],null);
			Assert.AreEqual(1390,actual[1].Energy,"expected 2.5*364 + 5*68 + 2*70 kcal");
		}

		[Test]
		public void TestGet1DayAverages()
		{
			var actual=TallyService.GetAverages(1);
			Assert.AreEqual(4,actual.Count,"should have gotten 4 days' worth of averages");

			AssertDate("2019-04-05",actual[0],"should start at first day with data");
			Assert.AreEqual(136,actual[0].Energy,"1-day average should equal daily tallies");

			AssertDate("2019-04-06",actual[1],"day without data should still have been inserted");
			Assert.AreEqual(0,actual[1].Energy,"day without data should be at 0");

			AssertDate("2019-04-07",actual[2],"day without data should still have been inserted");
			Assert.AreEqual(0,actual[2].Energy,"day without data should be at 0");

			AssertDate("2019-04-08",actual[3],null);
			Assert.AreEqual(1390,actual[3].Energy,"1-day average should equal daily tallies");
		}

		[Test]
		public void TestGet3DayAverages()
		{
			IntakeService.Add(new Intake { When=new DateTime(2019,4,4,1,2,3),Amount=1,Food=Eggs });
			IntakeService.Add(new Intake { When=new DateTime(2019,4,6,9,8,7),Amount=2,Food=Eggs });

			var actual=TallyService.GetAverages(3);
			Assert.AreEqual(5,actual.Count,"should have gotten 4 days' worth of averages");

			AssertDate("2019-04-04",actual[0],"should start at first day with data");

			AssertDate("2019-04-05",actual[1],null);

			AssertDate("2019-04-06",actual[2],null);
			Assert.AreEqual(115.333333F,actual[2].Energy,"expected (1*70 + 2*68 + 2*70)/3");

			AssertDate("2019-04-07",actual[3],"day without data should still have been inserted");
			Assert.AreEqual(92,actual[3].Energy,"expected (2*68 + 2*70 + 0)/3");

			AssertDate("2019-04-08",actual[4],null);
			Assert.AreEqual(510,actual[4].Energy,"expected (2*70 + 0 + 1390)/3");
		}

		[Test]
		public void TestGetAveragesInRanges()
		{
			var dates=Enumerable.Range(2,9).Select(i => string.Format("2019-04-{0:00}",i)).ToArray();
			AssertDates(dates,0,6,TallyService.GetAverages(1,new DateTime(2019,4,2)));
			AssertDates(dates,0,7,TallyService.GetAverages(1,new DateTime(2019,4,2),new DateTime(2019,4,9)));
			AssertDates(dates,4,4,TallyService.GetAverages(1,new DateTime(2019,4,6),new DateTime(2019,4,6)));
			AssertDates(dates,3,4,TallyService.GetAverages(1,null,new DateTime(2019,4,6)));
		}


		protected void AssertDate(string expected,Tally subject,string message)
		{
			Assert.AreEqual(expected,subject.When.ToString("yyyy-MM-dd"),message);
		}

		public void AssertDates(string[] dates,int first,int last,IList<Tally> subjects)
		{
			Assert.AreEqual(new ArraySegment<string>(dates,first,last-first+1).ToList(),subjects.Select(s => s.When.ToString("yyyy-MM-dd")).ToList());
		}
	}
}
