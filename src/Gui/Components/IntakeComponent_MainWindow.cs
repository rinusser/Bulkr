﻿// Copyright 2019 Richard Nusser
// Licensed under GPLv3 (see http://www.gnu.org/licenses/)

using System;

using Bulkr.Core;
using Bulkr.Core.Models;
using Bulkr.Core.Services;
using Bulkr.Gui.Components;
using Bulkr.Gui.Utils;

public partial class MainWindow : ApplicationWindow
{
	/// <summary>
	///   The <see cref="IntakeComponent"/> instance to use for managing intake entries.
	/// </summary>
	private IntakeComponent IntakeComponent { get; set; }


	/// <summary>
	///   Adds <see cref="IntakeComponent"/> instance to main window. Called by constructor.
	/// </summary>
	private void AddIntakeComponent()
	{
		IntakeComponent=new IntakeComponent(new IntakeComponentContext { Window=this,FoodService=FoodService.Create() });

		intake_layout_table.HeightRequest=food_layout_table.Allocation.Height-3;  //TODO: see if -3 can be calculated instead

		var cellRenderer=(Gtk.CellRendererText)intake_food_value.Cells[0];
		cellRenderer.WidthChars=20;
		cellRenderer.Ellipsize=Pango.EllipsizeMode.End;
	}


	/// <summary>
	///   Handler for clicking intake "New" button
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void OnIntakeNewClicked(object sender,EventArgs e)
	{
		IntakeComponent.New();
	}

	/// <summary>
	///   Handler for clicking intake "Revert" button
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void OnIntakeRevertClicked(object sender,EventArgs e)
	{
		IntakeComponent.Revert();
	}

	/// <summary>
	///   Handler for clicking intake "Save" button
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void OnIntakeSaveClicked(object sender,EventArgs e)
	{
		IntakeComponent.Save();
	}

	/// <summary>
	///   Handler for clicking intake "Delete" button
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void OnIntakeDeleteClicked(object sender,EventArgs e)
	{
		IntakeComponent.Delete();
	}

	/// <summary>
	///   Handler for clicking intake "Forward" button
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void OnIntakeNavForwardClicked(object sender,EventArgs e)
	{
		IntakeComponent.Next();
	}

	/// <summary>
	///   Handler for clicking intake "Back" button
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void OnIntakeNavBackClicked(object sender,EventArgs e)
	{
		IntakeComponent.Previous();
	}

	/// <summary>
	///   Zero-pads hour/minute input widgets' contents.
	///   Gets called automatically whenever a time widget needs to display something.
	/// </summary>
	/// <param name="widget">The widget that needs formatting.</param>
	/// <param name="args">Output values for GTK.</param>
	protected void OnIntakeTimeOutput(object widget,Gtk.OutputArgs args)
	{
		var spinButton=(Gtk.SpinButton)widget;
		spinButton.Text=string.Format("{0:00}",spinButton.ValueAsInt);
		args.RetVal=1;
	}

	/// <summary>
	///   Updates the unit label next to the amount input.
	///   Gets called automatically whenever another entry in the "food" dropdown was selected.
	/// </summary>
	/// <param name="sender">Sender.</param>
	/// <param name="e">E.</param>
	protected void OnIntakeFoodChanged(object sender,EventArgs e)
	{
		var comboBox=(Gtk.ComboBox)sender;
		var referenceSize=((Food)comboBox.GetActiveModel())?.ReferenceSize;
		intake_amount_unit.Text=GetUnitLabelForReferenceSize(referenceSize);
	}

	/// <summary>
	///   Gets the amount unit label for a given reference size.
	/// </summary>
	/// <param name="referenceSize">The reference size.</param>
	/// <returns>The unit label.</returns>
	protected string GetUnitLabelForReferenceSize(ReferenceSizeType? referenceSize)
	{
		switch(referenceSize)
		{
			case ReferenceSizeType._100g:
				return "g";
			case ReferenceSizeType._100ml:
				return "ml";
			case ReferenceSizeType._1pc:
				return "piece(s)";
			case null:
				return "";
		}
		throw new NotImplementedException("unhandled reference size");
	}
}
