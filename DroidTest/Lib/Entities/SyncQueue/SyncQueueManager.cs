using System;
using System.Net;
using System.Text;
using System.IO;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Globalization;

using Newtonsoft.Json;
using RestSharp;
using RestSharp.Extensions;

using Android.Util;

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

		public static string HttpUploadFile(string url, string file, string paramName, string contentType, NameValueCollection nvc) {
			Log.Debug(@"UpLoad begin", string.Format("Uploading {0} to {1}", file, url));
			string boundary = "---------------------------" + DateTime.Now.Ticks.ToString("x");
			byte[] boundarybytes = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n");

			HttpWebRequest wr = (HttpWebRequest)WebRequest.Create(url);
			wr.ContentType = "multipart/form-data; boundary=" + boundary;
			wr.Method = "POST";
			wr.KeepAlive = true;
			wr.Credentials = System.Net.CredentialCache.DefaultCredentials;

			Stream rs = wr.GetRequestStream();

			string formdataTemplate = "Content-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}";
			foreach (string key in nvc.Keys)
			{
				rs.Write(boundarybytes, 0, boundarybytes.Length);
				string formitem = string.Format(formdataTemplate, key, nvc[key]);
				byte[] formitembytes = System.Text.Encoding.UTF8.GetBytes(formitem);
				rs.Write(formitembytes, 0, formitembytes.Length);
			}
			rs.Write(boundarybytes, 0, boundarybytes.Length);

			string headerTemplate = "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: {2}\r\n\r\n";
			string header = string.Format(headerTemplate, paramName, Path.GetFileName(file), contentType);
			byte[] headerbytes = System.Text.Encoding.UTF8.GetBytes(header);
			rs.Write(headerbytes, 0, headerbytes.Length);

			FileStream fileStream = new FileStream(file, FileMode.Open, FileAccess.Read);
			byte[] buffer = new byte[4096];
			int bytesRead = 0;
			while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0) {
				rs.Write(buffer, 0, bytesRead);
			}
			fileStream.Close();

			byte[] trailer = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "--\r\n");
			rs.Write(trailer, 0, trailer.Length);
			rs.Close();

			string response = string.Empty;
			WebResponse wresp = null;
			try {
				wresp = wr.GetResponse();
				Stream stream2 = wresp.GetResponseStream();
				StreamReader reader2 = new StreamReader(stream2);
				response = reader2.ReadToEnd();
				Log.Debug(@"Upload End", string.Format("File uploaded, server response is: {0}", response));
			} catch(Exception ex) {
				Log.Error(@"Error uploading file", ex.Message);
				if(wresp != null) {
					wresp.Close();
					wresp = null;
				}
				response = string.Format (@"Exception: {0}", ex.Message);
			} finally {
				wr = null;
			}
			return response;
		}

	}
}
