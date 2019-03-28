// Copyright 2019 Richard Nusser
// Licensed under GPLv3 (see http://www.gnu.org/licenses/)

using Bulkr.Core.Services;
using Microsoft.EntityFrameworkCore;

namespace Bulkr.Gui_Tests.Components
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
