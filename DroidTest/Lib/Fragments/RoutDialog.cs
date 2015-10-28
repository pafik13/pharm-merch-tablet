
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
	public class RoutDialog : DialogFragment
	{
		public const int NEEDED_PHARMACIES_COUNT = 50;
		public const int NEEDED_CHARS_COUNT = 3;

		public const string DAY_OF_WEEK = @"DAY_OF_WEEK";
		public const string LIST_ITEM = @"LIST_ITEM";
		public const string PHARMACY_ID = @"PHARMACY_ID";

		private TextView tvSearchPhrase = null;
		private TextView tvDanger = null;
		private TextView tvSuccess = null;
		private Button btnPick = null;
		private LinearLayout llDanger = null;
		private LinearLayout llSuccess = null;
		private Spinner spnPharmacyPicker = null;

		private List<Pharmacy> pharmacies = null;
		private	int position = -1;

		public event EventHandler AfterPick;

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

			Dialog.SetCanceledOnTouchOutside (true);

			View view = inflater.Inflate (Resource.Layout.RoutDialog, container, false);

			tvSearchPhrase = view.FindViewById <TextView> (Resource.Id.rdSearchPhrase);
			tvSearchPhrase.TextChanged += SearchPhrase_TextChanged;

			tvDanger = view.FindViewById <TextView> (Resource.Id.rdDangerText);
			tvSuccess = view.FindViewById <TextView> (Resource.Id.rdSuccessText);

			btnPick = view.FindViewById <Button> (Resource.Id.rdPick);

			btnPick.Click += (object sender, EventArgs e) => {
				if (position != -1) {
					Arguments.PutInt(PHARMACY_ID, pharmacies[position].id);
					OnAfterPick(EventArgs.Empty);
					Dismiss();
				}
			};

			llDanger = view.FindViewById <LinearLayout> (Resource.Id.rdDangerLayout);
			llSuccess = view.FindViewById <LinearLayout> (Resource.Id.rdSuccessLayout);

			spnPharmacyPicker = view.FindViewById <Spinner> (Resource.Id.rdPharmacyPicker); 
			spnPharmacyPicker.ItemSelected += PharmacyPicker_ItemSelected;

			return view;


		}

		protected virtual void OnAfterPick(EventArgs e)
		{
			if (AfterPick != null) {
				AfterPick (this, e);
			}
		}

		void PharmacyPicker_ItemSelected (object sender, AdapterView.ItemSelectedEventArgs e)
		{
			position = e.Position;
			return;
		}

		void SearchPhrase_TextChanged (object sender, Android.Text.TextChangedEventArgs e)
		{
			Toast.MakeText(Activity, string.Format("Length = {0}", e.Text.Count ()), ToastLength.Short).Show();
//			if (e.Text.Count () > NEEDED_CHARS_COUNT) {
			pharmacies = (List<Pharmacy>)PharmacyManager.GetPharmacies (e.Text.ToString());


			if (pharmacies.Count > 0 && pharmacies.Count <= NEEDED_PHARMACIES_COUNT) {
				btnPick.Visibility = ViewStates.Visible;
				spnPharmacyPicker.Visibility = ViewStates.Visible;
				llDanger.Visibility = ViewStates.Invisible;
				llSuccess.Visibility = ViewStates.Visible;
				tvSuccess.Text = string.Format (@"Кол-во аптек: {0}", pharmacies.Count);

				ArrayAdapter adapter = new ArrayAdapter (Activity,
					Android.Resource.Layout.SimpleSpinnerItem, GetStringsForSpinner());
//				var adapter = ArrayAdapter.CreateFromResource (
//					Activity, GetStringsForSpinner(), Android.Resource.Layout.SimpleSpinnerItem);

				adapter.SetDropDownViewResource (Resource.Layout.RoutDialogSpinnerItem); //Android.Resource.Layout.SimpleSpinnerDropDownItem
				spnPharmacyPicker.Adapter = adapter;

			} else {
				btnPick.Visibility = ViewStates.Gone;
				spnPharmacyPicker.Visibility = ViewStates.Gone;
				llDanger.Visibility = ViewStates.Visible;
				llSuccess.Visibility = ViewStates.Invisible;
				tvDanger.Text = string.Format (@"Кол-во аптек: {0}", pharmacies.Count);
			}
//			}
		}

		string[] GetStringsForSpinner()
		{
			string[] strings = new string[pharmacies.Count];

			for (int i = 0; i < pharmacies.Count; i++) {
				strings [i] = pharmacies [i].fullName;
			}

			return strings;
		}
	}
}

