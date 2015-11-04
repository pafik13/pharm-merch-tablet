
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

using DroidTest.Lib.Entities;
using DroidTest.Lib.Entities.Pharmacy;

namespace DroidTest.Lib.Fragments
{
	public class RoutFragment : Fragment
	{
		private LinearLayout llContent = null;
		private Spinner spnDatePicker = null;
		private ImageView rfMore = null;

		private Rout rout = null;

		public override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);

			// Create your fragment here
		}

		public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			// Use this to return your custom view for this Fragment
			// return inflater.Inflate(Resource.Layout.YourFragment, container, false);

//			return base.OnCreateView (inflater, container, savedInstanceState);

			View view = inflater.Inflate(Resource.Layout.RoutFragment, container, false);

			rout = new Rout () { Date = DateTime.Now.Date };

			llContent = view.FindViewById <LinearLayout> (Resource.Id.rfContent);
			llContent.SetMinimumWidth( (int) (Math.Min (Resources.DisplayMetrics.HeightPixels, Resources.DisplayMetrics.WidthPixels) / Resources.DisplayMetrics.Density)); 

			spnDatePicker = view.FindViewById <Spinner> (Resource.Id.rfDatePicker);

			string[] data = new string[2];
			data [0] = DateTime.Now.Date.ToString ();
			data [1] = DateTime.Now.Date.AddDays(1).ToString ();

			ArrayAdapter adapter = new ArrayAdapter (Activity, Android.Resource.Layout.SimpleSpinnerItem, data);

			spnDatePicker.Adapter = adapter;

			RefreshRout ();

			rfMore = view.FindViewById <ImageView> (Resource.Id.rfMore);

			rfMore.Click += (object sender, EventArgs e) => {
				((Vibrator)Activity.GetSystemService (Context.VibratorService)).Vibrate(100);
				FragmentTransaction trans = FragmentManager.BeginTransaction ();
				RoutDialogAddNew rdan = new RoutDialogAddNew ();
				rdan.Show (trans, @"routPicker");
			};

			return view;
		}

		void RefreshRout()
		{
			llContent.RemoveAllViews ();

			RelativeLayout rl = null;
			RelativeLayout.LayoutParams rlParams = null;

			for (int d = 0; d < rout.Items.Count; d++ ) {
				var item = rout.Items[d];
				LinearLayout ll = new LinearLayout (Activity);
				LinearLayout.LayoutParams param = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent, 1);
				param.SetMargins (5, 0, 5, 0);
				ll.LayoutParameters = param;
				ll.Orientation = Orientation.Vertical;

				rl = new RelativeLayout (Activity);
				rl.LayoutParameters = new LinearLayout.LayoutParams (ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent, 1);
				rl.SetBackgroundResource (Resource.Drawable.bottomline);

				TextView head = new TextView (Activity);
				var culture = new System.Globalization.CultureInfo("ru-RU");
				//				var day = ;
				head.Text = UppercaseFirst(culture.DateTimeFormat.GetDayName(item.DayOfWeek));
				head.SetTextAppearance (Activity, Resource.Style.headerTextForRout);
				//				head.LayoutParameters = new LinearLayout.LayoutParams (ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent, 1);

				rlParams = new RelativeLayout.LayoutParams (ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent);
				rlParams.AddRule (LayoutRules.CenterInParent);
				head.LayoutParameters = rlParams;

				rl.AddView (head);
				ll.AddView (rl);


				//				TextView height = new TextView (Activity);
				//				height.Text = ().ToString();
				//				height.LayoutParameters = new LinearLayout.LayoutParams (ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent, 1);
				//				ll.AddView (height);

				//				TextView width = new TextView (Activity);
				//				width.Text = ().ToString();
				//				width.LayoutParameters = new LinearLayout.LayoutParams (ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent, 1);
				//				ll.AddView (width);

				for (int i = 0; i < Rout.PHARMACIES_COUNT; i++) {
					rl = new RelativeLayout (Activity);
					rl.LayoutParameters = new LinearLayout.LayoutParams (ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent, 2);
					rl.SetBackgroundResource (Resource.Drawable.bottomline);
					rl.Click += Rl_Click;

					rlParams = new RelativeLayout.LayoutParams (ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent);
					rlParams.AddRule (LayoutRules.CenterInParent);

					if (rout.Items [d].Pharmacies [i] == 0) {
						ImageView imgEmpty = new ImageView (Activity);
						imgEmpty.SetImageResource (Resource.Drawable.ic_add_circle_outline_black_24dp);
						imgEmpty.LayoutParameters = rlParams;
						rl.AddView (imgEmpty);
					} else {
						//					p.LayoutParameters = new LinearLayout.LayoutParams (ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent, 1);
						//p.LayoutParameters = new ViewGroup.LayoutParams(ViewGroup.LayoutParams.WrapContent, 
						//					pharmacy.Draw
						TextView tvPharmacy = new TextView (Activity);
						tvPharmacy.Text = PharmacyManager.GetPharmacy(rout.Items [d].Pharmacies [i]).fullName;
						tvPharmacy.SetTextAppearance (Activity, Resource.Style.rowTextForPharmacy);
						tvPharmacy.LayoutParameters = rlParams;
						rl.AddView (tvPharmacy);
					}

					rl.Id = i;
					ll.AddView (rl);
				}

				ll.Id = d;
				llContent.AddView (ll);
			}
		}

		void Rl_Click (object sender, EventArgs e)
		{
			((Vibrator)Activity.GetSystemService (Context.VibratorService)).Vibrate(100);
			RelativeLayout item = (RelativeLayout)sender;
			LinearLayout parent = (LinearLayout)item.Parent;
			Toast.MakeText (Activity, string.Format(@"llID={0}, rlID={1}", item.Id,parent.Id), ToastLength.Short).Show ();
			FragmentTransaction trans = FragmentManager.BeginTransaction ();
			RoutDialog rd = new RoutDialog ();
			Bundle args = new Bundle ();
			args.PutInt (RoutDialog.DAY_OF_WEEK, parent.Id);
			args.PutInt (RoutDialog.LIST_ITEM, item.Id);
			rd.Arguments = args;
			rd.AfterPick += Rd_AfterPick;
			rd.Show (trans, @"routPicker");
		}

		void Rd_AfterPick (object sender, EventArgs e)
		{
			RoutDialog routDialog = (RoutDialog) sender;
			int dayOfWeek = routDialog.Arguments.GetInt (RoutDialog.DAY_OF_WEEK);
			int listItem = routDialog.Arguments.GetInt (RoutDialog.LIST_ITEM);
			int pharmacyID = routDialog.Arguments.GetInt (RoutDialog.PHARMACY_ID);
			rout.Items [dayOfWeek].Pharmacies [listItem] = pharmacyID;
			RefreshRout ();
		}
			
		static string UppercaseFirst(string s)
		{
			// Check for empty string.
			if (string.IsNullOrEmpty(s))
			{
				return string.Empty;
			}
			// Return char and concat substring.
			return char.ToUpper(s[0]) + s.Substring(1);
		}
	}
}

