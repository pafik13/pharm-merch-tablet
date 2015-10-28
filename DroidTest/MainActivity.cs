using System;
using System.Collections.Generic;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

using Android.Support.V4.Widget;
using Android.Support.V4.App;

using DroidTest.Lib;
using DroidTest.Lib.Fragments;
using DroidTest.Lib.Entities;

namespace DroidTest
{
	[Activity (Label = "DroidTest", MainLauncher = true, Icon = "@drawable/icon")]
	public class MainActivity : Activity
	{
		private string[] planetTitles = null;
		private DrawerLayout drawerLayout = null;
		private ActionBarDrawerToggle drawerToggle = null;
		private ListView leftDrawer = null;
		private SortedList<string, string> navMenu = null;

		Android.App.Fragment fragment = null;
		private User user = null;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);

			planetTitles = Resources.GetStringArray (Resource.Array.planets_array);
			//MainMenu mainMenu = new MainMenu();
			//MainMenu.GetMenu ().Values.CopyTo (planetTitles, 0);

			navMenu = MainMenu.GetMenu ();

			leftDrawer = FindViewById<ListView> (Resource.Id.left_drawer);

//			user = new User {username = @"Zvezdova", email = @"zvezdova@mail.ru"};

			user = Common.GetCurrentUser ();

			RefreshFullMenu ();

			//leftDrawer.AddView (lv);

			//LayoutInflater.Inflate (Resource.Layout.MerchantInfo, leftDrawer, true);

			leftDrawer.ItemClick += delegate(object sender, AdapterView.ItemClickEventArgs e) {
				selectPosition(e.Position, e);
			};

			//LayoutInflater inflater = (LayoutInflater)this.BaseContext.GetSystemService (Context.LayoutInflaterService);

			//View view = inflater.Inflate (Resource.Layout.MerchantInfo, null, true);

			//leftDrawer.AddHeaderView (view);

			drawerLayout = FindViewById<DrawerLayout> (Resource.Id.drawer_layout);
			drawerToggle = new ActionBarDrawerToggle (
				this,
				drawerLayout,
				Resource.Drawable.ic_drawer,
				Resource.String.drawer_open,
				Resource.String.drawer_close
			);

			drawerLayout.SetDrawerListener (drawerToggle);

			/**
			 *  Необходимо для работы иконки в качестве переключателя
			 */
			ActionBar.SetDisplayHomeAsUpEnabled (true);
			ActionBar.SetHomeButtonEnabled (true);

			if (bundle == null) {
				if (user != null) {
					selectPosition (navMenu.IndexOfKey (MainMenu.INFO), null);
				} else {
					selectPosition (navMenu.IndexOfKey (MainMenu.MAIN), null);
				}
			}
		}
			
		public void RefreshFullMenu()
		{
			leftDrawer.Adapter = new ArrayAdapter(this, Resource.Layout.LeftDrawerItem, MainMenu.GetItems(user) );
		}

		public void selectPosition(int position, AdapterView.ItemClickEventArgs e)
		{

			Bundle args = new Bundle ();

//			if (fragment is InfoFragment) {
//				username = fragment.Arguments.GetString (InfoFragment.USERNAME);
//			}

			string navMenuKey = navMenu.Keys [position];

			switch (navMenuKey) {
				case MainMenu.SIGN:
					if (user != null) {
						user = null;
						Common.SetCurrentUser (user);
						RefreshFullMenu ();
						fragment = new InfoFragment ();
						fragment.Arguments = args;
					} else {
						user = Common.GetCurrentUser ();
						RefreshFullMenu ();
						fragment = new InfoFragment ();
						fragment.Arguments = args;
					}
					break;
				case MainMenu.MAIN:
					fragment = new MainFragment ();
					break;
				case MainMenu.PHARMACY:
					fragment = new PharmacyFragment ();
					break;
				case MainMenu.INFO:
					fragment = new InfoFragment ();
					fragment.Arguments = args;
//				((InfoFragment)fragment).OnSuc
					break;	
				case MainMenu.ROUT:
					fragment = new RoutFragment ();
					fragment.Arguments = args;	
					break;
				default:
					fragment = new MyFragment();
			
					args.PutInt (MyFragment.POSITION, position);
					fragment.Arguments = args;
					break;
			}
//			Toast.MakeText(this, menuList.ToString(), ToastLength.Short).Show();

			this.FragmentManager.BeginTransaction ().Replace (Resource.Id.content_frame, fragment).Commit();

			leftDrawer.SetItemChecked (position, true);

			ActionBar.Title = navMenu[navMenuKey];//planetTitles [position];

			drawerLayout.CloseDrawer (leftDrawer);

 		}

		/**
         * When using the ActionBarDrawerToggle, you must call it during
         * OnPostCreate() and OnConfigurationChanged()...
         */
		protected override void OnPostCreate (Bundle savedInstanceState)
		{
			base.OnPostCreate (savedInstanceState);
			drawerToggle.SyncState ();
		}

		public override bool OnOptionsItemSelected(IMenuItem item)
		{
			// The action bar home/up action should open or close the drawer.
			// ActionBarDrawerToggle will take care of this.
			if (drawerToggle.OnOptionsItemSelected (item)) {
				return true;
			}

			return base.OnOptionsItemSelected (item);
		}

	}

	public class MyFragment : Android.App.Fragment
	{
		public static string POSITION = @"position";

		public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			//return base.OnCreateView (inflater, container, savedInstanceState);
			View rootView = inflater.Inflate(Resource.Layout.FragmentLayout, container, false);

			//string [] planets = Resources.GetStringArray (Resource.Array.planets_array);
			TextView txt = rootView.FindViewById<TextView> (Resource.Id.text);
			txt.Text = MainMenu.GetMenu().Keys[this.Arguments.GetInt (POSITION)];//planets [this.Arguments.GetInt (POSITION)];

			return rootView;
		}

	}

	public class MainMenu
	{
		public const string SIGN	 = @"0_SIGN";
		public const string MAIN     = @"1_MAIN";
		public const string PHARMACY = @"2_PHARMACY";
		public const string ROUT     = @"3_ROUT";
		public const string SETTINGS = @"4_SETTINGS";
		public const string INFO	 = @"5_INFO";
		public const string ABOUT    = @"6_ABOUT";

		static SortedList <string, string> menuDict;
		static string[] menuItems;

		static MainMenu()
		{
			if (menuDict == null) {
				menuDict = new SortedList<string, string> ();
				menuDict.Add (SIGN, @"Вход/Выход");
				menuDict.Add (MAIN, @"Главная");
				menuDict.Add (PHARMACY, @"Аптеки");
				menuDict.Add (ROUT, @"Маршрут");
				menuDict.Add (SETTINGS, @"Настройки");
				menuDict.Add (INFO, @"Информация");
				menuDict.Add (ABOUT, @"О программе");

				RefreshMenuItems ();
			}
		}

//		private Dictionary<string, string> menuItems = null;
//
//		public MainMenu()
//		{
//			menuItems.Add (MAIN, @"Главная");
//			menuItems.Add (PHARMCY, @"Аптеки");
//			menuItems.Add (ABOUT, @"О программе");
//		}

		public static SortedList<string, string> GetMenu()
		{
			return menuDict;
		}

		public static void RefreshMenuItems()
		{
			menuItems = new string[menuDict.Count];
			menuDict.Values.CopyTo (menuItems, 0);
		}

		public static string[] GetItems(User user)
		{
			if (user != null) {
				menuDict [SIGN] = user.username + @"(Выход)";
			} else {
				menuDict [SIGN] = @"Вход";
			}

			RefreshMenuItems ();

			return menuItems;
		}

	}
}


