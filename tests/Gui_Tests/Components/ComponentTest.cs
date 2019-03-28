// Copyright 2019 Richard Nusser
// Licensed under GPLv3 (see http://www.gnu.org/licenses/)

using NUnit.Framework;
using System;

namespace Bulkr.Gui_Tests.Components
{
	[TestFixture]
	public class ComponentTest
	{
		private TargetComponent Component;
		private TargetService Service;

		[SetUp]
		public void SetUp()
		{
			string testname=TestContext.CurrentContext.Test.FullName;
			Component=new TargetComponent();
			Service=TargetService.Create(testname);
		}

		[Test]
		public void TestClickingNewShowsCorrectValues()
		{
			Component.ID.Text="changeme";
			Component.NavLabel.Text="changeme";
			Component.New();
			Assert.AreEqual("",Component.ID.Text);
			Assert.AreEqual("1/1",Component.NavLabel.Text);
			//TODO: check other fields
			Component.New();
			Assert.AreEqual("1/1",Component.NavLabel.Text);
		}

		[Test]
		public void TestClickingNewTwiceShowsCorrectNavigation()
		{
			var preexisting=new TargetModel
			{
				RequiredString="stringy",
				RequiredFloat=0.123F,
				RequiredEnum=TargetEnum.One
			};
			Service.Add(preexisting);
			Component.Previous(); //this reloads the list of entries so the component sees our manually added item
			Component.New();
			Assert.AreEqual("2/2",Component.NavLabel.Text);
			Component.New();
			Assert.AreEqual("2/2",Component.NavLabel.Text);
		}
	}
}
