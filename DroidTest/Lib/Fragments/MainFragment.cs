
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
		const int MENU_PHOTO = 2;

		private bool bIsPhotoMake = false;

		private User user = null;
		private Merchant merchant = null;
		private Rout currentRout = null;
		private List<Pharmacy> currentPharmacies = null;
		private int selectedPharmacy = 0;
		private List<Info> infos = null;
		private List<Drug> drugs = null;
		private List<Attendance> currentAttendances = null;
		private Attendance newAttendance = null;
		private List<AttendanceResult> newAttendanceResults = null;
		private List<AttendancePhoto> newAttendancePhotos = null;

		private TableLayout table = null;
		private TextView text = null;
		private Spinner spnSelectedPharmacy = null;

		private static Java.IO.File file = null;

		private IMenuItem miAddAtt = null;
		private IMenuItem miAddPhoto = null;

		public override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);

			// Create your fragment here
			SetHasOptionsMenu(true);
		}

		public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{

			// Use this to return your custom view for this Fragment
			// return inflater.Inflate(Resource.Layout.YourFragment, container, false);

			View rootView = inflater.Inflate(Resource.Layout.MainFragment, container, false);

			text = rootView.FindViewById<TextView> (Resource.Id.largeText);

			user = Common.GetCurrentUser ();

			if (user == null) {
				text.Text = @"НЕТ ПРЕПАРАТОВ ИЛИ СОБИРАЕМОЙ ИНФОРМАЦИИ";
				text.SetTextAppearance (Activity, Resource.Style.text_danger);

				miAddAtt.SetEnabled (false);
				return rootView;
			}

			merchant = Common.GetMerchant (user.username);

			currentPharmacies = (List<Pharmacy>)PharmacyManager.GetPharmacies (string.Empty, 20);

			infos = Common.GetInfos (user.username);

			drugs = Common.GetDrugs (user.username);

			table = rootView.FindViewById<TableLayout> (Resource.Id.mfFullContent);

			spnSelectedPharmacy = rootView.FindViewById<Spinner> (Resource.Id.mfSelectedPharmacySpinner);

			ArrayAdapter adapter = new ArrayAdapter (Activity, Android.Resource.Layout.SimpleSpinnerItem, PharmacyManager.ToArray (currentPharmacies));

			spnSelectedPharmacy.Adapter = adapter;

			spnSelectedPharmacy.ItemSelected += (object sender, AdapterView.ItemSelectedEventArgs e) => {
				selectedPharmacy = currentPharmacies [e.Position].id;

				currentAttendances = (List<Attendance>)AttendanceManager.GetAttendances (selectedPharmacy);

				RefreshTable();
			};

			spnSelectedPharmacy.SetSelection (0);

//			RefreshTable ();

			return rootView;
			//return base.OnCreateView (inflater, container, savedInstanceState);
		}

		void RefreshTable()
		{
			table.RemoveAllViews ();

			TableRow header = new TableRow (Activity);
//			header.
			header.SetMinimumHeight (88);
			TableRow.LayoutParams hParamsDrug = new TableRow.LayoutParams ();
			hParamsDrug.Height = TableLayout.LayoutParams.WrapContent;
			hParamsDrug.Width = TableLayout.LayoutParams.WrapContent;
			hParamsDrug.Gravity = GravityFlags.Center;
//			hParamsDrug.Span = 2;

			TextView hDrug = new TextView (Activity);
			hDrug.Text = @"Препараты";
			hDrug.LayoutParameters = hParamsDrug;
			header.AddView(hDrug);

			TableRow.LayoutParams lpRow = new TableRow.LayoutParams ();
			lpRow.Height = TableLayout.LayoutParams.WrapContent;
			lpRow.Width = TableLayout.LayoutParams.WrapContent;
			lpRow.Gravity = GravityFlags.Center;

//			TableLayout.LayoutParams pp = new TableLayout.LayoutParams ();
//			pp.
//			TableLayout tlHeader = new TableLayout (Activity);
//			TableRow rAttendance = new TableRow (Activity);

			foreach (var attendace in currentAttendances) {
				TextView hAttendace = new TextView (Activity);
				hAttendace.Text = attendace.date.ToString(@"dd-MMM ddd");
				hAttendace.LayoutParameters = lpRow;
				hAttendace.Rotation = -70;
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
				v.LayoutParameters = lpRow;

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
					vv.LayoutParameters = lpRow;
					vv.SetBackgroundColor (Android.Graphics.Color.White);
					rr.AddView (vv);

					foreach (var attendace in currentAttendances) {
						
						TableRow.LayoutParams lpValue = new TableRow.LayoutParams ();
						lpValue.Height = TableLayout.LayoutParams.WrapContent;
						lpValue.Width = TableLayout.LayoutParams.WrapContent;
						lpValue.Gravity = GravityFlags.Center;
						lpValue.SetMargins (1, 1, 1, 1);

						RelativeLayout rl = new RelativeLayout(Activity);
						rl.SetGravity (GravityFlags.Center);
						rl.SetMinimumHeight (68);
						rl.SetMinimumWidth (68);
						rl.LayoutParameters = lpValue;

						rl.Id = attendace.id;
						rl.SetTag (Resource.String.IDinfo, info.id);
						rl.SetTag (Resource.String.IDdrug, drug.id);
						rl.SetTag (Resource.String.IDattendance, attendace.id);

						string value = string.Empty;
						if (attendace.id != -1) {
							value = AttendanceResultManager.GetAttendanceResultValue (attendace.id, info.id, drug.id);
						} else {
							value = AttendanceResultManager.GetResultValue (newAttendanceResults, info.id, drug.id);
							rl.Click += Rl_Click;
						}

						TextView vvv = new TextView (Activity);
						vvv.Gravity = GravityFlags.Center;
						if (string.IsNullOrEmpty (value) || value.Equals(@"N")) {
							vvv.SetTextAppearance (Activity, Resource.Style.text_danger);
//							rl.SetBa
							rl.SetBackgroundColor (Android.Graphics.Color.LightPink);
//							rl.SetBackgroundResource(Resource.Style.alert_success);
						} else {
							vvv.SetTextAppearance (Activity, Resource.Style.text_success);
//							vvv.SetTextSize (ComplexUnitType.Sp, 24);
//							vvv.SetTextColor(Android.Graphics.Color.Argb);

							rl.SetBackgroundColor (Android.Graphics.Color.LightGreen);
						}
						vvv.Text = AttendanceResult.StringBoolToRussian(value);
						rl.AddView (vvv);							
						rr.AddView (rl);
					}

					tl.AddView (rr);
				}

				r.AddView (tl);
				table.AddView (r);
			}
		}

		void Rl_Click (object sender, EventArgs e)
		{
			RelativeLayout rlAttendace = (RelativeLayout) sender;

			int IDattendance = (int) rlAttendace.GetTag(Resource.String.IDattendance);
			int IDdrug = (int) rlAttendace.GetTag(Resource.String.IDdrug);
			int IDinfo = (int) rlAttendace.GetTag(Resource.String.IDinfo);

			TableRow trDrug = (TableRow) rlAttendace.Parent;
			TableLayout trInfo = (TableLayout) rlAttendace.Parent.Parent;

//			string message = string.Format(@"Click {0} to RL.id:{1}, P,id:{2}, PP.id:{3}", tag, rlAttendace.Id, trDrug.Id, trInfo.Id);
			string message = string.Format(@"Click to IDattendance:{0}, IDdrug:{1}, IDinfo:{2}", IDattendance, IDdrug, IDinfo);

			Toast.MakeText(Activity,  message, ToastLength.Short).Show();

			string value = AttendanceResultManager.GetResultValue(newAttendanceResults, IDinfo, IDdrug);

			value = AttendanceResult.InvertStringBool(value);

			AttendanceResultManager.SetResultValue(newAttendanceResults, IDinfo, IDdrug, value);

			RefreshTable();
		}

		void DrugInfoValueDialog_AfterSave(object sender, EventArgs e)
		{
			DrugInfoValueDialog dialog = (DrugInfoValueDialog)sender;
			int attendaceId = dialog.Arguments.GetInt(DrugInfoValueDialog.ATTENDANCE_ID);
			int drugId = dialog.Arguments.GetInt(DrugInfoValueDialog.DRUG_ID);
			int infoId = dialog.Arguments.GetInt(DrugInfoValueDialog.INFO_ID);
			string value = dialog.Arguments.GetString (DrugInfoValueDialog.VALUE);

//			SetDrugInfoValue (drugInfo, attendaceId, infoId, drugId, value);

			RefreshTable ();
		}

		void AttendanceDialogAddNew_AfterSave(object sender, EventArgs e)
		{
			AttendanceDialogAddNew dialog = (AttendanceDialogAddNew)sender;
			string sDate = dialog.Arguments.GetString (AttendanceDialogAddNew.DATE);

			DateTime date = DateTime.Parse (sDate);

//			drugInfo.attendaces.Add (new Attendance (1, date, infos, drugs) { id = ++AttID });
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
//			inflater.Inflate (Resource.Menu.mfActionBar, menu);

			miAddAtt = menu.Add (1, 1, 1, @"Начать посещение");
			miAddAtt.SetIcon (Resource.Drawable.ic_add_circle_outline_white_48dp);
//			miAddAtt.SetShowAsAction()
			miAddAtt.SetEnabled (true);

//			miAddAtt.Id

			miAddPhoto = menu.Add(1, MENU_PHOTO, 2, @"Добавить фото");
			miAddPhoto.SetIcon (Resource.Drawable.ic_camera_alt_white_48dp);
			miAddPhoto.SetEnabled (false);

			base.OnCreateOptionsMenu (menu, inflater);
		}

		public override bool OnOptionsItemSelected (IMenuItem item)
		{
			if (item.ItemId == miAddAtt.ItemId) {
				Log.Info ("Add Att Click", "Click");
				Toast.MakeText (Activity, @"add_attendance click", ToastLength.Short).Show ();

				if (miAddPhoto.IsEnabled) {
					int attID = AttendanceManager.SaveAttendance (newAttendance);
					AttendanceResultManager.SaveNewAttendanceResults (attID, newAttendanceResults);
					AttendancePhotoManager.SaveNewAttendancePhotos (attID, newAttendancePhotos);
					newAttendance = null;
					newAttendanceResults = null;
					newAttendancePhotos = null;
					currentAttendances = (List<Attendance>)AttendanceManager.GetAttendances (selectedPharmacy);
					RefreshTable ();
					miAddPhoto.SetEnabled (false);
					miAddAtt.SetIcon (Resource.Drawable.ic_add_circle_outline_white_48dp);
					miAddAtt.SetTitle (@"Начать посещение");
				} else {
					newAttendance = new Attendance () { id = -1, date = DateTime.Now, pharmacy = selectedPharmacy, merchant = merchant.id};
					newAttendanceResults = AttendanceResultManager.GenerateResults (infos, drugs, @"N");
					newAttendancePhotos = new List<AttendancePhoto> ();
					currentAttendances.Add (newAttendance);
					miAddPhoto.SetEnabled (true);
					miAddAtt.SetIcon (Resource.Drawable.ic_remove_circle_outline_white_48dp);
					miAddAtt.SetTitle (@"Закончить посещение");
					RefreshTable ();
				}

				return true;
			}

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
			case MENU_PHOTO:
				Toast.MakeText (Activity, @"add_photo click", ToastLength.Short).Show ();

				if (Common.CreateDirForPhotos (user)) {
					Intent intent = new Intent (MediaStore.ActionImageCapture);
					file = new Java.IO.File (Common.GetDirForPhotos(user), String.Format("photo_{0}.jpg", Guid.NewGuid()));
					intent.PutExtra (MediaStore.ExtraOutput, Android.Net.Uri.FromFile (file));
					bIsPhotoMake = true;
					StartActivityForResult (intent, 0);
				}

				break;
			default:
				break;
			}

			return base.OnOptionsItemSelected (item);
		}

		public override void OnPause ()
		{
			base.OnPause ();

			if (!bIsPhotoMake) {
				if (newAttendance != null) {
					int attID = AttendanceManager.SaveAttendance (newAttendance);
					AttendanceResultManager.SaveNewAttendanceResults (attID, newAttendanceResults);

					newAttendance = null;
					newAttendanceResults = null;
				}
			}
		}

		private float convertToDegree(String stringDMS){
			float result = 0.0f;
			if (string.IsNullOrEmpty(stringDMS)) {
				return result;
			} else {
				char[] spl1 = new char[1] { ',' };
				string[] DMS = stringDMS.Split(spl1, 3);

				char[] spl2 = new char[1] { '/' };
				string[] stringD = DMS[0].Split(spl2, 2);
				double D0 = double.Parse((stringD[0]));
				double D1 = double.Parse(stringD[1]);
				double FloatD = D0/D1;

				string[] stringM = DMS[1].Split(spl2, 2);
				double M0 = double.Parse(stringM[0]);
				double M1 = double.Parse(stringM[1]);
				double FloatM = M0/M1;

				string[] stringS = DMS[2].Split(spl2, 2);
				double S0 = double.Parse(stringS[0]);
				double S1 = double.Parse(stringS[1]);
				double FloatS = S0/S1;

				return (float)(FloatD + (FloatM/60) + (FloatS/3600));
			}
		}

		public override void OnActivityResult (int requestCode, Result resultCode, Intent data)
		{
			base.OnActivityResult (requestCode, resultCode, data);

			if (resultCode == Result.Ok) {
				bIsPhotoMake = false;
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

				AttendancePhoto attPhoto = new AttendancePhoto () { id = -1,  photoPath = file.ToString ()};
				DateTime dtStamp;
				if (DateTime.TryParse (exif.GetAttribute (ExifInterface.TagDatetime), out dtStamp)){
					attPhoto.stamp = dtStamp;
				};

				float gps;
				if (float.TryParse (exif.GetAttribute (ExifInterface.TagGpsLatitude), out gps)){
					attPhoto.latitude = gps;
				};

				if (float.TryParse (exif.GetAttribute (ExifInterface.TagGpsLongitude), out gps)){
					attPhoto.longitude = gps;
				};

				attPhoto.latitude = convertToDegree (exif.GetAttribute (ExifInterface.TagGpsLatitude));
				attPhoto.longitude = convertToDegree (exif.GetAttribute (ExifInterface.TagGpsLongitude));

				newAttendancePhotos.Add (attPhoto);
			}

			// Dispose of the Java side bitmap.
			GC.Collect();
		}
	}
}

