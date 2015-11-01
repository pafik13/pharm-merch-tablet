
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using Android.App;
using Android.Provider;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Android.Media;

using Newtonsoft.Json;

using DroidTest.Lib.Entities;
using DroidTest.Lib.Entities.Pharmacy;

namespace DroidTest.Lib.Fragments
{
	public class MainFragment : Fragment
	{
		private List<Info> infos = null;
		private List<InfoItem> infosList = null;
		private List<Drug> drugs = null;

		private List<DrugInfo> drugInfos = null;
		private DrugInfo drugInfo = null;

		private TableLayout table = null;
		private TextView text = null;

		private User user = null;
		private static Java.IO.File file = null;

		private int AttID = 0;

		public override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);

			// Create your fragment here
			SetHasOptionsMenu(true);
		}

		DrugInfo GetDrugInfo(int pharmacyID)
		{
			drugInfos = Common.GetDrugInfos();

			foreach (var item in drugInfos) {
				if (item.pharmacy == pharmacyID) {
					return item;
				}
			}

			return null;
		}

		string GetDrugInfoValue(List<AttendanceResult> results, int infoID, int drugID)
		{
			var searchResult = from result in results
							  where (result.info == infoID) && (result.drug == drugID)
							 select result;

			if (searchResult.Count() > 1)
			{
				return string.Empty;
			}

			foreach (var item in searchResult) {
				return item.value;
			}

			return string.Empty;
		}

		void SetDrugInfoValue(DrugInfo drugInfo, DateTime date, int infoID, int drugID, string value)
		{
			foreach (var attendace in drugInfo.attendaces) {
				if (attendace.date == date) {
					foreach (var result in attendace.results) {
						if (result.info == infoID && result.drug == drugID) {
							result.value = value;
							return;
						}
					}
				}
			}
		}

		void SetDrugInfoValue(DrugInfo drugInfo, int attendanceID, int infoID, int drugID, string value)
		{
			foreach (var attendace in drugInfo.attendaces) {
				if (attendace.id == attendanceID) {
					foreach (var result in attendace.results) {
						if (result.info == infoID && result.drug == drugID) {
							result.value = value;
							return;
						}
					}
				}
			}
		}

		public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{

			// Use this to return your custom view for this Fragment
			// return inflater.Inflate(Resource.Layout.YourFragment, container, false);
			user = Common.GetCurrentUser ();

			infos = new List<Info>();
			infos.Add (new Info {id = 2, name = @"Кол-во" });
			infos.Add (new Info {id = 4, name = @"Розница" });
			infos.Add (new Info {id = 6, name = @"Заказано" });

			drugs = Common.GetDrugs (user.username);

			List<DrugInfo> dInfos = new List<DrugInfo>();
			drugInfo = new DrugInfo(1);

			drugInfo.attendaces.Add (new Attendance (new DateTime (2015, 10, 29), infos, drugs) { id = ++AttID });
			drugInfo.attendaces.Add (new Attendance (new DateTime (2015, 10, 30), infos, drugs) { id = ++AttID });
			drugInfo.attendaces.Add (new Attendance (new DateTime (2015, 10, 31), infos, drugs) { id = ++AttID });

			SetDrugInfoValue (drugInfo, new DateTime (2015, 10, 29), 2, 1, 1.ToString ());

			View rootView = inflater.Inflate(Resource.Layout.MainFragment, container, false);

			string[] planets_array = Resources.GetStringArray (Resource.Array.planets_array);

			Pharmacy pharmacy = new Pharmacy { id = 1, fullName = @"ООО Аптека №1", shortName = @"Аптека  №1", address = @"Перекопская, 34" };
			string json = JsonConvert.SerializeObject (pharmacy);
			Log.Info (@"JSON_Pharm", json);
			text = rootView.FindViewById<TextView> (Resource.Id.largeText);
			//text.Text = @"Привет!!!";
			text.Text = json;

			table = rootView.FindViewById<TableLayout> (Resource.Id.mfFullContent);

			RefreshTable ();

			return rootView;
			//return base.OnCreateView (inflater, container, savedInstanceState);
		}

		void RefreshTable()
		{
			
			table.RemoveAllViews ();

			TableRow header = new TableRow (Activity);
			header.SetMinimumHeight (70);
			TableRow.LayoutParams hParamsDrug = new TableRow.LayoutParams ();
			hParamsDrug.Height = TableLayout.LayoutParams.WrapContent;
			hParamsDrug.Width = TableLayout.LayoutParams.WrapContent;
			hParamsDrug.Gravity = GravityFlags.Center;
//			hParamsDrug.Span = 2;

			TextView hDrug = new TextView (Activity);
			hDrug.Text = @"Препараты";
			hDrug.LayoutParameters = hParamsDrug;
			header.AddView(hDrug);

			TableRow.LayoutParams p = new TableRow.LayoutParams ();
			p.Height = TableLayout.LayoutParams.WrapContent;
			p.Width = TableLayout.LayoutParams.WrapContent;
			p.Gravity = GravityFlags.Center;

			TableLayout tlHeader = new TableLayout (Activity);
			TableRow rAttendance = new TableRow (Activity);

			foreach (var attendace in drugInfo.attendaces) {
				TextView hAttendace = new TextView (Activity);
				hAttendace.Text = attendace.date.ToString(@"dd-MMM ddd");
				hAttendace.LayoutParameters = p;
				hAttendace.Rotation = -60;
				header.AddView (hAttendace);
//				rAttendance.AddView(hAttendace);
			}
//			tlHeader.AddView (rAttendance);
//			header.AddView (tlHeader);
//			table.AddView(header);

			foreach (var info in infos) {
				TableRow r = new TableRow (Activity);

				TextView v = new TextView (Activity);
				v.Gravity = GravityFlags.Center;
				v.SetSingleLine (false);
				v.SetMinimumHeight (72);
				v.SetMinimumWidth (68);
				v.Rotation = -90;
				//				v.SetBackgroundResource (Resource.Style.text_row);
				//				v.SetB
				//				v.Text = info.infoID.ToString();
				//				v.Text = GetInfo(info.infoID).name;
				//				v.SetHorizontallyScrolling (false);
				v.Text = info.name;
				v.LayoutParameters = p;

				r.AddView (v);

				TableLayout tl = new TableLayout (Activity);
				if (header.Parent == null) {
					tl.AddView (header);
				}
				tl.Id = info.id;
				foreach (var drug in drugs) {
					TableRow rr = new TableRow (Activity);
					rr.Id = drug.id;

					TextView vv = new TextView (Activity);
					vv.Gravity = GravityFlags.Center;
					vv.SetMinimumHeight (42);
					vv.SetMinimumWidth (76);
					//					vv.Text = drugInfo.drugID.ToString();
					//					vv.Text = GetDrug(drugInfo.drugID).fullName;
					vv.Text = drug.fullName;
					vv.LayoutParameters = p;
					rr.AddView (vv);

					foreach (var attendace in drugInfo.attendaces) {
						RelativeLayout rl = new RelativeLayout(Activity);
						rl.SetGravity (GravityFlags.Center);
						rl.SetMinimumHeight (68);
						rl.SetMinimumWidth (68);
						rl.LayoutParameters = p;
						rl.Id = attendace.id;
						rl.Click += (object sender, EventArgs e) => {
							RelativeLayout rlAttendace = (RelativeLayout) sender;
							TableRow trDrug = (TableRow) rl.Parent;
							TableLayout trInfo = (TableLayout) rl.Parent.Parent;

							string message = string.Format(@"Click to RL.id:{0}, P,id:{1}, PP.id:{2}", rlAttendace.Id, trDrug.Id, trInfo.Id);

							Toast.MakeText(Activity,  message, ToastLength.Short).Show();

							FragmentTransaction trans = FragmentManager.BeginTransaction ();
							DrugInfoValueDialog drugInfoValueDialog = new DrugInfoValueDialog ();
							Bundle args = new Bundle();
							args.PutInt(DrugInfoValueDialog.ATTENDANCE_ID, rlAttendace.Id);
							args.PutInt(DrugInfoValueDialog.DRUG_ID, trDrug.Id);
							args.PutInt(DrugInfoValueDialog.INFO_ID, trInfo.Id);
							args.PutString(DrugInfoValueDialog.VALUE, GetDrugInfoValue(drugInfo.attendaces[rlAttendace.Id - 1].results, trInfo.Id, trDrug.Id));

							drugInfoValueDialog.Arguments = args;
							drugInfoValueDialog.AfterSave += DrugInfoValueDialog_AfterSave;

							drugInfoValueDialog.Show (trans, "dialog fragment");

							Log.Info ("ifSignInButton", "Click");
						};

						string value = GetDrugInfoValue (attendace.results, info.id, drug.id);

						if (string.IsNullOrEmpty (value)) {
							ImageView iv = new ImageView (Activity);
							iv.SetImageResource (Resource.Drawable.ic_add_circle_white_24dp);
							rl.SetBackgroundColor (Android.Graphics.Color.LightPink);
//							rl.SetBackgroundResource(Resource.Style.alert_success);
							rl.AddView (iv);						
						} else {
							TextView vvv = new TextView (Activity);
							vvv.Gravity = GravityFlags.Center;
							vvv.Text = value;
							vvv.SetTextAppearance (Activity, Resource.Style.text_success);
//							vvv.SetTextSize (ComplexUnitType.Sp, 24);
//							vvv.SetTextColor(Android.Graphics.Color.Argb);

							rl.SetBackgroundColor (Android.Graphics.Color.LightGreen);
							rl.AddView (vvv);							
						}

						rr.AddView (rl);
					}

					//					for (int i = 0; i < 2; i++) { // Values
					//						ImageView iv = new ImageView (Activity);
					//						iv.SetImageResource (Resource.Drawable.ic_add_circle_white_24dp);
					//						rr.AddView (iv);
					//					}

					tl.AddView (rr);
				}

				r.AddView (tl);
				table.AddView (r);
			}
		}

		void DrugInfoValueDialog_AfterSave(object sender, EventArgs e)
		{
			DrugInfoValueDialog dialog = (DrugInfoValueDialog)sender;
			int attendaceId = dialog.Arguments.GetInt(DrugInfoValueDialog.ATTENDANCE_ID);
			int drugId = dialog.Arguments.GetInt(DrugInfoValueDialog.DRUG_ID);
			int infoId = dialog.Arguments.GetInt(DrugInfoValueDialog.INFO_ID);
			string value = dialog.Arguments.GetString (DrugInfoValueDialog.VALUE);

			SetDrugInfoValue (drugInfo, attendaceId, infoId, drugId, value);

			RefreshTable ();
		}

		void AttendanceDialogAddNew_AfterSave(object sender, EventArgs e)
		{
			AttendanceDialogAddNew dialog = (AttendanceDialogAddNew)sender;
			string sDate = dialog.Arguments.GetString (AttendanceDialogAddNew.DATE);

			DateTime date = DateTime.Parse (sDate);

			drugInfo.attendaces.Add (new Attendance (date, infos, drugs) { id = ++AttID });
			RefreshTable ();
		}
		Info GetInfo(int id)
		{
			foreach (var info in infos) {
				if (info.id == id) {
					return info;
				}
			}
			return null;
		}

		Drug GetDrug(int id)
		{
			foreach (var drug in drugs) {
				if (drug.id == id) {
					return drug;
				}
			}
			return null;
		}


		public override void OnCreateOptionsMenu (IMenu menu, MenuInflater inflater)
		{
			inflater.Inflate (Resource.Menu.mfActionBar, menu);
			
			base.OnCreateOptionsMenu (menu, inflater);
		}

		public override bool OnOptionsItemSelected (IMenuItem item)
		{
			switch (item.ItemId) {
			case Resource.Id.add_attendance:
				Toast.MakeText (Activity, @"add_attendance click", ToastLength.Short).Show ();
				FragmentTransaction trans = FragmentManager.BeginTransaction ();
				AttendanceDialogAddNew attendanceDialogAddNew = new AttendanceDialogAddNew ();
				Bundle args = new Bundle();
				args.PutString(AttendanceDialogAddNew.DATE, string.Empty);

				attendanceDialogAddNew.Arguments = args;
				attendanceDialogAddNew.AfterSave += AttendanceDialogAddNew_AfterSave;

				attendanceDialogAddNew.Show (trans, "dialog fragment");

				Log.Info ("ifSignInButton", "Click");
				break;
			case Resource.Id.add_photo:
				Toast.MakeText (Activity, @"add_photo click", ToastLength.Short).Show ();

				if (Common.CreateDirForPhotos (user)) {
					Intent intent = new Intent (MediaStore.ActionImageCapture);
					file = new Java.IO.File (Common.GetDirForPhotos(user), String.Format("myPhoto_{0}.jpg", Guid.NewGuid()));
					intent.PutExtra (MediaStore.ExtraOutput, Android.Net.Uri.FromFile (file));
					StartActivityForResult (intent, 0);
				}

				break;
			default:
				break;
			}

			return base.OnOptionsItemSelected (item);
		}

		public override void OnActivityResult (int requestCode, Result resultCode, Intent data)
		{
			base.OnActivityResult (requestCode, resultCode, data);

			if (resultCode == Result.Ok) {
				// Make it available in the gallery
				Intent mediaScanIntent = new Intent (Intent.ActionMediaScannerScanFile);
				Android.Net.Uri contentUri = Android.Net.Uri.FromFile (file);
				mediaScanIntent.SetData (contentUri);

				Activity.SendBroadcast (mediaScanIntent);

				ExifInterface exif = new ExifInterface (file.ToString ());
				text.Text += String.Format(@"TagGpsLatitude : {0} \n", exif.GetAttribute (ExifInterface.TagGpsLatitude));
				text.Text += String.Format(@"TagGpsLongitude : {0} \n", exif.GetAttribute (ExifInterface.TagGpsLongitude));
				text.Text += String.Format(@"TagGpsDatestamp : {0} \n", exif.GetAttribute (ExifInterface.TagGpsDatestamp));
				text.Text += String.Format(@"TagIso : {0} \n", exif.GetAttribute (ExifInterface.TagIso));
				text.Text += String.Format(@"TagDatetime : {0} \n", exif.GetAttribute (ExifInterface.TagDatetime));
			}

			// Dispose of the Java side bitmap.
			GC.Collect();
		}
	}
}

