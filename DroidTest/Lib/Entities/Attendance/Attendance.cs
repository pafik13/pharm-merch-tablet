using System;

using RestSharp;

namespace DroidTest.Lib.Entities
{
	[Serializable]
	public class Attendance: IEntity
	{
		public Attendance ()
		{
		}

		public int id { get; set; }
		public int merchant { get; set; }
		public int pharmacy { get; set; }
		public DateTime date { get; set; }

	}
}
