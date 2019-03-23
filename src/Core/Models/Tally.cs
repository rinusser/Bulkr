// Copyright 2019 Richard Nusser
// Licensed under GPLv3 (see http://www.gnu.org/licenses/)

using System;
namespace Bulkr.Core.Models
{
	public class Tally : NutritionalData
	{
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

		public void AddFood(Food food, float amount)
		{
			float scale=amount/food.ReferenceSize.GetScale();

			Energy+=food.Energy*scale;

			TotalFat+=GetScaledIncrement(food.TotalFat,scale);
			SaturatedFat+=GetScaledIncrement(food.SaturatedFat,scale);
			TotalCarbohydrates+=GetScaledIncrement(food.TotalCarbohydrates,scale);
			Sugar+=GetScaledIncrement(food.Sugar,scale);
			Protein+=GetScaledIncrement(food.Protein,scale);
			Fiber+=GetScaledIncrement(food.Fiber,scale);
		}

		protected float? GetScaledIncrement(float? addition, float scale)
		{
			return addition!=null ? (float)addition*scale : 0;
		}
	}
}
