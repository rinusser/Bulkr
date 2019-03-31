// Copyright 2019 Richard Nusser
// Licensed under GPLv3 (see http://www.gnu.org/licenses/)

using Bulkr.Core.Models;

namespace Bulkr.Gui_Tests.TestTargets
{
	public class TargetModel : Model
	{
		public string RequiredString { get; set; }
		public string OptionalString { get; set; }
		public float RequiredFloat { get; set; }
		public float? OptionalFloat { get; set; }
		public TargetEnum RequiredEnum { get; set; }
		public TargetEnum? OptionalEnum { get; set; }
	}
}
