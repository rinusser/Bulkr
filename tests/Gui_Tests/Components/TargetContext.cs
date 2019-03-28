// Copyright 2019 Richard Nusser
// Licensed under GPLv3 (see http://www.gnu.org/licenses/)

using Microsoft.EntityFrameworkCore;

namespace Bulkr.Gui_Tests.Components
{
	public class TargetContext : DbContext
	{
		public static TargetContext CreateInMemoryInstance(string name)
		{
			var options=new DbContextOptionsBuilder<TargetContext>().UseInMemoryDatabase(databaseName:name).Options;
			return new TargetContext(options);
		}

		public DbSet<TargetModel> TargetSet { get; set; }


		private TargetContext(DbContextOptions<TargetContext> options) : base(options)
		{
		}
	}
}
