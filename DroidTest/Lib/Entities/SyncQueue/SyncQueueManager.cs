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
	public static class SyncQueueManager
	{
		static SyncQueueManager ()
		{
		}

		public static SyncQueue GetSyncQueue(int id)
		{
			return SyncQueueRepository.GetSyncQueue(id);
		}

		public static List<DateTime> GetAvailableDates ()
		{
			return new List<DateTime> (SyncQueueRepository.GetAvailableDates ());
		}

		public static string [] DatesToString (List<DateTime> dates)
		{
			string[] result = new string[dates.Count];

			for (int i = 0; i < dates.Count; i++) {
				result[i] = dates[i].Date.ToString (@"d");
			} 

			return result;
		}

		public static IList<SyncQueue> GetSyncQueue ()
		{
			return new List<SyncQueue> (SyncQueueRepository.GetSyncQueue ());
		}

		public static IList<SyncQueue> GetSyncQueue (DateTime date)
		{
			return new List<SyncQueue> (SyncQueueRepository.GetSyncQueue (date));
		}

		public static int AddToQueue (Attendance attendance)
		{
			return SyncQueueRepository.AddToQueue(attendance);
		}

		public static Attendance GetAttendace(string location)
		{
			return SyncQueueRepository.GetAttendace(location);

		}

		public static int AddToQueue (AttendanceResult attendanceResult)
		{
			return SyncQueueRepository.AddToQueue(attendanceResult);
		}

		public static AttendanceResult GetAttendaceResult(string location)
		{
			return SyncQueueRepository.GetAttendaceResult(location);

		}

		public static int AddToQueue (AttendancePhoto attendancePhoto)
		{
			return SyncQueueRepository.AddToQueue(attendancePhoto);
		}

		public static AttendancePhoto GetAttendancePhoto(string location)
		{
			return SyncQueueRepository.GetAttendancePhoto(location);
		}

		public static int SaveSyncQueue (SyncQueue item)
		{
			return SyncQueueRepository.SaveSyncQueue(item);
		}

	}
}
