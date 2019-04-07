// Copyright 2019 Richard Nusser
// Licensed under GPLv3 (see http://www.gnu.org/licenses/)

using System.Linq;
using Microsoft.EntityFrameworkCore;

using Bulkr.Core.Models;
using Bulkr.Core.Persistence;

namespace Bulkr.Core.Services
{
	/// <summary>
	///   CRUD Service for managing <see cref="Intake"/> items.
	/// </summary>
	public class IntakeService : Service<Intake>
	{
		/// <summary>
		///   Instantiates a new <see cref="IntakeService"/> instance.
		/// </summary>
		/// <remarks>
		///   If no name is supplied, the application configuration's default will be used.
		/// </remarks>
		/// <param name="name">Optional: a database name to use, to e.g. distinguish individual tests.</param>
		/// <returns>The instance.</returns>
		public static IntakeService Create(string name = null)
		{
			return new IntakeService(BulkrContext.GetInstance(name));
		}


		/// <summary>
		///   Basic constructor.
		/// </summary>
		/// <param name="databaseContext">The database context to use.</param>
		public IntakeService(BulkrContext databaseContext) : base(databaseContext,databaseContext.Intakes)
		{
		}


		/// <summary>
		///   Makes database read accesses eager load members.
		/// </summary>
		/// <returns>The Linq handle to the table, ready for use.</returns>
		protected override IQueryable<Intake> GetConfiguredDbSet()
		{
			return DbSet.Include("Food");
		}

		/// <summary>
		///   Reloads reference members from the database, making sure Entity Framework can save the references.
		/// </summary>
		/// <param name="item">The item to fix.</param>
		protected override void DetachReferences(Intake item)
		{
			if(item.Food!=null)
				item.Food=((BulkrContext)DatabaseContext).Foods.Find(item.Food.ID);
		}
	}
}
