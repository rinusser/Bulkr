// Copyright 2019 Richard Nusser
// Licensed under GPLv3 (see http://www.gnu.org/licenses/)

using NUnit.Framework;

using Bulkr.Gui_Tests.TestTargets;
using Bulkr.Gui.Utils;

namespace Bulkr.Gui_Tests.Utils
{
	[TestFixture]
	public class WidgetUtilsTest
	{
		[Test]
		public void TestCanFindWidgetsOfAllVisibilities()
		{
			var window=new TargetWindow();
			window.InitializeOptionalWidgets();
			Assert.AreEqual(TargetWindow.PUBLIC_NAME,WidgetUtils.GetWidgetFieldByName(window,"PublicWidget").Name);
			Assert.AreEqual(TargetWindow.PROTECTED_NAME,WidgetUtils.GetWidgetFieldByName(window,"ProtectedWidget").Name);
			Assert.AreEqual(TargetWindow.INTERNAL_NAME,WidgetUtils.GetWidgetFieldByName(window,"InternalWidget").Name);
			Assert.AreEqual(TargetWindow.PRIVATE_NAME,WidgetUtils.GetWidgetFieldByName(window,"PrivateWidget").Name);
		}
	}
}
