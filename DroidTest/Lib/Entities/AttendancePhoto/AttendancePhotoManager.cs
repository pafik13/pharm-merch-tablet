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
	public static class AttendancePhotoManager
	{
		static AttendancePhotoManager ()
		{
		}

		public static AttendancePhoto GetAttendancePhoto(int id)
		{
			return AttendancePhotoRepository.GetAttendancePhoto(id);
		}

		public static IList<AttendancePhoto> GetAttendancePhotos (int attendanceID = -1)
		{
			if (attendanceID == -1) {
				return new List<AttendancePhoto> (AttendancePhotoRepository.GetAttendancePhotos ());
			} else {
				return new List<AttendancePhoto> (AttendancePhotoRepository.GetAttendancePhotos (attendanceID));
			}
		}

		public static int SaveAttendancePhoto (AttendancePhoto item)
		{
			return AttendancePhotoRepository.SaveAttendancePhoto(item);;
		}

		public static bool SaveNewAttendancePhotos (int attendanceID, List<AttendancePhoto> photos)
		{
			return AttendancePhotoRepository.SaveNewAttendancePhotos(attendanceID, photos);
		}

		public static int DeleteAttendancePhoto(int id)
		{
			return AttendancePhotoRepository.DeleteAttendancePhoto(id);
		}

	}
}
