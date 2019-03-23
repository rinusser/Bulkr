// Copyright 2019 Richard Nusser
// Licensed under GPLv3 (see http://www.gnu.org/licenses/)

using System;
namespace Bulkr.Core
{
	public enum ReferenceSizeType
	{
		_100g,
		_100ml,
		_1pc,
	}

	public static class ReferenceSizeTypeExtensions
	{
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
