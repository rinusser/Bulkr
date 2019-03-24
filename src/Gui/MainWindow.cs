// Copyright 2019 Richard Nusser
// Licensed under GPLv3 (see http://www.gnu.org/licenses/)

using System;
using Gtk;

using Bulkr.Core.Models;
using Bulkr.Core.Services;


public partial class MainWindow : Gtk.Window
{
	public MainWindow() : base(Gtk.WindowType.Toplevel)
	{
		Build();
	}

	protected void OnDeleteEvent(object sender, DeleteEventArgs a)
	{
		Application.Quit();
		a.RetVal=true;
	}

	protected void OnButton1Clicked(object sender,EventArgs e)
	{
		var foodService=FoodService.CreatePersistentInstance();
		foodService.Add(new Food());
		int count=foodService.GetAll().Count;
		textview1.Buffer.Text+=string.Format("added food item to database, there are now {0} entries\n",count);
	}
}
