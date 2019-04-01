// Copyright 2019 Richard Nusser
// Licensed under GPLv3 (see http://www.gnu.org/licenses/)

namespace Bulkr.Core.Models
{
	/// <summary>
	///   Base class for models.
	/// </summary>
	public abstract class Model
	{
		/// <summary>
		///   ID field for models. Entity Framework will use this as the primary key.
		/// </summary>
		public int ID { get; set; }
	}
}
