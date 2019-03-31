// Copyright 2019 Richard Nusser
// Licensed under GPLv3 (see http://www.gnu.org/licenses/)

using Bulkr.Core.Models;
using Bulkr.Core.Services;
using Bulkr.Gui.Forms;
using Bulkr.Gui.Forms.Field;
using Bulkr.Gui.Utils;

namespace Bulkr.Gui.Components
{
	public abstract class Component<MODEL> where MODEL : Model, new()
	{
		protected ApplicationWindow Window { get; }
		protected Form<MODEL> Form { get; set; }
		protected MODEL CurrentItem { get; set; }
		protected Service<MODEL> Service { get; set; }
		protected int CurrentEntryNumber { get; set; }
		protected int TotalCount { get; set; }
		private string WidgetNamePrefix { get; }


		protected Component(ApplicationWindow window)
		{
			Window=window;
			CurrentItem=new MODEL();
			WidgetNamePrefix=typeof(MODEL).Name.ToLower()+"_";
			Form=CreateForm();
			Service=CreateService();
			UpdateTotalCount();
			NavTo(1);
		}


		protected abstract Form<MODEL> CreateForm();
		protected abstract Service<MODEL> CreateService();


		public void New()
		{
			if(CurrentItem.ID>0||TotalCount<1)
				TotalCount++;
			CurrentItem=new MODEL();
			CurrentEntryNumber=TotalCount;
			UpdateNavInfo();
			Form.Populate(null);
		}

		public void Revert()
		{
			if(CurrentItem.ID>0)
			{
				CurrentItem=Service.GetByID(CurrentItem.ID);
				PopulateWithCurrentItem();
			}
			else
			{
				CurrentItem=new MODEL();
				Form.Populate(null);
			}
		}

		public void Save()
		{
			try
			{
				MODEL item=Form.ParseInto(new MODEL()); //this triggers the exception handler if there are any errors
				item=Form.ParseInto(CurrentItem); //if validation succeeded: parse data into (tracked) entity

				if(CurrentItem.ID<1)
				{
					CurrentItem=Service.Add(item);
					UpdateTotalCount();
					Log(string.Format("added item ID={0}, there are now {1} entries",CurrentItem.ID,TotalCount));
				}
				else
				{
					item.ID=CurrentItem.ID;
					CurrentItem=Service.Update(item);
					Log(string.Format("updated item ID={0}",CurrentItem.ID));
				}
				NavTo(CurrentEntryNumber);
			}
			catch(InputException ex)
			{
				Log("error adding item: "+ex.Message);
			}
		}

		public void Delete()
		{
			if(CurrentItem.ID>0)
			{
				var id=CurrentItem.ID;
				Service.Delete(CurrentItem);
				UpdateTotalCount();
				Log(string.Format("deleted item ID={0}, there are now {1} entries",id,TotalCount));
			}
			NavTo(CurrentEntryNumber);
		}

		public void Next()
		{
			NavTo(CurrentEntryNumber+1);
		}

		public void Previous()
		{
			NavTo(CurrentEntryNumber-1);
		}


		public void NavTo(int number)
		{
			var items=Service.GetAll();

			if(number>items.Count)
				number=items.Count;
			if(number<1&&items.Count>0)
				number=1;
			if(items.Count>0)
			{
				CurrentItem=items[number-1];
			}
			else
			{
				CurrentItem=new MODEL();
				number=0;
			}
			TotalCount=items.Count;
			CurrentEntryNumber=number;
			UpdateNavInfo();
			PopulateWithCurrentItem();
		}

		protected void Log(string message)
		{
			string prefix=typeof(MODEL).Name;
			Window.AddLogEntry(string.Format("{0}: {1}",prefix,message));
		}

		protected void UpdateNavInfo()
		{
			GetFieldLabelWidget("nav").Text=string.Format("{0}/{1}",CurrentEntryNumber,TotalCount);
		}

		protected void PopulateWithCurrentItem()
		{
			Form.Populate(CurrentItem);
		}

		protected void UpdateTotalCount()
		{
			TotalCount=Service.GetAll().Count;
			if(CurrentEntryNumber>TotalCount)
				CurrentEntryNumber=TotalCount;
		}


		protected virtual Gtk.Label GetFieldLabelWidget(string field)
		{
			return (Gtk.Label)Window.GetWidget(WidgetNamePrefix+field.ToLower()+"_label");
		}

		protected Gtk.Widget GetFieldValueWidget(string field)
		{
			return Window.GetWidget(WidgetNamePrefix+field.ToLower()+"_value");
		}

		protected Gtk.Label GetFieldValueLabel(string field)
		{
			return (Gtk.Label)GetFieldValueWidget(field);
		}

		protected Gtk.Entry GetFieldValueInput(string field)
		{
			return (Gtk.Entry)GetFieldValueWidget(field);
		}

		protected Gtk.ComboBox GetFieldValueComboBox(string field)
		{
			return (Gtk.ComboBox)GetFieldValueWidget(field);
		}
	}
}