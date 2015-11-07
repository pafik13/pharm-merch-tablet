
using System;
using System.IO;
using System.Net;
using System.Globalization;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

using Newtonsoft.Json;

using RestSharp;
using RestSharp.Authenticators;
using RestSharp.Deserializers;

using DroidTest.Lib.Entities;
using DroidTest.Lib.Entities.Pharmacy;

namespace DroidTest.Lib.Fragments
{

	public class SyncFragment : Fragment
	{
		private DateTime selectedDate = DateTime.Now.Date;
		private List<DateTime> dates = null;
		private List<SyncQueue> queue = null;

		private Spinner spnDates = null;
		private LinearLayout llSyncItems = null;
		private ImageView ivSync = null;
		ProgressDialog progressDialog = null;

		private LayoutInflater SavedInflater = null;

		public override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);

			// Create your fragment here
		}

		public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			// Use this to return your custom view for this Fragment
			// return inflater.Inflate(Resource.Layout.YourFragment, container, false);

			base.OnCreateView (inflater, container, savedInstanceState);

			dates = SyncQueueManager.GetAvailableDates ();

			View view = inflater.Inflate (Resource.Layout.SyncFragment, container, false);

			SavedInflater = inflater;

			spnDates = view.FindViewById<Spinner> (Resource.Id.sfSelectedDateSpinner);
			spnDates.Adapter = new ArrayAdapter (Activity, Android.Resource.Layout.SimpleSpinnerItem, SyncQueueManager.DatesToString(dates));
			spnDates.ItemSelected += (object sender, AdapterView.ItemSelectedEventArgs e) => {
//				TextView tv = (TextView) e.View;
				selectedDate = dates[e.Position];
				Toast.MakeText(Activity, selectedDate.ToString(@"d"), ToastLength.Short).Show();
				queue = (List<SyncQueue>) SyncQueueManager.GetSyncQueue(dates[e.Position]);

				RefreshContent();
			};

			llSyncItems = view.FindViewById<LinearLayout> (Resource.Id.sfList);
			ivSync = view.FindViewById<ImageView> (Resource.Id.sfSyncImage);

			ivSync.Click += (object sender, EventArgs e) => {
				progressDialog = ProgressDialog.Show(Activity, "", "Loading rooms...", true);
				progressDialog.SetProgressStyle(ProgressDialogStyle.Spinner);
				new Thread(new ThreadStart(delegate
					{
						//LOAD METHOD TO GET ACCOUNT INFO
						Activity.RunOnUiThread(() => progressDialog.SetMessage(@"Начало загрузки информации о посещениях"));

						UpLoadAttendances();
						UpLoadAttendanceResults();
						UpLoadAttendancePhotos();

						//HIDE PROGRESS DIALOG
						Activity.RunOnUiThread(() => { progressDialog.SetMessage(@"Обновление данных"); RefreshContent(); progressDialog.Dismiss(); }); //progressBar.Visibility = ViewStates.Gone);
					})).Start();
			};

			Activity.Window.AddFlags (WindowManagerFlags.KeepScreenOn);

			return view;
		}

		void UpLoadAttendances()
		{
			string cookieName = string.Empty;
			string cookieValue = string.Empty;
			var user = Common.GetCurrentUser ();

			var login = new RestClient(@"http://sbl-logisapp.rhcloud.com/");

			//login.Authenticator = new SimpleAuthenticator("identifier", "lyubin.p@gmail.com", "password", "q1234567");
			login.Authenticator = new SimpleAuthenticator("identifier", user.username, "password", user.password);
			login.CookieContainer = new CookieContainer();

			var request = new RestRequest(@"auth/local", Method.POST);
			var response = login.Execute<User>(request);
			User userRes = response.Data;

			if (userRes == null)
			{
				Activity.RunOnUiThread(() => progressDialog.SetMessage(@"Не удалось пройти аутентификацию!"));
				return;
			}

			cookieName = response.Cookies[0].Name;
			cookieValue = response.Cookies[0].Value;

			var queueToUpload = (List<SyncQueue>) SyncQueueManager.GetSyncQueue(selectedDate);
			foreach (var q in queueToUpload) {
				if (( q.type == SyncQueueType.sqtAttendance) && (!q.isSync)) {

					var client = new RestClient(@"http://sbl-logisapp.rhcloud.com/");

					//Debug.WriteLine(@"Получение информации о себе.", @"Info");
					Attendance oldAttendance = SyncQueueManager.GetAttendace(q.fileLoacation);
					Activity.RunOnUiThread(() => progressDialog.SetMessage(string.Format(@"Загрузка посещения с id:{0}", oldAttendance.id)));
					request = new RestRequest(@"Attendance/", Method.POST);
					request.AddCookie(cookieName, cookieValue);
					request.RequestFormat = DataFormat.Json;
					request.JsonSerializer.ContentType = "application/json; charset=utf-8";
					int oldID = oldAttendance.id;
					oldAttendance.id = 0;
					request.AddBody(oldAttendance);
					var respAttendance = client.Execute<Attendance>(request);

					Attendance newAttendance = respAttendance.Data;
					oldAttendance.id = oldID;
//					Thread.Sleep (500);

					switch (respAttendance.StatusCode) {
					case HttpStatusCode.OK:
					case HttpStatusCode.Created:
						if (AttendanceResultManager.CorrectAttendanceForSync (oldAttendance.id, newAttendance.id)
							&& AttendancePhotoManager.CorrectAttendanceForSync (oldAttendance.id, newAttendance.id)
							&& AttendanceManager.CorrectAfterSync (oldAttendance, newAttendance)) {
							q.isSync = true;
							SyncQueueManager.SaveSyncQueue (q);
							Activity.RunOnUiThread (() => { 
								progressDialog.SetMessage (string.Format (@"Посещение с id:{0} ЗАГРУЖЕНО!", oldAttendance.id));
//								RefreshContent ();
							});
						} else {
							Activity.RunOnUiThread (() => { 
								progressDialog.SetMessage (string.Format (@"Не удалось скорректировать данные для посещения с id:{0} ОШИБКА!", oldAttendance.id));
//								RefreshContent ();
							});
						}
						continue;
					default:
//						Thread.Sleep (500);
						Activity.RunOnUiThread(() => progressDialog.SetMessage(@"Не удалось загрузить посещение!"));
//						Thread.Sleep (1500);
						break;
					}
				}
			}
		}

		void UpLoadAttendanceResults()
		{
			string cookieName = string.Empty;
			string cookieValue = string.Empty;
			var user = Common.GetCurrentUser ();

			var login = new RestClient(@"http://sbl-logisapp.rhcloud.com/");

			//login.Authenticator = new SimpleAuthenticator("identifier", "lyubin.p@gmail.com", "password", "q1234567");
			login.Authenticator = new SimpleAuthenticator("identifier", user.username, "password", user.password);
			login.CookieContainer = new CookieContainer();

			var request = new RestRequest(@"auth/local", Method.POST);
			var response = login.Execute<User>(request);
			User userRes = response.Data;

			if (userRes == null)
			{
				Activity.RunOnUiThread(() => progressDialog.SetMessage(@"Не удалось пройти аутентификацию!"));
			}

			cookieName = response.Cookies[0].Name;
			cookieValue = response.Cookies[0].Value;

			var queueToUpload = (List<SyncQueue>) SyncQueueManager.GetSyncQueue(selectedDate);
			foreach (var q in queueToUpload) {
				if (( q.type == SyncQueueType.sqtAttendanceResult) && (!q.isSync)) {
					var client = new RestClient(@"http://sbl-logisapp.rhcloud.com/");

					//Debug.WriteLine(@"Получение информации о себе.", @"Info");
					AttendanceResult attendanceResult = SyncQueueManager.GetAttendaceResult(q.fileLoacation);
					Attendance attendance = AttendanceManager.GetAttendance (attendanceResult.attendance);
					Activity.RunOnUiThread(() => progressDialog.SetMessage(string.Format(@"Загрузка значения с id {0} по посещению с id:{1}", attendanceResult.id, attendance.id)));
					request = new RestRequest(@"AttendanceResult/", Method.POST);
					request.AddCookie(cookieName, cookieValue);
					request.RequestFormat = DataFormat.Json;
					request.JsonSerializer.ContentType = "application/json; charset=utf-8";
					attendanceResult.id = 0;
					request.AddBody(attendanceResult);
					var respAttendanceResult = client.Execute(request);

					switch (respAttendanceResult.StatusCode) {
					case HttpStatusCode.OK:
					case HttpStatusCode.Created:
						q.isSync = true;
						SyncQueueManager.SaveSyncQueue (q);
//						Thread.Sleep (500);
						Activity.RunOnUiThread(() => { 
							progressDialog.SetMessage(string.Format(@"@""Значение с id {0} по посещению с id:{1}"" ЗАГРУЖЕНО!", attendanceResult.id, attendance.id));
//							RefreshContent();
						});
						continue;
					default:
//						Thread.Sleep (500);
						Activity.RunOnUiThread(() => progressDialog.SetMessage(@"Не удалось загрузить значение по посещению!"));
//						Thread.Sleep (1500);
						break;
					}
				}
			}
		}

		void UpLoadAttendancePhotos()
		{
			string cookieName = string.Empty;
			string cookieValue = string.Empty;
			var user = Common.GetCurrentUser ();

			var login = new RestClient(@"http://sbl-logisapp.rhcloud.com/");

			//login.Authenticator = new SimpleAuthenticator("identifier", "lyubin.p@gmail.com", "password", "q1234567");
			login.Authenticator = new SimpleAuthenticator("identifier", user.username, "password", user.password);
			login.CookieContainer = new CookieContainer();

			var loginReq = new RestRequest(@"auth/local", Method.POST);
			var loginRes = login.Execute<User>(loginReq);
			User userRes = loginRes.Data;

			if (userRes == null)
			{
				Activity.RunOnUiThread(() => progressDialog.SetMessage(@"Не удалось пройти аутентификацию!"));
			}

			cookieName = loginRes.Cookies[0].Name;
			cookieValue = loginRes.Cookies[0].Value;

			var queueToUpload = (List<SyncQueue>) SyncQueueManager.GetSyncQueue(selectedDate);
			foreach (var q in queueToUpload) {
				if (( q.type == SyncQueueType.sqtAttendancePhoto) && (!q.isSync)) {
					//Debug.WriteLine(@"Получение информации о себе.", @"Info");
					AttendancePhoto attendancePhoto = SyncQueueManager.GetAttendancePhoto(q.fileLoacation);
					Attendance attendance = AttendanceManager.GetAttendance (attendancePhoto.attendance);
					Activity.RunOnUiThread(() => progressDialog.SetMessage(string.Format(@"Загрузка фото с id {0} по посещению с id:{1}", attendancePhoto.id, attendance.id)));

					var client = new RestClient(@"http://sbl-logisapp.rhcloud.com/");

					var request = new RestRequest (@"AttendancePhoto/create?attendance={attendance}&longitude={longitude}&latitude={latitude}&stamp={stamp}", Method.POST);
					request.AddCookie(cookieName, cookieValue);
					request.AddUrlSegment(@"attendance", attendancePhoto.attendance.ToString());
					request.AddUrlSegment(@"longitude", attendancePhoto.longitude.ToString(CultureInfo.CreateSpecificCulture("en-GB")));
					request.AddUrlSegment(@"latitude", attendancePhoto.latitude.ToString(CultureInfo.CreateSpecificCulture("en-GB")));
					request.AddUrlSegment(@"stamp", attendancePhoto.stamp.ToString());
					request.AddFile (@"photo", File.ReadAllBytes (attendancePhoto.photoPath), Path.GetFileName (attendancePhoto.photoPath), string.Empty);

					var response = client.Execute(request);

					switch (response.StatusCode) {
					case HttpStatusCode.OK:
					case HttpStatusCode.Created:
//					case HttpStatusCode.U
						q.isSync = true;
						SyncQueueManager.SaveSyncQueue (q);
//						Thread.Sleep (500);
						Activity.RunOnUiThread(() => {
							progressDialog.SetMessage(string.Format(@"Фото с id {0} по посещению с id:{1}"" ЗАГРУЖЕНО!", attendancePhoto.id, attendance.id));
//							RefreshContent();
						});
						continue;
					default:
//						Thread.Sleep (500);
						Activity.RunOnUiThread(() => progressDialog.SetMessage(@"Не удалось загрузить фото по посещению!"));
//						Thread.Sleep (1500);
						break;
					}
				}
			}			
		}

		void RefreshContent()
		{
			llSyncItems.RemoveAllViews ();
			queue = (List<SyncQueue>)SyncQueueManager.GetSyncQueue (selectedDate);
			foreach (var q in queue) {
				View view = SavedInflater.Inflate(Resource.Layout.SyncFragmentItem, null);

				RelativeLayout rl = view.FindViewById<RelativeLayout> (Resource.Id.sfiRelativeLayout);
				ImageView iv = view.FindViewById<ImageView> (Resource.Id.sfiStatusImage);

				TextView type = view.FindViewById<TextView> (Resource.Id.sfiTypeInfoText);
				TextView loc = view.FindViewById<TextView> (Resource.Id.sfiLocationText);

				if (q.isSync) {
					rl.SetBackgroundColor (Android.Graphics.Color.LightGreen);
					iv.SetImageResource (Resource.Drawable.ic_check_circle_white_48dp);
					type.SetTextAppearance (Activity, Resource.Style.text_success);
					loc.SetTextAppearance (Activity, Resource.Style.text_success_small);
				} else {
					rl.SetBackgroundColor (Android.Graphics.Color.LightPink);
					iv.SetImageResource (Resource.Drawable.ic_highlight_off_white_48dp);
					type.SetTextAppearance (Activity, Resource.Style.text_danger);
					loc.SetTextAppearance (Activity, Resource.Style.text_danger_small);
				}

				switch (q.type) {
				case SyncQueueType.sqtAttendance:
					Attendance att = SyncQueueManager.GetAttendace (q.fileLoacation);
					Pharmacy pharm = PharmacyManager.GetPharmacy (att.pharmacy);
					type.Text = string.Format(@"Тип: Посещение аптеки {0} за дату {1}", pharm.fullName, att.date.ToString(@"d"));
					loc.Text = string.Format(@"Размещение: {0}", q.fileLoacation);
					break;
				case SyncQueueType.sqtAttendanceResult:
					AttendanceResult attRes = SyncQueueManager.GetAttendaceResult (q.fileLoacation);
					Attendance att2 = AttendanceManager.GetAttendance(attRes.attendance);
					Pharmacy pharm2 = PharmacyManager.GetPharmacy (att2.pharmacy);
					type.Text = string.Format(@"Тип: Значение по препарату в посещение аптеки {0} за дату {1}", pharm2.fullName, att2.date.ToString(@"d"));
					loc.Text = string.Format(@"Размещение: {0}", q.fileLoacation);
					break;
				case SyncQueueType.sqtAttendancePhoto:
					AttendancePhoto attPho = SyncQueueManager.GetAttendancePhoto (q.fileLoacation);
					type.Text = string.Format(@"Фото: {0}", attPho.photoPath);
					loc.Text = q.fileLoacation;
					break;
				default:
					type.Text = @"Неизвестный тип файла";
					type.SetTextColor (Android.Graphics.Color.DarkRed);
					break;
				}

				llSyncItems.AddView (view);
			}
		}

		void RefreshContent2()
		{
			LinearLayout.LayoutParams p = new LinearLayout.LayoutParams (LinearLayout.LayoutParams.MatchParent, LinearLayout.LayoutParams.WrapContent);
			foreach (var q in queue) {
				RelativeLayout rl = new RelativeLayout (Activity);
				rl.LayoutParameters = p;

				if (q.isSync) {
					rl.SetBackgroundColor (Android.Graphics.Color.LightGreen);
				} else {
					rl.SetBackgroundColor (Android.Graphics.Color.LightPink);
				}

				TextView type = new TextView (Activity);
				TextView loc = new TextView (Activity);

				switch (q.type) {
				case SyncQueueType.sqtAttendance:
					Attendance att = SyncQueueManager.GetAttendace (q.fileLoacation);
					Pharmacy pharm = PharmacyManager.GetPharmacy (att.pharmacy);
					type.Text = string.Format(@"Посещение аптеки {0} за дату {1}", pharm.fullName, att.date.ToString(@"d"));
					loc.Text = q.fileLoacation;
					break;
				case SyncQueueType.sqtAttendanceResult:
					AttendanceResult attRes = SyncQueueManager.GetAttendaceResult (q.fileLoacation);
					Attendance att2 = AttendanceManager.GetAttendance(attRes.attendance);
					Pharmacy pharm2 = PharmacyManager.GetPharmacy (att2.pharmacy);
					type.Text = string.Format(@"Значение по препарату в посещение аптеки {0} за дату {1}", pharm2.fullName, att2.date.ToString(@"d"));
					loc.Text = q.fileLoacation;
					break;
				case SyncQueueType.sqtAttendancePhoto:
					type.Text = string.Format(@"Фото");
					loc.Text = q.fileLoacation;
					break;
				default:
					type.Text = @"Неизвестный тип файла";
					type.SetTextColor (Android.Graphics.Color.DarkRed);
					break;
				}

				rl.AddView (type);

				RelativeLayout.LayoutParams rlLP = new RelativeLayout.LayoutParams (RelativeLayout.LayoutParams.WrapContent, RelativeLayout.LayoutParams.WrapContent);
				rlLP.AddRule(LayoutRules.Above, type.Id);
				loc.LayoutParameters = rlLP;
				rl.AddView (loc);

				View v = new View (Activity);
				v.SetMinimumHeight (2);

				rl.AddView (v);

				llSyncItems.AddView (rl);
			}
		}
	}
}

