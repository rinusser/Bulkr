// Copyright 2019 Richard Nusser
// Licensed under GPLv3 (see http://www.gnu.org/licenses/)

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

using Bulkr.Core.Models;
using Bulkr.Core.Persistence;

namespace Bulkr.Core.Services
{
	/// <summary>
	///   Service for analyzing per-day nutritional intake.
	/// </summary>
	public class TallyService
	{
		/// <summary>
		///   Creates an instance.
		/// </summary>
		/// <param name="name">The database name to use, usually <c>null</c> when running the application, and the test's name in automated tests.</param>
		/// <returns>The created instance.</returns>
		public static TallyService Create(string name = null)
		{
			return new TallyService(BulkrContext.GetInstance(name));
		}


		/// <summary>
		///   The database context to read data from.
		/// </summary>
		public BulkrContext DatabaseContext { get; }


		/// <summary>
		///   Basic constructor.
		/// </summary>
		/// <param name="databaseContext">The database context to use.</param>
		public TallyService(BulkrContext databaseContext)
		{
			DatabaseContext=databaseContext;
		}


		/// <summary>
		///   Fetches all available intake analyses.
		/// </summary>
		/// <returns>The list of analyses, one item per day.</returns>
		public IList<Tally> GetAll()
		{
			return Tally(GetBaseQuery()).ToList();
		}


		/// <summary>
		///   Calculates <paramref name="days">n</paramref>-day averages.
		/// </summary>
		/// <remarks>
		///   Each tally will contain the averaged nutritional data for that day and the <paramref name="days"/>-1 previous
		///   days. Days without intake entries will be calculated as if nothing was eaten that day.
		/// </remarks>
		/// <remarks>
		///   If you request e.g. 7 day averages without a <paramref name="startDate"/> (or one that isn't at least 6 days
		///   after the first stored <see cref="Intake"/> entry, the first 6 tallies in the list won't be accurate as
		///   there's not enough data for them.
		/// </remarks>
		/// <param name="days">How many days should be averaged into each resulting tally. Must be &gt;=1.</param>
		/// <param name="startDate">Optional start day for analysis.</param>
		/// <param name="endDate">Optional end day for analysis.</param>
		/// <returns>The averaged tallies.</returns>
		public IList<Tally> GetAverages(uint days,DateTime? startDate = null,DateTime? endDate = null)
		{
			if(days<1)
				throw new ArgumentException("must request at least 1 day averages");

			var rv=new List<Tally>();
			IDictionary<DateTime,Tally> itemsByDay=GetAll().ToDictionary(i=>i.When,i=>i);
			if(itemsByDay.Count<1)
				return rv;

			endDate=endDate??itemsByDay.Last().Key;
			var currentDate=startDate??itemsByDay.First().Key;
			var scale=1F/days;

			do
			{
				var average=new Tally{ When=currentDate };
				for(var ti = 0;ti<days;ti++)
					average.AddNutritionalData(itemsByDay.ContainsKey(currentDate.AddDays(-ti)) ? itemsByDay[currentDate.AddDays(-ti)] : new Tally(),scale);
				rv.Add(average);

				currentDate=currentDate.AddDays(1).Date;
			}
			while(currentDate<=endDate);

			return rv;
		}


		/// <summary>
		///   Gets the base query to build upon in other methods.
		/// </summary>
		/// <returns>The base query.</returns>
		protected IQueryable<IGrouping<DateTime,Intake>> GetBaseQuery()
		{
			return DatabaseContext.Intakes.Include("Food")
				.GroupBy(i => i.When.Date);
		}

		/// <summary>
		///   Takes a database query of <see cref="Intake"/> items grouped by date and produces <see cref="Tally"/>
		///   objects for each day.
		/// </summary>
		/// <remarks>
		///   Aggregating the query before calling .ToList() on it throws a <see cref="NotImplementedException"/> in
		///   Entity Framework Core 2.2.3, so this method will execute the query (via .ToList()), and only then aggregate it.
		/// </remarks>
		/// <param name="query">The query to read data from.</param>
		/// <returns>The tally objects, one per day.</returns>
		protected IEnumerable<Tally> Tally(IQueryable<IGrouping<DateTime,Intake>> query)
		{
			return query.ToList()
				.Select(g => g.Aggregate(new Tally { When=g.Key },(x,i) => x.AddFood(i.Food,i.Amount)))
				.OrderBy(i => i.When);
		}
	}
}
