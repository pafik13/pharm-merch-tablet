using System;
using System.Collections.Generic;

using RestSharp;

namespace DroidTest.Lib.Entities
{
	[Serializable]
	public class Attendance: IEntity
	{
		public Attendance (DateTime dateOfAttendance, List<Info> infos, List<Drug> drugs)
		{
			date = dateOfAttendance;

			results = new List<AttendanceResult> ();
			foreach (var info in infos) {
				foreach (var drug in drugs) {
					results.Add (new AttendanceResult () {
						info = info.id,
						drug = drug.id,
						value = string.Empty
					});
				}
			}
		}

		public int id { get; set; }
		public DateTime date { get; set; }
		public List<AttendanceResult> results { get; set; }
	}

	[Serializable]
	public class AttendanceResult: IEntity
	{
		public AttendanceResult()
		{
		}

		public int id	{ get; set; }
		public int info { get; set; }
		public int drug { get; set; }
		public string value { get; set; }
	}
}

