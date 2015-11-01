
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

namespace DroidTest.Lib.Fragments
{
	public class DrugInfoValueDialog : DialogFragment
	{
		public const string ATTENDANCE_ID = @"ATTENDANCE_ID";
		public const string DRUG_ID 	  = @"DRUG_ID";
		public const string INFO_ID 	  = @"INFO_ID";
		public const string VALUE 	  	  = @"VALUE";

		private Button btnSave = null;
		private Button btnCancel = null;

		private TextView tvValue = null;

		public event EventHandler AfterSave;

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

			View view = inflater.Inflate (Resource.Layout.DrugInfoValueDialog, container, false);
			btnSave = view.FindViewById<Button> (Resource.Id.divSave);
			btnSave.Click += (object sender, EventArgs e) => {
				Arguments.PutString(VALUE, tvValue.Text);
				OnAfterSave(EventArgs.Empty);
				Dismiss();
			};
			btnCancel = view.FindViewById<Button> (Resource.Id.divCancel);
			btnCancel.Click += (object sender, EventArgs e) => {
				Dismiss();
			};
			tvValue  = view.FindViewById<TextView> (Resource.Id.divValue);
			tvValue.Text = Arguments.GetString (VALUE, string.Empty);

			return view;
		}

		protected virtual void OnAfterSave(EventArgs e)
		{
			if (AfterSave != null) {
				AfterSave (this, e);
			}
		}
	}
}

