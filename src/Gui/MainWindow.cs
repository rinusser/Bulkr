// Copyright 2019 Richard Nusser
// Licensed under GPLv3 (see http://www.gnu.org/licenses/)

using System;
using System.Reflection;
using Gtk;

public partial class MainWindow : Gtk.Window
{
	public MainWindow() : base(Gtk.WindowType.Toplevel)
	{
		Build();
		AddFoodComponent();
	}


	protected void OnDeleteEvent(object sender,DeleteEventArgs a)
	{
		Application.Quit();
		a.RetVal=true;
	}

	public Gtk.Widget GetWidget(string name)
	{
		return (Gtk.Widget)GetType().GetField(name,BindingFlags.Instance|BindingFlags.Public|BindingFlags.NonPublic).GetValue(this);
	}

	public void AddLogEntry(string text)
	{
		string timestamp=DateTime.Now.ToString("[yyyy-MM-dd HH:mm:ss] ");
		textview1.Buffer.Text=textview1.Buffer.Text+timestamp+text+"\n";
	}
}
