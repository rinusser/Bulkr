// Copyright 2019 Richard Nusser
// Licensed under GPLv3 (see http://www.gnu.org/licenses/)

using System.Collections.Generic;

using Bulkr.Gui.Utils;

namespace Bulkr.Gui_Tests.TestTargets
{
	public class TargetWindow : ApplicationWindow
	{
		public static readonly string PUBLIC_NAME="PuBlic";
		public static readonly string PROTECTED_NAME="prOtecteD";
		public static readonly string INTERNAL_NAME="internAl";
		public static readonly string PRIVATE_NAME="pRivaTe";

		public static readonly string UNINITIALIZED_TEXT="uninitialized";


		public Gtk.Label targetmodel_id_value;
		public Gtk.Entry targetmodel_requiredstring_value;
		public Gtk.Entry targetmodel_optionalstring_value;
		public Gtk.Entry targetmodel_requiredfloat_value;
		public Gtk.Entry targetmodel_optionalfloat_value;
		public Gtk.ComboBox targetmodel_requiredenum_value;
		public Gtk.ComboBox targetmodel_optionalenum_value;
		public Gtk.Label targetmodel_nav_label;

		private Gtk.Widget PublicWidget;
		protected Gtk.Widget ProtectedWidget;
		internal Gtk.Widget InternalWidget;
		private Gtk.Widget PrivateWidget;

		public IList<string> LogMessages { get; set; }


		public TargetWindow()
		{
			targetmodel_id_value=new Gtk.Label { Text=UNINITIALIZED_TEXT };
			targetmodel_requiredstring_value=new Gtk.Entry { Text=UNINITIALIZED_TEXT };
			targetmodel_optionalstring_value=new Gtk.Entry { Text=UNINITIALIZED_TEXT };
			targetmodel_requiredfloat_value=new Gtk.Entry { Text=UNINITIALIZED_TEXT };
			targetmodel_optionalfloat_value=new Gtk.Entry { Text=UNINITIALIZED_TEXT };
			targetmodel_requiredenum_value=new Gtk.ComboBox();
			targetmodel_optionalenum_value=new Gtk.ComboBox();
			targetmodel_nav_label=new Gtk.Label { Text=UNINITIALIZED_TEXT };

			LogMessages=new List<string>();
		}


		public void InitializeOptionalWidgets()
		{
			PublicWidget=new Gtk.Label { Name=PUBLIC_NAME };
			ProtectedWidget=new Gtk.Label { Name=PROTECTED_NAME };
			InternalWidget=new Gtk.Label { Name=INTERNAL_NAME };
			PrivateWidget=new Gtk.Label { Name=PRIVATE_NAME };
		}


		public void AddLogEntry(string text)
		{
			LogMessages.Add(text);
		}

		public Gtk.Widget GetWidget(string name)
		{
			return WidgetUtils.GetWidgetFieldByName(this,name);
		}
	}
}