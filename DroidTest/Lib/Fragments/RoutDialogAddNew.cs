
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

namespace DroidTest
{
	public class RoutDialogAddNew : DialogFragment
	{
		private TextView tvDate = null;
		private Button btnIncrease = null;
		private Button btnDecrease = null;

		private DateTime date = DateTime.MinValue;

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

			date = DateTime.Now.AddDays (DayOfWeek.Monday - DateTime.Now.DayOfWeek);

			View view = inflater.Inflate (Resource.Layout.RoutDialogAddNew, container, false);

			tvDate = view.FindViewById<TextView> (Resource.Id.rdanDate);

			tvDate.Text = date.ToString ();

			btnIncrease = view.FindViewById<Button> (Resource.Id.rdanIncrease);
			btnIncrease.Click += (object sender, EventArgs e) => {
				date = date.AddDays(7);
				tvDate.Text = date.ToString();
			};

			btnDecrease = view.FindViewById<Button> (Resource.Id.rdanDecrease);
			btnDecrease.Click += (object sender, EventArgs e) => {
				date = date.AddDays(-7);
				tvDate.Text = date.ToString();
			};

			return view;
		}
	}
}

