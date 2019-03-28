// Copyright 2019 Richard Nusser
// Licensed under GPLv3 (see http://www.gnu.org/licenses/)

using System;
namespace Bulkr.Core.Models
{
	public class Food : NutritionalData
	{
		public string Name { get; set; }
		public string Brand { get; set; }

		public ReferenceSizeType ReferenceSize { get; set; }
	}
}
