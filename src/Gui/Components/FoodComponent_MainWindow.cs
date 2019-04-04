// Copyright 2019 Richard Nusser
// Licensed under GPLv3 (see http://www.gnu.org/licenses/)

using System;
using Bulkr.Gui.Components;
using Bulkr.Gui.Utils;

public partial class MainWindow : ApplicationWindow
{
	/// <summary>
	///   The <see cref="FoodComponent"/> instance to use for managing food items.
	/// </summary>
	private FoodComponent FoodComponent { get; set; }


	/// <summary>
	///   Adds <see cref="FoodComponent"/> instance to main window. Called by constructor.
	/// </summary>
	private void AddFoodComponent()
	{
		FoodComponent=new FoodComponent(new ComponentContext { Window=this });
	}


	/// <summary>
	///   Handler for clicking food "New" button
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void OnFoodNewClicked(object sender,EventArgs e)
	{
		FoodComponent.New();
	}

	/// <summary>
	///   Handler for clicking food "Revert" button
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void OnFoodRevertClicked(object sender,EventArgs e)
	{
		FoodComponent.Revert();
	}

	/// <summary>
	///   Handler for clicking food "Save" button
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void OnFoodSaveClicked(object sender,EventArgs e)
	{
		FoodComponent.Save();
	}

	/// <summary>
	///   Handler for clicking food "Delete" button
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void OnFoodDeleteClicked(object sender,EventArgs e)
	{
		FoodComponent.Delete();
	}

	/// <summary>
	///   Handler for clicking food "Forward" button
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void OnNavForwardClicked(object sender,EventArgs e)
	{
		FoodComponent.Next();
	}

	/// <summary>
	///   Handler for clicking food "Back" button
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void OnNavBackClicked(object sender,EventArgs e)
	{
		FoodComponent.Previous();
	}
}
