// Copyright 2019 Richard Nusser
// Licensed under GPLv3 (see http://www.gnu.org/licenses/)

using System;
using Bulkr.Gui.Components;
using Bulkr.Gui.Utils;

public partial class MainWindow : ApplicationWindow
{
	private FoodComponent FoodComponent { get; set; }


	private void AddFoodComponent()
	{
		FoodComponent=new FoodComponent(this);
	}


	protected void OnFoodNewClicked(object sender,EventArgs e)
	{
		FoodComponent.New();
	}

	protected void OnFoodRevertClicked(object sender,EventArgs e)
	{
		FoodComponent.Revert();
	}

	protected void OnFoodSaveClicked(object sender,EventArgs e)
	{
		FoodComponent.Save();
	}

	protected void OnFoodDeleteClicked(object sender,EventArgs e)
	{
		FoodComponent.Delete();
	}

	protected void OnNavForwardClicked(object sender,EventArgs e)
	{
		FoodComponent.Next();
	}

	protected void OnNavBackClicked(object sender,EventArgs e)
	{
		FoodComponent.Previous();
	}
}
