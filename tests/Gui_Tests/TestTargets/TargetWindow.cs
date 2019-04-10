// Copyright 2019 Richard Nusser
// Licensed under GPLv3 (see http://www.gnu.org/licenses/)

using System.Collections.Generic;
using System.Reflection;

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


		public Gtk.Label targetmodel_requiredstring_label;
		public Gtk.Entry targetmodel_requiredstring_value;
		public Gtk.Label targetmodel_optionalstring_label;
		public Gtk.Entry targetmodel_optionalstring_value;

		public Gtk.Entry targetmodel_requiredfloat_value;
		public Gtk.Label targetmodel_requiredfloat_label;
		public Gtk.Entry targetmodel_optionalfloat_value;
		public Gtk.Label targetmodel_optionalfloat_label;

		public Gtk.ComboBox targetmodel_requiredenum_value;
		public Gtk.Label targetmodel_requiredenum_label;
		public Gtk.ComboBox targetmodel_optionalenum_value;
		public Gtk.Label targetmodel_optionalenum_label;

		public Gtk.ComboBox targetmodel_requiredservicedropdown_value;
		public Gtk.Label targetmodel_requiredservicedropdown_label;
		public Gtk.ComboBox targetmodel_optionalservicedropdown_value;
		public Gtk.Label targetmodel_optionalservicedropdown_label;

		public Gtk.Calendar targetmodel_requireddatetime_date_value;
		public Gtk.SpinButton targetmodel_requireddatetime_hour_value;
		public Gtk.SpinButton targetmodel_requireddatetime_minute_value;

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

			targetmodel_requiredenum_value=CreateComboBox();
			targetmodel_optionalenum_value=CreateComboBox();

			targetmodel_requiredservicedropdown_value=CreateComboBox();
			targetmodel_optionalservicedropdown_value=CreateComboBox();

			targetmodel_requireddatetime_date_value=new Gtk.Calendar();
			targetmodel_requireddatetime_hour_value=new Gtk.SpinButton(0,23,1);
			targetmodel_requireddatetime_minute_value=new Gtk.SpinButton(0,59,1);

			SetUpLabels();

			LogMessages=new List<string>();
		}

		private void SetUpLabels()
		{
			foreach(var field in GetType().GetFields(BindingFlags.Instance|BindingFlags.Public|BindingFlags.NonPublic))
				if(field.Name.EndsWith("_label",System.StringComparison.Ordinal))
					field.SetValue(this,new Gtk.Label { Text=UNINITIALIZED_TEXT });
		}

		private Gtk.ComboBox CreateComboBox()
		{
			var comboBox=new Gtk.ComboBox();
			var cellRenderer=new Gtk.CellRendererText();
			comboBox.PackStart(cellRenderer,true);
			comboBox.AddAttribute(cellRenderer,"text",2);
			return comboBox;
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


		public Gtk.Label GetLabel(string fieldName)
		{
			return (Gtk.Label)GetType().GetField(string.Format("targetmodel_{0}_label",fieldName.ToLower())).GetValue(this);
		}
	}
}