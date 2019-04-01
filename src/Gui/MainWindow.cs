// Copyright 2019 Richard Nusser
// Licensed under GPLv3 (see http://www.gnu.org/licenses/)

using System;
using Gtk;

using Bulkr.Gui.Utils;

/// <summary>
///   Main window for the application.
/// </summary>
public partial class MainWindow : Gtk.Window, WidgetFinder, WidgetLogger
{
	/// <summary>
	///   Default constructor, gets invoked by Gtk.
	/// </summary>
	public MainWindow() : base(Gtk.WindowType.Toplevel)
	{
		Build();
		AddFoodComponent();
	}


	/// <summary>
	///   Gtk signal handler for closing main window.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="a"></param>
	protected void OnDeleteEvent(object sender,DeleteEventArgs a)
	{
		Application.Quit();
		a.RetVal=true;
	}

	/// <summary>
	///   Fetches a Gtk.Widget type field by name.
	/// </summary>
	/// <param name="name">The field name to look up.</param>
	/// <returns>The widget.</returns>
	public Gtk.Widget GetWidget(string name)
	{
		return WidgetUtils.GetWidgetFieldByName(this,name);
	}

	/// <summary>
	///   Adds a message to the application's log output.
	///   <para>
	///     The current timestamp will be added to the message automatically.
	///   </para>
	/// </summary>
	/// <param name="message">The log message.</param>
	public void AddLogEntry(string message)
	{
		string timestamp=DateTime.Now.ToString("[yyyy-MM-dd HH:mm:ss] ");
		textview1.Buffer.Text=textview1.Buffer.Text+timestamp+message+"\n";
	}
}
