// Copyright 2019 Richard Nusser
// Licensed under GPLv3 (see http://www.gnu.org/licenses/)

using System.Linq;
using Microsoft.EntityFrameworkCore;

using Bulkr.Core.Services;

namespace Bulkr.Gui_Tests.TestTargets
{
	public class TargetService : Service<TargetModel>
	{
		public static TargetService Create(string name)
		{
			TargetContext targetContext=TargetContext.CreateInMemoryInstance(name);
			return new TargetService(targetContext,targetContext.TargetSet);
		}


		public TargetService(TargetContext databaseContext,DbSet<TargetModel> dbSet) : base(databaseContext,dbSet)
		{
		}


		protected override IQueryable<TargetModel> GetConfiguredDbSet()
		{
			return DbSet.Include("RequiredServiceDropDown").Include("OptionalServiceDropDown");
		}

		protected override void DetachReferences(TargetModel item)
		{
			if(item.RequiredServiceDropDown!=null)
				item.RequiredServiceDropDown=((TargetContext)DatabaseContext).ReferencedSet.Find(item.RequiredServiceDropDown.ID);

			if(item.OptionalServiceDropDown!=null)
				item.OptionalServiceDropDown=((TargetContext)DatabaseContext).ReferencedSet.Find(item.OptionalServiceDropDown.ID);
		}
	}
}
