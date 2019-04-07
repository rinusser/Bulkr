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
		protected static readonly string DROPDOWN_NULL_LABEL=DropDown<TargetModel,TargetEnum,TargetEnum>.NULL_LABEL;


		protected TargetWindow Window;
		protected TargetComponent Component;
		protected TargetService Service;
		protected ReferencedService ReferencedService;
		protected TestCaseFactory TestCaseFactory;


		[SetUp]
		public void SetUp()
		{
			Window=new TargetWindow();
			Service=CreateService();
			ReferencedService=ReferencedService.Create(TestContext.CurrentContext.Test.FullName,(TargetContext)Service.DatabaseContext);
			TestCaseFactory=new TestCaseFactory(ReferencedService);
			Component=new TargetComponent(new TargetComponentContext { Window=Window,ReferencedService=ReferencedService });
		}

		protected TargetService CreateService()
		{
			return TargetService.Create(TestContext.CurrentContext.Test.FullName);
		}


		protected void ClearForm()
		{
			TestCaseFactory.Empty().Enter(Window);
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


		protected List<int> GetStoredIDs()
		{
			return CreateService().GetAll().Select(x => x.ID).ToList();
		}
	}
}
