// Copyright 2019 Richard Nusser
// Licensed under GPLv3 (see http://www.gnu.org/licenses/)

using System;
using Gtk;

using Bulkr.Core.Models;
using Bulkr.Core.Services;
using Bulkr.Gui.Form.Element;

public partial class MainWindow : Gtk.Window
{
	protected FoodService FoodService { get; }
	protected Food CurrentFood { get; set; }
	protected int CurrentEntryNumber { get; set; }
	protected int TotalCount { get; set; }


	public MainWindow() : base(Gtk.WindowType.Toplevel)
	{
		FoodService=FoodService.CreatePersistentInstance();
		UpdateTotalCount();
		CurrentEntryNumber=TotalCount>0 ? 1 : 0;
		CurrentFood=new Food();
		Build();
		NavTo(1);
	}


	protected void OnDeleteEvent(object sender,DeleteEventArgs a)
	{
		Application.Quit();
		a.RetVal=true;
	}

	protected void OnNavForwardClicked(object sender,EventArgs e)
	{
		NavTo(CurrentEntryNumber+1);
	}

	protected void OnNavBackClicked(object sender,EventArgs e)
	{
		NavTo(CurrentEntryNumber-1);
	}

	protected void NavTo(int number)
	{
		var foods=FoodService.GetAll();

		if(number>foods.Count)
			number=foods.Count;
		if(number<1&&foods.Count>0)
			number=1;
		if(foods.Count>0)
		{
			CurrentFood=foods[number-1];
		}
		else
		{
			CurrentFood=new Food();
			number=0;
		}
		TotalCount=foods.Count;
		CurrentEntryNumber=number;
		UpdateNavInfo();
		PopulateCurrentFoodFields();
	}

	protected void UpdateNavInfo()
	{
		food_nav_label.Text=string.Format("{0}/{1}",CurrentEntryNumber,TotalCount);
	}

	protected void PopulateCurrentFoodFields()
	{
		food_id_value.Text=CurrentFood.ID>0 ? CurrentFood.ID.ToString() : "";
		food_name_value.Text=CurrentFood.Name??"";
		food_brand_value.Text=CurrentFood.Brand??"";

		ReferenceSize.SelectAt(food_referencesize_value,CurrentFood.ReferenceSize);

		food_energy_value.Text=NutritionalValue.Format(CurrentFood.Energy);
		food_totalfat_value.Text=NutritionalValue.Format(CurrentFood.TotalFat);
		food_saturatedfat_value.Text=NutritionalValue.Format(CurrentFood.SaturatedFat);
		food_carbohydrates_value.Text=NutritionalValue.Format(CurrentFood.TotalCarbohydrates);
		food_sugar_value.Text=NutritionalValue.Format(CurrentFood.Sugar);
		food_protein_value.Text=NutritionalValue.Format(CurrentFood.Protein);
		food_fiber_value.Text=NutritionalValue.Format(CurrentFood.Fiber);
	}

	protected void OnFoodNewClicked(object sender,EventArgs e)
	{
		if(CurrentFood.ID>0)
			TotalCount++;
		CurrentFood=new Food();
		CurrentEntryNumber=TotalCount;
		UpdateNavInfo();
		PopulateCurrentFoodFields();
	}

	protected void OnFoodRevertClicked(object sender,EventArgs e)
	{
		if(CurrentFood.ID>0)
			CurrentFood=FoodService.GetByID(CurrentFood.ID);
		else
			CurrentFood=new Food();
		PopulateCurrentFoodFields();
	}

	protected void OnFoodSaveClicked(object sender,EventArgs e)
	{
		Food item=CurrentFood;
		try
		{
			item.Name=food_name_value.Text.Trim();
			if(item.Name.Length<1)
				throw new InputException("name","cannot be empty");
			item.Brand=food_brand_value.Text.Trim().Length>0 ? food_brand_value.Text.Trim() : null;
			item.ReferenceSize=ReferenceSize.Parse(food_referencesize_value.ActiveText);
			item.Energy=NutritionalValue.ParseNonNegative("energy",food_energy_value.Text);
			item.TotalFat=NutritionalValue.ParseNonNegativeOrNull("total fat",food_totalfat_value.Text);
			item.SaturatedFat=NutritionalValue.ParseNonNegativeOrNull("saturated fat",food_saturatedfat_value.Text);
			item.TotalCarbohydrates=NutritionalValue.ParseNonNegativeOrNull("total carbohydrates",food_carbohydrates_value.Text);
			item.Sugar=NutritionalValue.ParseNonNegativeOrNull("sugar",food_sugar_value.Text);
			item.Protein=NutritionalValue.ParseNonNegativeOrNull("protein",food_protein_value.Text);
			item.Fiber=NutritionalValue.ParseNonNegativeOrNull("fiber",food_fiber_value.Text);

			if(CurrentFood.ID<1)
			{
				CurrentFood=FoodService.Add(item);
				UpdateTotalCount();
				AddLogEntry(string.Format("added food item ID={0}, there are now {1} entries",CurrentFood.ID,TotalCount));
			}
			else
			{
				item.ID=CurrentFood.ID;
				CurrentFood=FoodService.Update(item);
				AddLogEntry(string.Format("updated food item ID={0}",CurrentFood.ID));
			}
			NavTo(CurrentEntryNumber);
		}
		catch(InputException ex)
		{
			AddLogEntry("error adding food item: "+ex.Message);
		}
	}

	protected void UpdateTotalCount()
	{
		TotalCount=FoodService.GetAll().Count;
		if(CurrentEntryNumber>TotalCount)
			CurrentEntryNumber=TotalCount;
	}

	protected void AddLogEntry(string text)
	{
		string timestamp=DateTime.Now.ToString("[yyyy-MM-dd HH:mm:ss] ");
		textview1.Buffer.Text=textview1.Buffer.Text+timestamp+text+"\n";
	}

	protected void OnFoodDeleteClicked(object sender,EventArgs e)
	{
		if(CurrentFood.ID>0)
		{
			var id=CurrentFood.ID;
			FoodService.Delete(CurrentFood);
			UpdateTotalCount();
			AddLogEntry(string.Format("deleted food item ID={0}, there are now {1} entries",id,TotalCount));
		}
		NavTo(CurrentEntryNumber);
	}
}
