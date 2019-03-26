// Copyright 2019 Richard Nusser
// Licensed under GPLv3 (see http://www.gnu.org/licenses/)

using System;
namespace Bulkr.Gui.Form.Element
{
	public class NutritionalValue
	{
		public static string Format(float? value)
		{
			if(value==null)
				return "";
			return string.Format("{0:0.##}",value);
		}

		public static float ParseNonNegative(string field,string text)
		{
			float value;
			try
			{
				value=float.Parse(text);
			}
			catch(FormatException)
			{
				throw new InputException(field,"not a valid number");
			}

			if(value<0)
				throw new InputException(field,"cannot be negative");

			return value;

		}

		public static float? ParseNonNegativeOrNull(string field,string text)
		{
			if(text.Trim().Length<=0)
				return null;
			return ParseNonNegative(field,text);
		}

	}
}