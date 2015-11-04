using System;
using System.Net;
using System.Text;
using System.Collections.Generic;
using System.Globalization;

using Newtonsoft.Json;
using RestSharp;
using RestSharp.Extensions;

namespace DroidTest.Lib.Entities
{
	public static class AttendanceManager
	{
		static AttendanceManager ()
		{
		}

		public static Attendance GetAttendance(int id)
		{
			return AttendanceRepository.GetAttendance(id);
		}

		public static IList<Attendance> GetAttendances (int pharmacyID = -1)
		{
			if (pharmacyID == -1) {
				return new List<Attendance> (AttendanceRepository.GetAttendances ());
			} else {
				return new List<Attendance> (AttendanceRepository.GetAttendances (pharmacyID));
			}
		}

		public static int SaveAttendance (Attendance item)
		{
			return AttendanceRepository.SaveAttendance(item);;
		}

		public static int DeleteAttendance(int id)
		{
			return AttendanceRepository.DeleteAttendance(id);
		}
			
	}
}
