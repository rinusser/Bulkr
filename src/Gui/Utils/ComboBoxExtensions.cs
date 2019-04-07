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
		public static void ForEach(this Gtk.ComboBox comboBox,Func<Gtk.TreeIter,GLib.Value,GLib.Value,GLib.Value,bool> callback)
		{
			Gtk.TreeIter iter;
			comboBox.Model.GetIterFirst(out iter);
			do
			{
				GLib.Value id=new GLib.Value();
				GLib.Value model=new GLib.Value();
				GLib.Value label=new GLib.Value();
				comboBox.Model.GetValue(iter,0,ref id);
				comboBox.Model.GetValue(iter,1,ref model);
				comboBox.Model.GetValue(iter,2,ref label);
				if(!callback(iter,id,model,label))
					return;
			}
			while(comboBox.Model.IterNext(ref iter));
		}

		/// <summary>
		///   Selects a specific entry by ID in a ComboBox.
		/// </summary>
		/// <param name="comboBox">The ComboBox to select the ID in.</param>
		/// <param name="wanted_id">The ID to select.</param>
		/// <param name="ignoreMissing">Whether to ignore IDs that can't be found.</param>
		/// <exception cref="ArgumentException">if the ID wasn't found in the dropdown and ignoreMissing=false.</exception>
		public static void SelectID<T>(this Gtk.ComboBox comboBox,T? wanted_id,bool ignoreMissing = false) where T : struct
		{
			bool found=false;
			ForEach(comboBox,(iter,packedID,model,label) =>
			{
				if(wanted_id==null^packedID.Val==null)
					return true;

				if(wanted_id!=null)
				{
					T? id=UnpackID<T>(packedID);
					if(!id.Equals(wanted_id))
						return true;
				}

				comboBox.SetActiveIter(iter);
				found=true;
				return false;
			});

			if(!found&&!ignoreMissing)
				throw new ArgumentException(string.Format("ID '{0}' not found in ComboBox '{1}'",wanted_id,comboBox.Name));
		}

		/// <summary>
		///   Returns the currently selected entry's ID.
		/// </summary>
		/// <param name="comboBox">The ComboBox to check.</param>
		/// <returns>The currently selected entry's ID, or <c>null</c>.</returns>
		/// <typeparam name="T">The ID's (non-nullable) data type.</typeparam>
		public static T? GetActiveID<T>(this Gtk.ComboBox comboBox) where T : struct
		{
			return UnpackID<T>(comboBox.GetActiveNthColumn<object>(0));
		}

		/// <summary>
		///   Returns the currently selected entry's model.
		/// </summary>
		/// <param name="comboBox">The ComboBox to check.</param>
		/// <returns>The currently selected entry's model, or <c>null</c>.</returns>
		public static object GetActiveModel(this Gtk.ComboBox comboBox)
		{
			return comboBox.GetActiveNthColumn<object>(1);
		}

		/// <summary>
		///   Returns the currently selected entry's label.
		/// </summary>
		/// <param name="comboBox">The ComboBox to check.</param>
		/// <returns>The currently selected entry's label. Can return <c>null</c>, but this shouldn't happen.</returns>
		public static string GetActiveLabel(this Gtk.ComboBox comboBox)
		{
			return comboBox.GetActiveNthColumn<string>(2);
		}


		/// <summary>
		///   Returns the currently selected entry's nth column from its internal store.
		/// </summary>
		/// <param name="comboBox">The ComboBox to check.</param>
		/// <param name="column">The 0-based column index.</param>
		/// <returns>The column's content.</returns>
		/// <typeparam name="T">What type to cast the content to - make sure it's nullable if you expect <c>null</c> values.</typeparam>
		private static T GetActiveNthColumn<T>(this Gtk.ComboBox comboBox,int column)
		{
			if(comboBox.Model==null)
				return default(T);

			Gtk.TreeIter iter;
			comboBox.GetActiveIter(out iter);
			GLib.Value current=new GLib.Value();
			comboBox.Model.GetValue(iter,column,ref current);
			return (T)current.Val;
		}

		/// <summary>
		///   Fetches the ID from a GTK list/tree store value.
		/// </summary>
		/// <remarks>
		///   GTK needs to have primitive values wrapped in e.g. <see cref="GLib.Value"/>. Currently enum- and model-based
		///   dropdowns generate differently nested IDs in the ComboBox's list store, this method unwraps either and returns
		///   the primitive ID type.
		/// </remarks>
		/// <remarks>
		///   This method is recursive and doesn't check for loops: it will crash if you pass e.g. <code>A.Val=B; B.Val=A;</code>.
		/// </remarks>
		/// <param name="id">The ID to unwrap.</param>
		/// <returns>The primitive ID, without any <see cref="GLib.Value"/> wrappers.</returns>
		/// <typeparam name="T">The ID data type. If your ID can be <c>null</c>, make sure this type is nullable.</typeparam>
		/// <exception cref="Exception">
		///   If anything other than <typeparamref name="T"/>, <see cref="GLib.Value"/> or <c>null</c> was encountered.
		/// </exception>
		private static T UnpackID<T>(object id)
		{
			if(id==null)
				return default(T);

			if(id.GetType().IsAssignableFrom(typeof(T)))
				return (T)id;

			if(id.GetType().IsAssignableFrom(typeof(GLib.Value)))
				return UnpackID<T>(((GLib.Value)id).Val);

			throw new Exception("invalid value for ID: expected null, id type or GLib.Value");
		}
	}
}
