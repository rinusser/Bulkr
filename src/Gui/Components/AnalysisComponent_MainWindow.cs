// Copyright 2019 Richard Nusser
// Licensed under GPLv3 (see http://www.gnu.org/licenses/)

using Bulkr.Gui.Utils;
using Bulkr.Gui.Components;

public partial class MainWindow : ApplicationWindow
{
	/// <summary>
	///   The AnalysisComponent instance for showing per-day nutritional analyses.
	/// </summary>
	private AnalysisComponent AnalysisComponent;


	/// <summary>
	///   Adds <see cref="AnalysisComponent"/> instance to main window. Called by constructor.
	/// </summary>
	public void AddAnalysisComponent()
	{
		AnalysisComponent=new AnalysisComponent(new ComponentContext { Window=this });
	}
}