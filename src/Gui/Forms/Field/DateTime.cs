// Copyright 2019 Richard Nusser
// Licensed under GPLv3 (see http://www.gnu.org/licenses/)

using System;

namespace Bulkr.Gui.Forms.Field
{
	/// <summary>
	///   Field class for date/time input.
	/// </summary>
	/// <remarks>
	///   Optional values are not supported at the moment.
	/// </remarks>
	/// <remarks>
	///   Date-only or time-only input not supported at the moment.
	/// </remarks>
	public class DateTime<MODEL> : AbstractField<MODEL,DateTime?> where MODEL : class
	{
		/// <summary>
		///   The widget for date input.
		/// </summary>
		private Gtk.Calendar CalendarWidget;

		/// <summary>
		///   The widget for hour input.
		/// </summary>
		private Gtk.SpinButton HourWidget;

		/// <summary>
		///   The widget for minute input.
		/// </summary>
		private Gtk.SpinButton MinuteWidget;


		/// <summary>
		///   Constructor, sets value ranges on time input widgets.
		/// </summary>
		/// <param name="propertyName">The model property's name DateTime values should be stored in.</param>
		/// <param name="calendarWidget">The input widget for the date.</param>
		/// <param name="hourWidget">The input widget for the hour.</param>
		/// <param name="minuteWidget">The input widget for the minute.</param>
		public DateTime(string propertyName,Gtk.Calendar calendarWidget,Gtk.SpinButton hourWidget,Gtk.SpinButton minuteWidget) : base(propertyName,calendarWidget)
		{
			CalendarWidget=calendarWidget;
			HourWidget=hourWidget;
			MinuteWidget=minuteWidget;

			HourWidget.SetRange(0,23);
			HourWidget.SetIncrements(1,6);
			MinuteWidget.SetRange(0,59);
			MinuteWidget.SetIncrements(1,15);
		}


		/// <summary>
		///   Fills input widgets with data from a model instance.
		/// </summary>
		/// <param name="model">The model to take data from.</param>
		public override void PopulateFrom(MODEL model)
		{
			DateTime? rawValue=GetModelValue(model);

			if(rawValue==null)
			{
				CalendarWidget.Date=DateTime.Today;
				HourWidget.Text="";
				MinuteWidget.Text="";
				return;
			}

			DateTime value=(DateTime)rawValue;

			CalendarWidget.Date=value;
			HourWidget.Text=string.Format("{0:00}",value.Hour);
			MinuteWidget.Text=string.Format("{0:00}",value.Minute);
		}


		/// <summary>
		///   Validates input widgets' contents.
		/// </summary>
		/// <returns><c>null</c></returns>
		protected override string PerformValidation()
		{
			ValidateRange(HourWidget.Text,"hour",0,23);
			ValidateRange(MinuteWidget.Text,"minute",0,59);

			if(ValidationErrors.Count<1)
			{
				DateTime parsed=CalendarWidget.Date.Date;
				parsed=parsed.AddHours(int.Parse(HourWidget.Text));
				parsed=parsed.AddMinutes(int.Parse(MinuteWidget.Text));
				ParsedValue=parsed;
			}

			return null;
		}

		/// <summary>
		///   Checks if a number is within a closed interval.
		/// </summary>
		/// <param name="input">The number to check.</param>
		/// <param name="fieldName">The field name to use in error messages.</param>
		/// <param name="min">The minimum, inclusive.</param>
		/// <param name="max">The maximum, inclusive.</param>
		private void ValidateRange(string input,string fieldName,int min,int max)
		{
			if(string.IsNullOrEmpty(input))
			{
				ValidationErrors.Add(new ValidationError(fieldName,"cannot be empty"));
				return;
			}

			try
			{
				var parsed=int.Parse(input);
				if(parsed<min||parsed>max)
					ValidationErrors.Add(new ValidationError(fieldName,string.Format("must be within {0} and {1}",min,max)));
			}
			catch(FormatException)
			{
				ValidationErrors.Add(new ValidationError(fieldName,"must be a number"));
			}
		}
	}
}
