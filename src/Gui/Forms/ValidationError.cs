// Copyright 2019 Richard Nusser
// Licensed under GPLv3 (see http://www.gnu.org/licenses/)

namespace Bulkr.Gui.Forms
{
	/// <summary>
	///   Container class for user input errors.
	/// </summary>
	public class ValidationError
	{
		/// <summary>
		///   The field name where a validation error occurred.
		/// </summary>
		public string Field { get; }

		/// <summary>
		///   The reason validation failed.
		/// </summary>
		public string Reason { get; }


		/// <param name="field">The field/property name the error happened in.</param>
		/// <param name="reason">An error message describing what went wrong.</param>
		public ValidationError(string field,string reason)
		{
			Field=field;
			Reason=reason;
		}
	}
}
