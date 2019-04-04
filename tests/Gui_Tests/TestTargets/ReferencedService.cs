// Copyright 2019 Richard Nusser
// Licensed under GPLv3 (see http://www.gnu.org/licenses/)

using Microsoft.EntityFrameworkCore;

using Bulkr.Core.Services;

namespace Bulkr.Gui_Tests.TestTargets
{
	public class ReferencedService : Service<ReferencedModel>
	{
		public static ReferencedService Create(string name,TargetContext targetContext)
		{
			return new ReferencedService(targetContext,targetContext.ReferencedSet);
		}


		public ReferencedService(TargetContext databaseContext,DbSet<ReferencedModel> dbSet) : base(databaseContext,dbSet)
		{
		}
	}
}
