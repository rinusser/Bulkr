// Copyright 2019 Richard Nusser
// Licensed under GPLv3 (see http://www.gnu.org/licenses/)

using System;
namespace Bulkr.Core.Models
{
	/// <summary>
	///   Base class for models containing nutritional values.
	/// </summary>
	/// <note>
	///   The regulations for food labels differ by country/region, see in particular <see cref="TotalCarbohydrates"/>.
	/// </note>
	public abstract class NutritionalData : Model
	{
		/// <summary>
		///   Energy value, in kcal.
		/// </summary>
		public float Energy { get; set; }

		/// <summary>
		///   Total amount of fat, in grams.
		/// </summary>
		public float? TotalFat { get; set; }

		/// <summary>
		///   Amount of saturated fats, in grams.
		/// </summary>
		public float? SaturatedFat { get; set; }

		/// <summary>
		///   Total amount of digestible carbohydrates, including sugar, in grams.
		/// </summary>
		/// <note>
		///   Some countries, including the US and Canada, include fiber in "carbohydrate" values on food labels.
		///   You're advised to put the amount of fiber in the <see cref="Fiber"/> property instead and subtract
		///   it here, so later analyses of the data entered can produce more accurate results and suggestions.
		/// </note>
		public float? TotalCarbohydrates { get; set; }

		/// <summary>
		///   Amount of sugar, in grams.
		/// </summary>
		public float? Sugar { get; set; }

		/// <summary>
		///   Amount of protein, in grams.
		/// </summary>
		public float? Protein { get; set; }

		/// <summary>
		///   Amount of salt, in grams.
		/// </summary>
		/// <note>
		///   This value is in accordance with current EU guidelines, Salt = Sodium * 2.5.
		/// </note>
		/// <value>The salt.</value>
		public float? Salt { get; set; }

		/// <summary>
		///   Amount of fiber, in grams.
		/// </summary>
		public float? Fiber { get; set; }
	}
}
