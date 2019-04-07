// Copyright 2019 Richard Nusser
// Licensed under GPLv3 (see http://www.gnu.org/licenses/)

using System;
using System.Collections.Generic;
using System.Linq;

namespace Bulkr.Core.Services
{
	/// <summary>
	///   Service for enum types. Attempting to write to this service will result in exceptions.
	/// </summary>
	public class EnumService<TYPE, ID> : CRUDService<TYPE,ID> where ID : struct
	{
		/// <summary>
		///   The exception message for write attempts.
		/// </summary>
		public static readonly string NO_WRITES_MESSAGE="enum service does not support write accesses";


		/// <summary>
		///    The generic enum type, as non-nullable.
		/// </summary>
		protected Type EnumType { get; }

		/// <summary>
		///   The list of values in the enum.
		/// </summary>
		protected IList<TYPE> EnumValues { get; }


		/// <summary>
		///   Constructor. Caches all data since it's small and immutable anyways.
		/// </summary>
		public EnumService()
		{
			var type=typeof(TYPE);
			var nullableBaseType=Nullable.GetUnderlyingType(type);
			if(nullableBaseType!=null)
				type=nullableBaseType;
			EnumType=type;

			EnumValues=Enum.GetValues(EnumType).Cast<TYPE>().ToList();
		}


		/// <summary>
		///   Returns the list of values in the enum.
		/// </summary>
		/// <returns>Enum values.</returns>
		public IList<TYPE> GetAll()
		{
			return EnumValues;
		}


		/// <summary>
		///   Unsupported: always throws an exception.
		/// </summary>
		/// <param name="unused">Doesn't matter.</param>
		/// <returns>Nothing.</returns>
		/// <exception cref="NotSupportedException">Every time.</exception>
		public TYPE GetByID(ID unused)
		{
			throw new NotSupportedException(NO_WRITES_MESSAGE);
		}

		/// <summary>
		///   Unsupported: always throws an exception.
		/// </summary>
		/// <param name="unused">Doesn't matter.</param>
		/// <returns>Nothing.</returns>
		/// <exception cref="NotSupportedException">Every time.</exception>
		public TYPE Add(TYPE unused)
		{
			throw new NotSupportedException(NO_WRITES_MESSAGE);
		}

		/// <summary>
		///   Unsupported: always throws an exception.
		/// </summary>
		/// <param name="unused">Doesn't matter.</param>
		/// <exception cref="NotSupportedException">Every time.</exception>
		public void Delete(TYPE unused)
		{
			throw new NotSupportedException(NO_WRITES_MESSAGE);
		}

		/// <summary>
		///   Unsupported: always throws an exception.
		/// </summary>
		/// <param name="unused">Doesn't matter.</param>
		/// <returns>Nothing.</returns>
		/// <exception cref="NotSupportedException">Every time.</exception>
		public TYPE Update(TYPE unused)
		{
			throw new NotSupportedException(NO_WRITES_MESSAGE);
		}
	}
}
