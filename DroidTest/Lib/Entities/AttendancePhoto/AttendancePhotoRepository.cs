﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Linq;

using DroidTest.Lib;

namespace DroidTest.Lib.Entities
{
	public class AttendancePhotoRepository
	{
		static string fUserName;
		static List<AttendancePhoto> attendancePhotos;

		static AttendancePhotoRepository ()
		{
			// set the db location
			attendancePhotos = new List<AttendancePhoto> ();

			User user = Common.GetCurrentUser ();
			if (user != null) {
				fUserName = user.username;
			} else {
				fUserName = @"";
			}

			ReadXml ();
		}

		static void ReadXml ()
		{
			string storeLocation = Path.Combine(Common.DatabaseFileDir, fUserName, @"attendancePhotos.xml");
			var serializer = new XmlSerializer(typeof(List<AttendancePhoto>));

			if (File.Exists(storeLocation)) {
				using (var stream = new FileStream(storeLocation, FileMode.Open))
				{
					attendancePhotos = (List<AttendancePhoto>)serializer.Deserialize(stream);
				}
			}
		}

		static bool WriteXml ()
		{
			string storeLocation = Path.Combine(Common.DatabaseFileDir, fUserName, @"attendancePhotos.xml");
			new FileInfo(storeLocation).Directory.Create();
			var serializer = new XmlSerializer(typeof(List<AttendancePhoto>));
			using (var writer = new StreamWriter(storeLocation))
			{
				serializer.Serialize(writer, attendancePhotos);
			}

			return true;
		}

		public static AttendancePhoto GetAttendancePhoto(int id)
		{
			for (var t = 0; t < attendancePhotos.Count; t++) {
				if (attendancePhotos[t].id == id)
					return attendancePhotos[t];
			}
			return null;
		}

		public static IEnumerable<AttendancePhoto> GetAttendancePhotos ()
		{
			return attendancePhotos;
		}

		public static IEnumerable<AttendancePhoto> GetAttendancePhotos (int attendanceID)
		{
			return (
			  from attPhotos in attendancePhotos
		    where attPhotos.attendance == attendanceID
			  orderby attPhotos.stamp
			  select attPhotos
			);
		}

		/// <summary>
		/// Insert or update a Doctor
		/// </summary>
		public static int SaveAttendancePhoto (AttendancePhoto item)
		{
			var max = 0;
			if (attendancePhotos.Count > 0)
				max = attendancePhotos.Max(x => x.id);

			if (item.id <= 0) {
				item.id = ++max;

//				SyncQueueManager.AddToQueue (item);

				attendancePhotos.Add (item);
			} else {
				var i = attendancePhotos.Find (x => x.id == item.id);
				if (i != null) {
					i = item; // replaces item in collection with updated value
				} else {
					attendancePhotos.Add (item);
				}
			}

			WriteXml ();
			return item.id;
		}

		public static bool SaveNewAttendancePhotos (int attendanceID, List<AttendancePhoto> photos)
		{
			foreach (var item in photos) {
				item.attendance = attendanceID;
				var max = 0;
				if (attendancePhotos.Count > 0)
					max = attendancePhotos.Max(x => x.id);
				item.id = ++max;

//				SyncQueueManager.AddToQueue (item);

				attendancePhotos.Add (item);
			}
			WriteXml ();
			return true;
		}

		public static bool CorrectAttendanceForSync(int oldAttendance, int newAttendance)
		{
			for (int i = 0; i < attendancePhotos.Count; i++) {
				if (attendancePhotos[i].attendance == oldAttendance) {
					attendancePhotos[i].attendance = newAttendance;
					SyncQueueManager.AddToQueue (attendancePhotos[i]);
				}				
			}

			WriteXml ();
			return true;
		}

		public static int DeleteAttendancePhoto (int id)
		{
			for (var t = 0; t< attendancePhotos.Count; t++) {
				if (attendancePhotos[t].id == id){
					attendancePhotos.RemoveAt (t);
					WriteXml ();
					return 1;
				}
			}
			return -1;
		}
	}
}
