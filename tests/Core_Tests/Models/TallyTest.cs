// Copyright 2019 Richard Nusser
// Licensed under GPLv3 (see http://www.gnu.org/licenses/)

using NUnit.Framework;
using System;

using Bulkr.Core;
using Bulkr.Core.Models;

namespace Bulkr.Core_Tests.Models
{
	[TestFixture()]
	public class TallyTest
	{
		private readonly float DEFAULT_TOLERANCE=0.00001F;

		[Test()]
		public void TestAddingScaledFood()
		{
			Tally tally=CreateTallyFixture();
			Food food=CreateScaledFoodFixture();
			tally.AddFood(food,2);

			Assert.AreEqual(1019,tally.Energy,DEFAULT_TOLERANCE);
			Assert.AreEqual(101.6,tally.TotalFat,DEFAULT_TOLERANCE);
			Assert.AreEqual(11.54,tally.SaturatedFat,DEFAULT_TOLERANCE);
			Assert.AreEqual(212,tally.TotalCarbohydrates,DEFAULT_TOLERANCE);
			Assert.AreEqual(47.96,tally.Sugar,DEFAULT_TOLERANCE);
			Assert.AreEqual(7.08,tally.Protein,DEFAULT_TOLERANCE);
			Assert.AreEqual(5.1,tally.Fiber,DEFAULT_TOLERANCE);
		}

		[Test()]
		public void TestAddingFoodIgnoresNullValues()
		{
			Tally tally=CreateTallyFixture();
			Food food=CreateScaledFoodFixture();
			food.SaturatedFat=null;
			food.Fiber=null;
			tally.AddFood(food,2);

			Assert.AreEqual(1019,tally.Energy,DEFAULT_TOLERANCE);
			Assert.AreEqual(11,tally.SaturatedFat,DEFAULT_TOLERANCE);
			Assert.AreEqual(5,tally.Fiber,DEFAULT_TOLERANCE);
		}

		[Test()]
		public void TestAddingUnscaledFood()
		{
			Tally tally=CreateTallyFixture();
			Food food=CreateUnscaledFoodFixture();
			tally.AddFood(food,3);

			Assert.AreEqual(1909,tally.Energy,DEFAULT_TOLERANCE);
			Assert.AreEqual(224,tally.TotalFat,DEFAULT_TOLERANCE);
		}

		private Tally CreateTallyFixture()
		{
			return new Tally
			{
				Energy=1009,
				TotalFat=101,
				SaturatedFat=11,
				TotalCarbohydrates=211,
				Sugar=47,
				Protein=7,
				Fiber=5
			};
		}

		private Food CreateScaledFoodFixture()
		{
			return new Food
			{
				ReferenceSize=ReferenceSizeType._100g,
				Energy=500,
				TotalFat=30,
				SaturatedFat=27,
				TotalCarbohydrates=50,
				Sugar=48,
				Protein=4,
				Fiber=5
			};
		}

		private Food CreateUnscaledFoodFixture()
		{
			return new Food
			{
				ReferenceSize=ReferenceSizeType._1pc,
				Energy=300,
				TotalFat=41
			};
		}
	}
}
