// Copyright 2019 Richard Nusser
// Licensed under GPLv3 (see http://www.gnu.org/licenses/)

using System;
namespace Bulkr.Core.Models
{
	/// <summary>
	///   Model for adding up nutritional values.
	/// </summary>
	public class Tally : NutritionalData
	{
		/// <summary>
		///   The date and/or time this tally is for.
		/// </summary>
		public DateTime When { get; set; }


		/// <summary>
		///   Initializes a new instance with all values set to 0.
		/// </summary>
		public Tally()
		{
			Energy=0;
			TotalFat=0;
			SaturatedFat=0;
			TotalCarbohydrates=0;
			Sugar=0;
			Protein=0;
			Fiber=0;
		}


		/// <summary>
		///   Takes a given amount of a food item and adds the nutritional values for that amount.
		///   <para>
		///     This method already takes care of the food's nutritional information's reference size.
		///     If you e.g. eat 20g of a chocolate bar with a "per 100g" food label, pass <c>20</c> in
		///     <paramref name="amount"/>. If you eat 2 eggs with "per 1 piece" nutritional data, pass <c>2</c>.
		///   </para>
		/// </summary>
		/// <remarks>
		///   <c>null</c> values in foods' nutritional data are converted to 0.
		/// </remarks>
		/// <param name="food">The food item to add.</param>
		/// <param name="amount">The amount of that item to add, in pieces, ml or g.</param>
		/// <returns><c>this</c>.</returns>
		public Tally AddFood(Food food,float amount)
		{
			return AddNutritionalData(food,amount/food.ReferenceSize.GetScale());
		}

		/// <summary>
		///   Adds other nutritional data to this instance.
		/// </summary>
		/// <remarks><c>null</c> values are converted to 0.</remarks>
		/// <param name="data">The data to add.</param>
		/// <param name="scale">A scaling factor for the other nutritional data.</param>
		/// <returns><c>this</c>.</returns>
		public Tally AddNutritionalData(NutritionalData data,float scale)
		{
			Energy+=data.Energy*scale;

			TotalFat+=GetScaledIncrement(data.TotalFat,scale);
			SaturatedFat+=GetScaledIncrement(data.SaturatedFat,scale);
			TotalCarbohydrates+=GetScaledIncrement(data.TotalCarbohydrates,scale);
			Sugar+=GetScaledIncrement(data.Sugar,scale);
			Protein+=GetScaledIncrement(data.Protein,scale);
			Fiber+=GetScaledIncrement(data.Fiber,scale);

			return this;
		}

		/// <summary>
		///   Scales a nutritional value, if set.
		/// </summary>
		/// <param name="addition">The nutritional value to scale.</param>
		/// <param name="scale">The factor to scale by.</param>
		/// <returns>The scaled value, or 0 if the nutritional value was <c>null</c>.</returns>
		protected float GetScaledIncrement(float? addition,float scale)
		{
			return addition!=null ? (float)addition*scale : 0;
		}
	}
}
