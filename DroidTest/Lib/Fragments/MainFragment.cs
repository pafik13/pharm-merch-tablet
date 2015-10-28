
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

using Newtonsoft.Json;

using DroidTest.Lib.Entities.Pharmacy;

namespace DroidTest.Lib.Fragments
{
	public class MainFragment : Fragment
	{
		public override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);

			// Create your fragment here
		}

		public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			// Use this to return your custom view for this Fragment
			// return inflater.Inflate(Resource.Layout.YourFragment, container, false);
			View rootView = inflater.Inflate(Resource.Layout.MainFragment, container, false);

			Pharmacy pharmacy = new Pharmacy { id = 1, fullName = @"ООО Аптека №1", shortName = @"Аптека  №1", address = @"Перекопская, 34" };
			string json = JsonConvert.SerializeObject (pharmacy);
			Log.Info (@"JSON_Pharm", json);
			TextView text = rootView.FindViewById<TextView> (Resource.Id.largeText);
			//text.Text = @"Привет!!!";
			text.Text = json;

			return rootView;
			//return base.OnCreateView (inflater, container, savedInstanceState);
		}
	}
}

