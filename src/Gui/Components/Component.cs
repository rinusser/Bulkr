// Copyright 2019 Richard Nusser
// Licensed under GPLv3 (see http://www.gnu.org/licenses/)

using Bulkr.Core.Models;
using Bulkr.Core.Services;
using Bulkr.Gui.Forms;
using Bulkr.Gui.Forms.Field;
using Bulkr.Gui.Utils;

namespace Bulkr.Gui.Components
{
	/// <summary>
	///   Base class for item management interfaces.
	///   <para>
	///     The Gtk widgets need to be created separately, then mapped to model fields in <see cref="CreateForm()"/>.
	///   </para>
	/// </summary>
	public abstract class Component<MODEL> where MODEL : Model, new()
	{
		/// <summary>The application window form widgets are in.</summary>
		protected ApplicationWindow Window { get; }

		/// <summary>The model &lt;-&gt; input widget mapping.</summary>
		protected Form<MODEL> Form { get; set; }

		/// <summary>The currently displayed item.</summary>
		protected MODEL CurrentItem { get; set; }

		/// <summary>The <see cref="T:Bulkr.Core.Services.Service"/> instance for this component.</summary>
		protected Service<MODEL> Service { get; set; }

		/// <summary>The currently displayed item number, starting with 1.</summary>
		protected int CurrentEntryNumber { get; set; }

		/// <summary>The number of items.</summary>
		protected int TotalCount { get; set; }

		/// <summary>The name prefix for this component's widget names.</summary>
		private string WidgetNamePrefix { get; }


		/// <summary>
		///   Base constructor. Subclasses classes generally should invoke this.
		/// </summary>
		/// <param name="window">The application window the form widgets are in.</param>
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


		/// <summary>
		///   Subclasses need to create a <see cref="T:Bulkr.Gui.Forms.Form"/> instance with the widget&lt;-&gt;model mapping here.
		///   <para>
		///     The base constructor will make a virtual method call to this, make sure not to access unitialized class members.
		///   </para>
		/// </summary>
		/// <returns>The Form instance.</returns>
		protected abstract Form<MODEL> CreateForm();

		/// <summary>
		///   Subclasses need to create a <see cref="T:Bulkr.Core.Services.Service"/> instance here.
		///   <para>
		///     The base constructor will make a virtual method call to this, make sure not to access unitialized class members.
		///   </para>
		/// </summary>
		/// <returns>The Form instance.</returns>
		protected abstract Service<MODEL> CreateService();


		/// <summary>
		///   Handler for "New" button: set up everything needed to enter a new item.
		/// </summary>
		public void New()
		{
			if(CurrentItem.ID>0||TotalCount<1)
				TotalCount++;
			CurrentItem=new MODEL();
			CurrentEntryNumber=TotalCount;
			UpdateNavInfo();
			Form.Populate(null);
		}

		/// <summary>
		///   Handler for "Revert" button: restores the input form to the last saved state.
		/// </summary>
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

		/// <summary>
		///   Handler for "Save" button: saves the current item.
		/// </summary>
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

		/// <summary>
		///   Handler for "Delete" button: removes the current item.
		/// </summary>
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

		/// <summary>
		///   Handler for "Next" button: switches to the next item.
		/// </summary>
		public void Next()
		{
			NavTo(CurrentEntryNumber+1);
		}

		/// <summary>
		///   Handler for "Previous" button: switches to the previous item.
		/// </summary>
		public void Previous()
		{
			NavTo(CurrentEntryNumber-1);
		}


		/// <summary>
		///   Switches to the nth item.
		/// </summary>
		/// <param name="number">The item number to switch to, starting at 1.</param>
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

		/// <summary>
		///   Adds a log message.
		///   <para>
		///     The message will include the generic type's name so log entries can be traced back to their origin.
		///   </para>
		/// </summary>
		/// <param name="message">The message to log.</param>
		protected void Log(string message)
		{
			string prefix=typeof(MODEL).Name;
			Window.AddLogEntry(string.Format("{0}: {1}",prefix,message));
		}

		/// <summary>
		///   Updates the navigation display with the internal navigation state.
		/// </summary>
		protected void UpdateNavInfo()
		{
			GetFieldLabelWidget("nav").Text=string.Format("{0}/{1}",CurrentEntryNumber,TotalCount);
		}

		/// <summary>
		///   Fills form fields with the current item's data.
		/// </summary>
		protected void PopulateWithCurrentItem()
		{
			Form.Populate(CurrentItem);
		}

		/// <summary>
		///   Re-reads the number of stored items.
		/// </summary>
		protected void UpdateTotalCount()
		{
			TotalCount=Service.GetAll().Count;
			if(CurrentEntryNumber>TotalCount)
				CurrentEntryNumber=TotalCount;
		}


		/// <summary>
		///   Fetches a label widget from the component's window.
		///   <para>
		///     This only works as long as the widget name follows the standard naming scheme.
		///   </para>
		/// </summary>
		/// <param name="field">The field name to fetch the label for.</param>
		/// <returns>The label widget.</returns>
		protected virtual Gtk.Label GetFieldLabelWidget(string field)
		{
			return (Gtk.Label)Window.GetWidget(WidgetNamePrefix+field.ToLower()+"_label");
		}

		/// <summary>
		///   Fetches a value widget from the component's window.
		///   <para>
		///     This only works as long as the widget name follows the standard naming scheme.
		///   </para>
		/// </summary>
		/// <param name="field">The field name to fetch the value widget for.</param>
		/// <returns>The value widget.</returns>
		protected Gtk.Widget GetFieldValueWidget(string field)
		{
			return Window.GetWidget(WidgetNamePrefix+field.ToLower()+"_value");
		}

		/// <summary>
		///   Like <see cref="GetFieldValueWidget"/>, but cast to <c>Gtk.Label</c>.
		/// </summary>
		/// <param name="field">The field name to fetch the value widget for.</param>
		/// <returns>The Gtk.Label widget.</returns>
		protected Gtk.Label GetFieldValueLabel(string field)
		{
			return (Gtk.Label)GetFieldValueWidget(field);
		}

		/// <summary>
		///   Like <see cref="GetFieldValueWidget"/>, but cast to <c>Gtk.Entry</c>.
		/// </summary>
		/// <param name="field">The field name to fetch the value widget for.</param>
		/// <returns>The Gtk.Entry widget.</returns>
		protected Gtk.Entry GetFieldValueInput(string field)
		{
			return (Gtk.Entry)GetFieldValueWidget(field);
		}

		/// <summary>
		///   Like <see cref="GetFieldValueWidget"/>, but cast to <c>Gtk.ComboBox</c>.
		/// </summary>
		/// <param name="field">The field name to fetch the value widget for.</param>
		/// <returns>The Gtk.ComboBox widget.</returns>
		protected Gtk.ComboBox GetFieldValueComboBox(string field)
		{
			return (Gtk.ComboBox)GetFieldValueWidget(field);
		}
	}
}