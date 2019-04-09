// Copyright 2019 Richard Nusser
// Licensed under GPLv3 (see http://www.gnu.org/licenses/)

using System;
using OxyPlot;
using OxyPlot.GtkSharp;
using OxyPlot.Series;
using OxyPlot.Axes;

using Bulkr.Core.Models;
using Bulkr.Core.Services;
using Bulkr.Gui.Utils;

namespace Bulkr.Gui.Components
{
	/// <summary>
	///   UI component for displaying nutritional analyses in a graph.
	/// </summary>
	/// <remarks>
	///   The tracker shown when clicking one of the columns in the chart displays the wrong values, this is because of
	///   an OxyPlot issue, see https://github.com/oxyplot/oxyplot/issues/1171.
	/// </remarks>
	public class AnalysisComponent : Component
	{
		/// <summary>
		///   The service to read per-day food intake from.
		/// </summary>
		private TallyService TallyService;

		/// <summary>
		///   The graph widget.
		/// </summary>
		private PlotView PlotView;

		/// <summary>
		/// The graph widget's internal model.
		/// </summary>
		private PlotModel PlotModel;

		/// <summary>
		///   The data series for per-day energy intake.
		/// </summary>
		private RectangleBarSeries EnergySeries;

		/// <summary>
		///   The data series for 7 day average energy intakes.
		/// </summary>
		private LineSeries EnergyAverageSeries;

		/// <summary>
		///   The earliest date to analyze data from.
		/// </summary>
		private DateTime AnalysisStart;

		/// <summary>
		///   The date that should be leftmost by default.
		/// </summary>
		private DateTime DefaultFirstDateDisplayed;

		/// <summary>
		///   The latest date to display.
		/// </summary>
		private DateTime AnalysisEnd;

		/// <summary>
		///   The y axis for energy values.
		/// </summary>
		private LinearAxis EnergyAxis;

		/// <summary>
		///   The x axis for dates.
		/// </summary>
		private DateTimeAxis DateAxis;


		/// <summary>
		///   Adds and configures the widget, sets up everything else needed and then loads data.
		/// </summary>
		/// <remarks>
		///   MonoDevelop/stetic couldn't find any OxyPlot widgets, so the plot widget gets added manually here.
		/// </remarks>
		/// <param name="context">The component context to use.</param>
		public AnalysisComponent(ComponentContext context) : base(context)
		{
			AddPlotWidget();
			SetupAxes();
			SetupDataSeries();
			TallyService=TallyService.Create();
			Reload();
		}


		/// <summary>
		///   Part of the component setup: adds the plot widget with an empty model and sets layout options.
		/// </summary>
		private void AddPlotWidget()
		{
			var table=(Gtk.Table)Window.GetWidget("analysis_layout_table");

			PlotView=new PlotView();
			table.Add(PlotView);

			var plotViewLayout=(Gtk.Table.TableChild)table[PlotView];
			plotViewLayout.TopAttach=0;
			plotViewLayout.BottomAttach=1;
			plotViewLayout.LeftAttach=0;
			plotViewLayout.RightAttach=1;
			plotViewLayout.XOptions=Gtk.AttachOptions.Fill|Gtk.AttachOptions.Expand;
			plotViewLayout.YOptions=Gtk.AttachOptions.Fill|Gtk.AttachOptions.Expand;

			PlotModel=new PlotModel();
			PlotView.Model=PlotModel;
			PlotView.ShowAll();
		}

		/// <summary>
		///   Part of the component setup: adds axes and determines their boundaries.
		/// </summary>
		private void SetupAxes()
		{
			AnalysisStart=DateTime.Today.AddYears(-1);
			DefaultFirstDateDisplayed=DateTime.Today.AddDays(-7).AddHours(-12);
			AnalysisEnd=DateTime.Today.AddDays(1).AddSeconds(-1);

			DateAxis=new DateTimeAxis
			{
				Title="date",
				Position=AxisPosition.Bottom,
				Minimum=DateTimeAxis.ToDouble(DefaultFirstDateDisplayed),
				AbsoluteMinimum=DateTimeAxis.ToDouble(AnalysisStart),
				Maximum=DateTimeAxis.ToDouble(AnalysisEnd.AddHours(-12)),
				AbsoluteMaximum=DateTimeAxis.ToDouble(AnalysisEnd.AddHours(-12)),
				StringFormat="  ddd\nyyyy-\nMM-dd",
				MajorStep=1,
				MajorTickSize=2,
				MinorStep=0.5,
				MinorTickSize=7,
				MinorGridlineStyle=LineStyle.Solid,
				MinorGridlineColor=OxyColors.LightGray,
			};
			PlotModel.Axes.Add(DateAxis);

			EnergyAxis=new LinearAxis
			{
				Title="kcal",
				AbsoluteMinimum=0,
				Minimum=0,
				MajorStep=1000,
				MajorGridlineStyle=LineStyle.Solid,
				MajorGridlineColor=OxyColors.Gray,
				MinorGridlineStyle=LineStyle.Solid,
				MinorGridlineColor=OxyColors.LightGray,
			};
			PlotModel.Axes.Add(EnergyAxis);
		}

		/// <summary>
		///   Part of the component setup: sets up data series to plot.
		/// </summary>
		private void SetupDataSeries()
		{
			EnergySeries=new RectangleBarSeries
			{
				Title="kcal eaten (per day)",
				FillColor=OxyColors.DarkOrange,
				//TrackerFormatString="{0}\n{1}: {3:yyyy-MM-dd}\n{4}: {6:0.0}",
				TrackerFormatString="",  //tracker shows wrong data because of OxyPlot bug, better disable tracker instead
			};
			PlotModel.Series.Add(EnergySeries);

			EnergyAverageSeries=new LineSeries
			{
				Title="kcal eaten (7d avg)",
				Color=OxyColors.DarkBlue,
				MarkerType=MarkerType.Square,
				MarkerFill=OxyColors.DarkBlue,
				CanTrackerInterpolatePoints=false,
				TrackerFormatString="{0}\n{1}: {2:yyyy-MM-dd}\n{3}: {4:0.0}",
			};
			PlotModel.Series.Add(EnergyAverageSeries);
		}


		/// <summary>
		///   Reads analysis data and plots it.
		/// </summary>
		public override void Reload()
		{
			EnergySeries.Items.Clear();
			float yMax=1970;
			var tallies=TallyService.GetAll();
			DateTime? firstDate=null;
			foreach(var tally in tallies)
			{
				if(tally.When<AnalysisStart)
					continue;
				if(firstDate==null)
					firstDate=tally.When;
				AddEnergyDataPoint(tally);
				if(tally.Energy>yMax)
					yMax=tally.Energy;
			}
			EnergyAxis.Zoom(0,yMax+30);
			EnergyAxis.AbsoluteMaximum=(yMax+30)*1.1;
			if(firstDate!=null)
				DateAxis.AbsoluteMinimum=Axis.ToDouble(firstDate?.Date<DefaultFirstDateDisplayed ? firstDate?.Date : DefaultFirstDateDisplayed);

			EnergyAverageSeries.Points.Clear();
			foreach(var tally in TallyService.GetAverages(7,endDate: DateTime.Today))
				EnergyAverageSeries.Points.Add(new DataPoint(Axis.ToDouble(tally.When.Date),tally.Energy));

			PlotModel.InvalidatePlot(true);
		}

		/// <summary>
		///   Adds a single data point for the energy series.
		/// </summary>
		/// <param name="tally">The <see cref="Tally"/> item to turn into a data point.</param>
		private void AddEnergyDataPoint(Tally tally)
		{
			var xMin=Axis.ToDouble(tally.When.Date.AddHours(-10));
			var xMax=Axis.ToDouble(tally.When.Date.AddHours(10));
			var yMin=0F;
			var yMax=tally.Energy;
			EnergySeries.Items.Add(new RectangleBarItem(xMin,yMin,xMax,yMax));
		}
	}
}
