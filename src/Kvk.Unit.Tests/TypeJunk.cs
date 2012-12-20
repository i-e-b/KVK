using System;

namespace Kvk.Unit.Tests
{
	public class TypeJunk
	{
		public DateTime Date { get; set; }
		public String Name { get; set; }
		public bool IsOK { get; set; }

		public SubType Owner { get; set; }
	}

	public class SubType
	{
		public string Name { get; set; }
		public string[] Address { get; set; }
		public NotesType Notes { get; set; }
	}

	public class NotesType
	{
		public string[] Note { get; set; }
	}
}
