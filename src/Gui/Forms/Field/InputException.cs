// Copyright 2019 Richard Nusser
// Licensed under GPLv3 (see http://www.gnu.org/licenses/)

using System;
namespace Bulkr.Gui.Forms.Field
{
	/// <summary>
	///   Exception class for user input errors.
	/// </summary>
	/// <note>
	///   Don't rely too heavily on this: the validation mechanism will be improved, probably rendering this obsolete.
	/// </note>
	class InputException : ArgumentException
	{
		/// <summary>
		///   The field name where a validation error occurred.
		/// </summary>
		public string Field { get; }

		/// <summary>
		///   The reason validation failed.
		/// </summary>
		public string Reason { get; }


		/// <summary>
		///   Override for custom exception messages.
		/// </summary>
		public override string Message { get { return Field+": "+Reason; } }


		/// <param name="field">The field/property name the error happened in.</param>
		/// <param name="reason">An error message describing what went wrong.</param>
		public InputException(string field,string reason)
		{
			Field=field;
			Reason=reason;
		}
	}
}
