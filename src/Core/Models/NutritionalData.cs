// Copyright 2019 Richard Nusser
// Licensed under GPLv3 (see http://www.gnu.org/licenses/)

using System;
namespace Bulkr.Core.Models
{
	public abstract class NutritionalData
	{
		public float Energy { get; set; }
		public float? TotalFat { get; set; }
		public float? SaturatedFat { get; set; }
		public float? TotalCarbohydrates { get; set; }
		public float? Sugar { get; set; }
		public float? Protein { get; set; }
		public float? Fiber { get; set; }
	}
}
