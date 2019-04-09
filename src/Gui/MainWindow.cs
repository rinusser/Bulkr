// Copyright 2019 Richard Nusser
// Licensed under GPLv3 (see http://www.gnu.org/licenses/)

using System;
using Gtk;

using Bulkr.Gui.Utils;

/// <summary>
///   Main window for the application.
/// </summary>
public partial class MainWindow : Gtk.Window, ApplicationWindow
{
	/// <summary>
	///   Default constructor, gets invoked by Gtk.
	/// </summary>
	public MainWindow() : base(Gtk.WindowType.Toplevel)
	{
		Build();
		AddLogEntry("finished building main window");
		AddFoodComponent();
		AddIntakeComponent();
		AddAnalysisComponent();
		AddLogEntry("finished loading UI components");
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
		textview1.ScrollToIter(textview1.Buffer.EndIter,0,false,0,0);
	}

	/// <summary>
	///   GTK event handler for when another tab was selected.
	/// </summary>
	/// <param name="o">O.</param>
	/// <param name="args">Arguments.</param>
	protected void OnTabChanged(object o,SwitchPageArgs args)
	{
		switch(args.PageNum) //TODO find a better way that doesn't require tab index or name
		{
			case 1:
				IntakeComponent.Reload();
				break;
			case 2:
				AnalysisComponent.Reload();
				break;
		}
	}
}
