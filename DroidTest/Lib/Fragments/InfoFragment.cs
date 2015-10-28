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

using DroidTest.Lib;
using DroidTest.Lib.Entities;
using DroidTest.Lib.Entities.Pharmacy;

namespace DroidTest.Lib.Fragments
{
	public class InfoFragment : Fragment
	{
		public static string USERNAME = @"username";

		String username = null;
		User user = null;

		Button bSignIn = null;
		RelativeLayout rlBeforeSignIn = null;
		//RelativeLayout rlAfterSignIn = null;
		LinearLayout llAfterSignIn = null;

		public override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);
			// Create your fragment here
		}

		public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			// Use this to return your custom view for this Fragment
			// return inflater.Inflate(Resource.Layout.YourFragment, container, false);

			user = Common.GetCurrentUser ();

			//return base.OnCreateView (inflater, container, savedInstanceState);

			View rootView = inflater.Inflate (Resource.Layout.InfoFragment, container, false);

			rlBeforeSignIn = rootView.FindViewById<RelativeLayout> (Resource.Id.ifBeforeSignInLayout);
			llAfterSignIn = rootView.FindViewById<LinearLayout> (Resource.Id.ifAfterSignInLayout);

			bSignIn = rootView.FindViewById<Button> (Resource.Id.ifSignInButton);

			bSignIn.Click += (object sender, EventArgs e) => {
				//rlBeforeSignIn.Visibility = ViewStates.Gone;
				FragmentTransaction trans = FragmentManager.BeginTransaction ();
				SigninDialog signinDialog = new SigninDialog (this.Activity);
				//signinDialog.Dialog.SetCanceledOnTouchOutside(true); //Cancelable = true;
				//signinDialog
				signinDialog.Show (trans, "dialog fragment");

//				Resource.Drawable.

				Log.Info ("ifSignInButton", "Click");
			};

			if (user != null) {
				Log.Info ("InfoFragment", "user IS NOT NULL");

				rlBeforeSignIn.Visibility = ViewStates.Gone;

				Rout rout = new Rout (){Date = DateTime.Now};

				View userCard = inflater.Inflate (Resource.Layout.InfoCard, container, false);
				var ucTitle = userCard.FindViewById<TextView> (Resource.Id.icTitle);
				var ucTable = userCard.FindViewById<TableLayout> (Resource.Id.icInfoTable);

				ucTitle.Text = @"Пользователь";

				ucTable.AddView (GetRow(@"ID", user.id.ToString()));
				ucTable.AddView (GetRow(@"Username", user.username));
				ucTable.AddView (GetRow(@"E-mail", user.email));
				ucTable.AddView (GetRow(@"Password", user.password));

				llAfterSignIn.AddView(userCard);


				View merchantCard = inflater.Inflate (Resource.Layout.InfoCard, container, false);
				var mcTitle = merchantCard.FindViewById<TextView> (Resource.Id.icTitle);
				var mcTable = merchantCard.FindViewById<TableLayout> (Resource.Id.icInfoTable);

				mcTitle.Text = @"Представитель";

				var merchant = Common.GetMerchant (user.username);
				mcTable.AddView (GetRow(@"ID", merchant.id.ToString()));
				mcTable.AddView (GetRow(@"ФИО", merchant.lastName + @" " +merchant.firstName));
				mcTable.AddView (GetRow(@"Телефон", merchant.phone));
				mcTable.AddView (GetRow(@"Менеджер", merchant.manager.ToString()));

				llAfterSignIn.AddView(merchantCard);


				View managerCard = inflater.Inflate (Resource.Layout.InfoCard, container, false);
				var managercTitle = managerCard.FindViewById<TextView> (Resource.Id.icTitle);
				var managercTable = managerCard.FindViewById<TableLayout> (Resource.Id.icInfoTable);

				managercTitle.Text = @"Менеджер";

				var manager = Common.GetManager (user.username);
				managercTable.AddView (GetRow(@"ID", manager.id.ToString()));
				managercTable.AddView (GetRow(@"ФИО", manager.lastName + @" " +manager.firstName));
				managercTable.AddView (GetRow(@"Телефон", manager.phone));
				managercTable.AddView (GetRow(@"Начальник", manager.head.ToString()));

				llAfterSignIn.AddView(managerCard);


				View projectCard = inflater.Inflate (Resource.Layout.InfoCard, container, false);
				var projectcTitle = projectCard.FindViewById<TextView> (Resource.Id.icTitle);
				var projectcTable = projectCard.FindViewById<TableLayout> (Resource.Id.icInfoTable);

				projectcTitle.Text = @"Проект";

				var project = Common.GetProject (user.username);
				projectcTable.AddView (GetRow(@"ID", project.id.ToString()));
				projectcTable.AddView (GetRow(@"Название", project.fullName));
				projectcTable.AddView (GetRow(@"Описание", project.description));
				if (project.drugs != null) {
					projectcTable.AddView (GetRow (@"Препараты", string.Join (", ", project.drugs)));
				} else {
					projectcTable.AddView (GetRow (@"Препараты", @""));
				}

				llAfterSignIn.AddView(projectCard);


				View drugsCard = inflater.Inflate (Resource.Layout.InfoCard, container, false);
				var drugscTitle = drugsCard.FindViewById<TextView> (Resource.Id.icTitle);
				var drugscTable = drugsCard.FindViewById<TableLayout> (Resource.Id.icInfoTable);

				drugscTitle.Text = @"Препараты";

				var drugs = Common.GetDrugs (user.username);
				drugscTable.AddView (GetRow(@"ID", @"Наименование"));
				foreach (var drug in drugs) {
					drugscTable.AddView (GetRow(drug.id.ToString(), drug.fullName));				
				}

				llAfterSignIn.AddView(drugsCard);


				View territoryCard = inflater.Inflate (Resource.Layout.InfoCard, container, false);
				var territoryTitle = territoryCard.FindViewById<TextView> (Resource.Id.icTitle);
				var territoryTable = territoryCard.FindViewById<TableLayout> (Resource.Id.icInfoTable);

				territoryTitle.Text = @"Территория";

				var territory = Common.GetTerritory (user.username);
				territoryTable.AddView (GetRow(@"ID", territory.id.ToString()));
				territoryTable.AddView (GetRow(@"Название", territory.name));
				territoryTable.AddView (GetRow(@"Информация", territory.info));
				territoryTable.AddView (GetRow (@"Осн. город", territory.baseCity));
				var pharmacies = PharmacyManager.GetPharmacies ();
				territoryTable.AddView (GetRow (@"Кол-во аптек", pharmacies.Count.ToString()));

				llAfterSignIn.AddView(territoryCard);
			}
			//string [] planets = Resources.GetStringArray (Resource.Array.planets_array);
			//TextView txt = rootView.FindViewById<TextView> (Resource.Id.text);
			//txt.Text = @"Hi!";//planets [this.Arguments.GetInt (POSITION)];

			return rootView;
		}

		TableRow GetRow(string attr, string value){
			TableRow row = new TableRow (this.Activity);

			TextView attrName = new TextView (this.Activity);
			//				attrName.SetTextAppearance (this.Activity, global::Android.Resource.Style.TextAppearanceLarge); //?android:attr/textAppearanceLarge			
			attrName.SetTextAppearance(Activity, Resource.Style.text_row);
			attrName.SetPadding(6, 6, 6, 0);
			attrName.Text = attr;
			row.AddView (attrName);

			TextView attrValue = new TextView (this.Activity);
			//				attrName.SetTextAppearance (this.Activity, global::Android.Resource.Style.TextAppearanceLarge); //?android:attr/textAppearanceLarge			
			attrValue.SetTextAppearance(Activity, Resource.Style.text_row);
			attrValue.SetPadding(6, 6, 6, 0);
			attrValue.Text = value;
			row.AddView (attrValue);

			return row;
		}
	}
}

