// Copyright 2019 Richard Nusser
// Licensed under GPLv3 (see http://www.gnu.org/licenses/)

namespace Bulkr.Core.Models
{
	/// <summary>
	///   Model for food items, e.g. a bar of chocolate.
	/// </summary>
	public class Food : NutritionalData
	{
		/// <summary>
		///   The food's name, e.g. "120% Cocoa Chocolate Bar"
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		///   The food item's optional brand name, e.g. "ChocCo Choco Co."
		/// </summary>
		public string Brand { get; set; }

		/// <summary>
		///   The reference amount the nutritional values are for, e.g. per 100g.
		/// </summary>
		public ReferenceSizeType ReferenceSize { get; set; }
	}
}
