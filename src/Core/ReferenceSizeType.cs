// Copyright 2019 Richard Nusser
// Licensed under GPLv3 (see http://www.gnu.org/licenses/)

using System;
namespace Bulkr.Core
{
	/// <summary>
	///   Type for nutritional values' reference size.
	/// </summary>
	public enum ReferenceSizeType
	{
		_100g,
		_100ml,
		_1pc,
	}

	/// <summary>
	///   Extension methods for <see cref="ReferenceSizeType"/>.
	/// </summary>
	public static class ReferenceSizeTypeExtensions
	{
		/// <summary>
		///   Gets the value by which to divide nutritional reference values.
		/// </summary>
		/// <param name="referenceSize">The reference size.</param>
		/// <returns>The scale, e.g. 100 if nutritional value is per 100g.</returns>
		public static float GetScale(this ReferenceSizeType referenceSize)
		{
			switch(referenceSize)
			{
				case ReferenceSizeType._100g:
				case ReferenceSizeType._100ml:
					return 100;
				case ReferenceSizeType._1pc:
					return 1;
				default:
#pragma warning disable RECS0083 // Shows NotImplementedException throws in the quick task bar
					throw new NotImplementedException("unhandled reference size");
#pragma warning restore RECS0083 // Shows NotImplementedException throws in the quick task bar
			}
		}
	}
}
