// Copyright 2019 Richard Nusser
// Licensed under GPLv3 (see http://www.gnu.org/licenses/)

using System;
using System.Collections.Generic;

namespace Bulkr.Gui.Forms
{
	public class Form<MODEL> where MODEL : new()
	{
		protected IList<Field.Field> Fields { get; }


		public Form()
		{
			Fields=new List<Field.Field>();
		}


		public Form<MODEL> AddField(Field.Field field)
		{
			Fields.Add(field);
			return this;
		}

		public void Populate(MODEL model)
		{
			foreach(var field in Fields)
				field.PopulateFrom(model);
		}

		public MODEL Parse()
		{
			return ParseInto(new MODEL());
		}

		public MODEL ParseInto(MODEL model)
		{
			foreach(var field in Fields)
				field.ParseInto(model);
			return model;
		}
	}
}
