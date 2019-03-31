// Copyright 2019 Richard Nusser
// Licensed under GPLv3 (see http://www.gnu.org/licenses/)

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
	}
}
