// Copyright 2019 Richard Nusser
// Licensed under GPLv3 (see http://www.gnu.org/licenses/)

using System;
using System.Collections.Generic;
using Bulkr.Core.Services;
using Bulkr.Gui.Forms.Field;
using Bulkr.Gui.Components;
using Bulkr.Gui.Forms;

namespace Bulkr.Gui_Tests.Components
{
	public class TargetComponent : Component<TargetModel>
	{
		//XXX could put these into fake window instead
		public Gtk.Label ID;
		public Gtk.Entry RequiredString;
		public Gtk.Entry OptionalString;
		public Gtk.Entry RequiredFloat;
		public Gtk.Entry OptionalFloat;
		public Gtk.ComboBox RequiredEnum;
		public Gtk.ComboBox OptionalEnum;
		public Gtk.Label NavLabel;


		public TargetComponent() : base(null)
		{
		}

		private void CreateWidgets() //XXX doc: virtual call in parent constructor requires this
		{
			ID=new Gtk.Label();
			RequiredString=new Gtk.Entry();
			OptionalString=new Gtk.Entry();
			RequiredFloat=new Gtk.Entry();
			OptionalFloat=new Gtk.Entry();
			RequiredEnum=new Gtk.ComboBox();
			OptionalEnum=new Gtk.ComboBox();
			NavLabel=new Gtk.Label();
		}


		protected override Form<TargetModel> CreateForm()
		{
			CreateWidgets();

			var enumOptions=new Dictionary<TargetEnum,string>
			{
				{ TargetEnum.One,"oNe" },
				{ TargetEnum.Two,"twO" }
			};

			return new Form<TargetModel>()
				.AddField(new ID("ID",ID))
				.AddField(new Text("RequiredString",RequiredString,Text.Option.Required))
				.AddField(new Text("OptionalString",OptionalString))
				.AddField(new Number("RequiredFloat",RequiredFloat))
				.AddField(new Number("OptionalFloat",OptionalFloat))
				.AddField(new DropDown<TargetEnum>("RequiredEnum",RequiredEnum,enumOptions))
				.AddField(new DropDown<TargetEnum>("OptionalEnum",OptionalEnum,enumOptions));
		}

		protected override Service<TargetModel> CreateService()
		{
			return TargetService.Create(NUnit.Framework.TestContext.CurrentContext.Test.FullName);
		}


		protected override Gtk.Label GetFieldLabelWidget(string name)
		{
			switch(name)
			{
				case "nav":
					return NavLabel;
				default:
					throw new NotImplementedException("unhandled widget name: "+name);
			}
		}
	}
}
