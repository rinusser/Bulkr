// Copyright 2019 Richard Nusser
// Licensed under GPLv3 (see http://www.gnu.org/licenses/)

using System;

namespace Bulkr.Core.Models
{
	/// <summary>
	///   Model for an intake entry, e.g. eating a bar of chocolate.
	/// </summary>
	public class Intake : Model
	{
		/// <summary>
		///   When the food was consumed.
		/// </summary>
		public DateTime When { get; set; }

		/// <summary>
		///   How much of the food was consumed.
		/// </summary>
		/// <remarks>
		///   This is in the food's reference size units, e.g. "25" for 25 grams of chocolate (per 100g),
		///   or "2" for 2 eggs.
		/// </remarks>
		public float Amount { get; set; }

		/// <summary>
		///   What food was eaten.
		/// </summary>
		public Food Food { get; set; }
	}
}
