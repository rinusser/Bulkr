// Copyright 2019 Richard Nusser
// Licensed under GPLv3 (see http://www.gnu.org/licenses/)

using System;

namespace Bulkr.Gui.Utils
{
	/// <summary>
	///   Extension methods to <c>Gtk.ComboBox</c>.
	/// </summary>
	public static class ComboBoxExtensions
	{
		/// <summary>
		///   Iterates over a Gtk ComboBox's items and executes the callback for each item.
		///   <para>
		///     Callbacks should return <c>true</c> if the loop should continue after executing the callback, or
		///     <c>false</c> if the loop should break.
		///   </para>
		/// </summary>
		/// <param name="comboBox">The ComboBox to iterate over.</param>
		/// <param name="callback">The callback function.</param>
		public static void ForEach(this Gtk.ComboBox comboBox,Func<GLib.Value,Gtk.TreeIter,bool> callback)
		{
			Gtk.TreeIter iter;
			comboBox.Model.GetIterFirst(out iter);
			do
			{
				GLib.Value current=new GLib.Value();
				comboBox.Model.GetValue(iter,0,ref current);
				if(!callback(current,iter))
					return;
			}
			while(comboBox.Model.IterNext(ref iter));
		}


		/// <summary>
		///   Selects a specific label in a ComboBox.
		/// </summary>
		/// <param name="comboBox">The ComboBox to select the label in.</param>
		/// <param name="label">The label to select.</param>
		/// <param name="ignoreMissing">Whether to ignore labels that can't be found.</param>
		/// <exception cref="ArgumentException">if the label wasn't found in the dropdown.</exception>
		public static void SelectLabel(this Gtk.ComboBox comboBox,string label,bool ignoreMissing = false)
		{
			bool found=false;
			ForEach(comboBox,(entry,iter) =>
			{
				if((string)entry.Val!=label)
					return true;
				comboBox.SetActiveIter(iter);
				found=true;
				return false;
			});

			if(!found&&!ignoreMissing)
				throw new ArgumentException(string.Format("label '{0}' not found in ComboBox '{1}'",label,comboBox.Name));
		}
	}
}
