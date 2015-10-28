
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

using DroidTest.Lib.Entities.Pharmacy;

namespace DroidTest.Lib.Fragments
{
	public class PharmacyDialog : DialogFragment
	{
		private Button save = null;
		private Button cancel = null;

		private EditText fullName = null;
		private EditText address = null;
		private EditText subway = null;

		public event EventHandler SuccessSaved;

		public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			base.OnCreateView (inflater, container, savedInstanceState);
			// Use this to return your custom view for this Fragment
			// return inflater.Inflate(Resource.Layout.YourFragment, container, false);
			View rootView = inflater.Inflate(Resource.Layout.PharmacyDialogEdit, container, false);

			save = rootView.FindViewById<Button> (Resource.Id.pdePharmacySaveButton);
			save.Click += Save_Click;

			cancel = rootView.FindViewById<Button> (Resource.Id.pdePharmacyCancelButton);
			cancel.Click += (object sender, EventArgs e) => {
				Dismiss();
			};


			fullName = rootView.FindViewById<EditText> (Resource.Id.pdePharmacyFullNameEdit);
			address = rootView.FindViewById<EditText> (Resource.Id.pdePharmacyAddressEdit);
			subway = rootView.FindViewById<EditText> (Resource.Id.pdePharmacySubwayEdit);

			return rootView;
		}

		void Save_Click (object sender, EventArgs e)
		{
			Pharmacy pharmacy = new Pharmacy() { fullName = fullName.Text, address = address.Text, subway = subway.Text};
			int id = PharmacyManager.SavePharmacy (pharmacy);
			Log.Info("PharmacyManager.SavePharmacy", id.ToString());
			Dismiss ();
			if (id > 0) {
				OnSuccessSaved (EventArgs.Empty);
			}
		}

		protected virtual void OnSuccessSaved(EventArgs e)
		{
			if (SuccessSaved != null) {
				SuccessSaved (this, e);
			}
		}
	}
}

