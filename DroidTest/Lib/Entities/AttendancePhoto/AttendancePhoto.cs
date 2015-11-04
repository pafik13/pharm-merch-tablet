using System;

using RestSharp;

namespace DroidTest.Lib.Entities
{
	[Serializable]
	public class AttendancePhoto: IEntity
	{
		public AttendancePhoto ()
		{
			stamp = DateTime.Now;
		}

		public int id { get; set; }
		public int attendance { get; set; }
		public int drug { get; set; }
		public float longitude { get; set; }
		public float latitude { get; set; }
		public string photoPath { get; set; }
		public DateTime stamp { get; set; }
	}
}
