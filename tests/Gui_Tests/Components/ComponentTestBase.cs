// Copyright 2019 Richard Nusser
// Licensed under GPLv3 (see http://www.gnu.org/licenses/)

using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

using Bulkr.Gui_Tests.TestTargets;
using Bulkr.Gui.Forms.Field;

namespace Bulkr.Gui_Tests.Components
{
	[TestFixture]
	public class ComponentTestBase
	{
		protected TargetWindow Window;
		protected TargetComponent Component;
		protected TargetService Service;


		[SetUp]
		public void SetUp()
		{
			Window=new TargetWindow();
			Component=new TargetComponent(Window);
			Service=CreateService();
		}

		protected TargetService CreateService()
		{
			return TargetService.Create(TestContext.CurrentContext.Test.FullName);
		}


		protected void ClearForm()
		{
			TestCase.Empty().Enter(Window);
		}


		protected void AssertNavigationIsAt(string position,string message)
		{
			AssertNavigationIsOneOf(new List<string> { position },message);
		}

		protected void AssertNavigationIsOneOf(IList<string> positions,string message)
		{
			if(positions.Count<1)
				throw new System.Exception("need to pass at least one expected position");
			//Assert.That(Window.targetmodel_nav_label.Text,Is.AnyOf(positions));  //requires NUnit 3.x
			message+=string.Format(", but is at {0}",Window.targetmodel_nav_label.Text);
			Assert.True(positions.Contains(Window.targetmodel_nav_label.Text),message);
		}


		protected void AssertNoErrorsInLog()
		{
			foreach(var message in Window.LogMessages)
				Assert.False(message.ToLower().Contains("error"),message);
		}

		protected void AssertHasOneErrorInLog()
		{
			int count=0;
			foreach(var message in Window.LogMessages)
				if(message.ToLower().Contains("error"))
					count++;
			Assert.AreEqual(1,count,"should have exactly 1 error message in log");
		}

		protected List<int> GetStoredIDs()
		{
			return CreateService().GetAll().Select(x => x.ID).ToList();
		}
	}
}
