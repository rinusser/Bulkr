// Copyright 2019 Richard Nusser
// Licensed under GPLv3 (see http://www.gnu.org/licenses/)

using System;
namespace Bulkr.Gui.Forms.Field
{
	class InputException : ArgumentException
	{
		public string Field { get; set; }
		public string Reason { get; set; }
		public override string Message { get { return Field+": "+Reason; } }

		public InputException(string field,string reason)
		{
			Field=field;
			Reason=reason;
		}
	}
}
