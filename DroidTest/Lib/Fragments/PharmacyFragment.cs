using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Text;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;

using DroidTest.Lib.Entities.Pharmacy;

namespace DroidTest.Lib.Fragments
{
	enum ColumnPosition {cpFirst, cpLast, cpMiddle}

	public class PharmacyFragment : Fragment
	{
		private Button pfPharmacyAddButton = null;
		private TableLayout pfPharmacyTable = null;
		private TableLayout pfPharmacyTableHeader = null;
//		private TableRow pfPharmacyTableHeaderRow = null;
		private TextView pfAddPharmacy = null;
		private LinearLayout pfContent = null;
		private TableRow row2 = null;
		private List<Pharmacy> pharmacies = null;
		private LayoutInflater layoutInflater = null;
		private EditText pfSearchEdit = null;

		public override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);

			// Create your fragment here
		}

		public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			// Use this to return your custom view for this Fragment
			// return inflater.Inflate(Resource.Layout.YourFragment, container, false);
			layoutInflater = inflater;

			View rootView = inflater.Inflate(Resource.Layout.PharmacyFragment, container, false);

			pfSearchEdit = rootView.FindViewById<EditText> (Resource.Id.pfSearchEdit);

			pfSearchEdit.TextChanged += SearchEdit_TextChanged;

			pfPharmacyAddButton = rootView.FindViewById<Button> (Resource.Id.pfPharmacyAddButton);

			pfPharmacyAddButton.Click += PharmacyAddButton_Click;

			pfPharmacyTable 	  = rootView.FindViewById<TableLayout> (Resource.Id.pfPharmacyTable);
			pfPharmacyTableHeader = rootView.FindViewById<TableLayout> (Resource.Id.pfPharmacyTableHeader);
			pfContent			  = rootView.FindViewById<LinearLayout> (Resource.Id.pfContent);
//			pfPharmacyTableHeaderRow = rootView.FindViewById<TableRow> (Resource.Id.pfPharmacyTableHeaderRow);
			pfAddPharmacy = rootView.FindViewById<TextView> (Resource.Id.pfAddPharmacy);

			pfAddPharmacy.Click += PharmacyAddButton_Click;

			//Add Header Row
			TableRow row = new TableRow (Activity);
			row.SetBackgroundResource(Resource.Drawable.bottomline);


			TextView id = GetHeadItem (ColumnPosition.cpFirst);
			id.Gravity = GravityFlags.Center;
			id.Text = @"ID";
			row.AddView (id);

			TextView fullName = GetHeadItem (ColumnPosition.cpMiddle);
			fullName.Gravity = GravityFlags.CenterVertical;
			fullName.Text = @"Full Name";
			row.AddView (fullName);

			TextView shortName = GetHeadItem (ColumnPosition.cpMiddle);
			shortName.Gravity = GravityFlags.CenterVertical;
			shortName.Text = @"Short Name";
			row.AddView (shortName);

			TextView officialName = GetHeadItem (ColumnPosition.cpMiddle);
			officialName.Gravity = GravityFlags.CenterVertical;
			officialName.Text = @"Official Name";
			row.AddView (officialName);

			TextView address = GetHeadItem (ColumnPosition.cpMiddle);
			address.Gravity = GravityFlags.CenterVertical;
			address.Text = @"Address";
			row.AddView (address);

			TextView subway = GetHeadItem (ColumnPosition.cpMiddle);
			subway.Gravity = GravityFlags.CenterVertical;
			subway.Text = @"Subway";
			row.AddView (subway);

			TextView phone = GetHeadItem (ColumnPosition.cpMiddle);
			phone.Gravity = GravityFlags.CenterVertical;
			phone.Text = @"Phone";
			row.AddView (phone);

			TextView email = GetHeadItem (ColumnPosition.cpMiddle);
			email.Gravity = GravityFlags.CenterVertical;
			email.Text = @"E-mail";
			row.AddView (email);

			TextView delete = GetHeadItem (ColumnPosition.cpLast);
			delete.Gravity = GravityFlags.CenterVertical;
			delete.Text = @"Actions";
			row.AddView (delete);

			pfPharmacyTableHeader.AddView(row);

			//Add Header Row
			row2 = new TableRow (Activity);
			row2.SetBackgroundResource(Resource.Drawable.bottomline);


			TextView id2 = GetHeadItem (ColumnPosition.cpFirst);
			id2.Gravity = GravityFlags.Center;
			id2.Text = @"ID";
			row2.AddView (id2);

			TextView fullName2 = GetHeadItem (ColumnPosition.cpMiddle);
			fullName2.Gravity = GravityFlags.CenterVertical;
			fullName2.Text = @"Full Name";
			row2.AddView (fullName2);

			TextView shortName2 = GetHeadItem (ColumnPosition.cpMiddle);
			shortName2.Gravity = GravityFlags.CenterVertical;
			shortName2.Text = @"Short Name";
			row2.AddView (shortName2);

			TextView officialName2 = GetHeadItem (ColumnPosition.cpMiddle);
			officialName2.Gravity = GravityFlags.CenterVertical;
			officialName2.Text = @"Official Name";
			row2.AddView (officialName2);

			TextView address2 = GetHeadItem (ColumnPosition.cpMiddle);
			address2.Gravity = GravityFlags.CenterVertical;
			address2.Text = @"Address";
			row2.AddView (address2);

			TextView subway2 = GetHeadItem (ColumnPosition.cpMiddle);
			subway2.Gravity = GravityFlags.CenterVertical;
			subway2.Text = @"Subway";
			row2.AddView (subway2);

			TextView phone2 = GetHeadItem (ColumnPosition.cpMiddle);
			phone2.Gravity = GravityFlags.CenterVertical;
			phone2.Text = @"Phone";
			row2.AddView (phone2);

			TextView email2 = GetHeadItem (ColumnPosition.cpMiddle);
			email2.Gravity = GravityFlags.CenterVertical;
			email2.Text = @"E-mail";
			row2.AddView (email2);

			TextView delete2 = GetHeadItem (ColumnPosition.cpLast);
			delete2.Gravity = GravityFlags.CenterVertical;
			delete2.Text = @"Actions";
			row2.AddView (delete2);
			pfPharmacyTable.AddView (row2);
//	
//			foreach (var pharmacy in pharmacies) {
//				TableRow row = new TableRow (this.Activity);
//
//				TextView id = new TextView (this.Activity);
//				id.Text = pharmacy.id.ToString ();
//				row.AddView (id);
//
//				TextView fullName = new TextView (this.Activity);
//				fullName.Text = pharmacy.fullName;
//				row.AddView (fullName);
//					
//				TextView address = new TextView (this.Activity);
//				address.Text = pharmacy.address;
//				row.AddView (address);
//
//				pfPharmacyTable.AddView(row);
//			}

			return rootView;
		}

		void SearchEdit_TextChanged (object sender, Android.Text.TextChangedEventArgs e)
		{
			RefreshTableContent ();
		}

		void PharmacyAddButton_Click (object sender, EventArgs e)
		{
			FragmentTransaction trans = FragmentManager.BeginTransaction();
			PharmacyDialog pharmacyDialog = new PharmacyDialog();
			pharmacyDialog.SuccessSaved += PharmacyDialog_SuccessSaved;
			pharmacyDialog.Show(trans, "dialog fragment");

			Log.Info("pfPharmacyAddButton", "Click");
			((Vibrator)Activity.GetSystemService (Context.VibratorService)).Vibrate(100);
		}

		private void RefreshTableContent2()
		{

			if (pfPharmacyTable != null) {
				int childCount = pfPharmacyTable.ChildCount;

				// Remove all rows except the first one
				if (childCount > 1) {
					pfPharmacyTable.RemoveViews(1, childCount - 1);
				}

				pharmacies = (List<Pharmacy>)PharmacyManager.GetPharmacies (pfSearchEdit.Text, 20);

				foreach (var pharmacy in pharmacies) {
					string src = pfSearchEdit.Text;
					string srcWithCap = UppercaseFirst(pfSearchEdit.Text);
					string rpl = "";

					TableRow row = new TableRow (Activity);

					row.SetBackgroundResource(Resource.Drawable.bottomline);

					//View view = layoutInflater.Inflate (Resource.Layout.LeftDrawerItem, null, false);//TextView id = new TextView (this.Activity);
					TextView id = new TextView (Activity);
					//					id.SetTextAppearance (this.Activity, global::Android.Resource.Style.TextAppearanceLarge); //?android:attr/textAppearanceLarge			
					//					id.SetTextAppearance(Activity, Resource.Style.rowTextForPharmacy);
					//					id.SetPadding(24, 0, 24, 0);
					//					TableRow.LayoutParams p = 
					//					p.RightMargin = 24;
					//					p.LeftMargin = 24;
					id.LayoutParameters = new TableRow.LayoutParams() {RightMargin = 24, LeftMargin = 24};
					id.SetBackgroundResource(Resource.Drawable.bottomline);
					id.SetTextAppearance (Activity, Resource.Style.rowTextForPharmacy);
					id.SetHeight (48);
					id.Gravity = GravityFlags.Center;
					id.Text = pharmacy.id.ToString ();
					row.AddView (id);

					//					CheckBox chk = new CheckBox (Activity);
					//					chk.SetPadding(24, 16, 24, 16);
					//					row.AddView (chk);

					TextView fullName = new TextView (Activity);
					//					fullName.SetTextAppearance (Activity, global::Android.Resource.Style.TextAppearanceLarge); //?android:attr/textAppearanceLarge			
					//					fullName.SetPadding(0, 0, 56, 0);
					//					TableRow.LayoutParams fullNameP = new TableRow.LayoutParams();
					//					fullNameP.RightMargin = 56;
					fullName.LayoutParameters = new TableRow.LayoutParams() {RightMargin = 56};
					//					fullName.SetTextAppearance (Activity, Resource.Style.rowTextForPharmacy);
					//					fullName.SetBackgroundResource(Resource.Drawable.bottomline);
					fullName.SetTextAppearance (Activity, Resource.Style.rowTextForPharmacy);
					fullName.SetHeight (48);
					fullName.Gravity = GravityFlags.CenterVertical;
					if (string.IsNullOrEmpty (src)) {
						fullName.Text = pharmacy.fullName;
					} else {
						rpl = pharmacy.fullName.Replace (src, @"<font color='red'>" + src + @"</font>");
						rpl = rpl.Replace (srcWithCap, @"<font color='red'>" + srcWithCap + @"</font>");
						fullName.TextFormatted = Html.FromHtml (rpl);						
					}
					row.AddView (fullName);

					TextView shortName = new TextView (Activity);
					//					shortName.SetPadding(0, 0, 56, 0);0
					//					shortName.SetBackgroundResource(Resource.Drawable.bottomline);
					shortName.SetTextAppearance (Activity, Resource.Style.rowTextForPharmacy);
					shortName.SetHeight (48);
					shortName.Gravity = GravityFlags.CenterVertical;
					if (string.IsNullOrEmpty (src)) {
						shortName.Text = pharmacy.shortName;
					} else {
						rpl = pharmacy.shortName.Replace (src, @"<font color='red'>" + src + @"</font>");
						rpl = rpl.Replace (srcWithCap, @"<font color='red'>" + srcWithCap + @"</font>");
						shortName.TextFormatted = Html.FromHtml (rpl);						
					}
					row.AddView (shortName);

					TextView officialName = new TextView (Activity);
					//					officialName.SetPadding(0, 0, 56, 0);
					//					officialName.SetBackgroundResource(Resource.Drawable.bottomline);
					officialName.SetTextAppearance (Activity, Resource.Style.rowTextForPharmacy);
					officialName.SetHeight (48);
					officialName.Gravity = GravityFlags.CenterVertical;
					if (string.IsNullOrEmpty (src)) {
						officialName.Text = pharmacy.officialName;
					} else {
						rpl = pharmacy.officialName.Replace (src, @"<font color='red'>" + src + @"</font>");
						rpl = rpl.Replace (srcWithCap, @"<font color='red'>" + srcWithCap + @"</font>");
						officialName.TextFormatted = Html.FromHtml (rpl);						
					}
					row.AddView (officialName);

					TextView address = new TextView (Activity);
					//					address.SetTextAppearance (this.Activity, global::Android.Resource.Style.TextAppearanceLarge); //?android:attr/textAppearanceLarge			
					//					address.SetPadding(0, 0, 56, 0);
					//					address.SetTextAppearance (Activity, Resource.Style.rowTextForPharmacy);
					//					address.SetBackgroundResource(Resource.Drawable.bottomline);
					address.SetTextAppearance (Activity, Resource.Style.rowTextForPharmacy);
					address.SetHeight (48);
					address.Gravity = GravityFlags.CenterVertical;
					if (string.IsNullOrEmpty (src)) {
						address.Text = pharmacy.address;
					} else {
						rpl = pharmacy.address.Replace (src, @"<font color='red'>" + src + @"</font>");
						rpl = rpl.Replace (srcWithCap, @"<font color='red'>" + srcWithCap + @"</font>");
						address.TextFormatted = Html.FromHtml (rpl);						
					}
					row.AddView (address);

					TextView subway = new TextView (Activity);
					//					subway.SetPadding(0, 0, 56, 0);
					//					subway.SetBackgroundResource(Resource.Drawable.bottomline);
					subway.SetTextAppearance (Activity, Resource.Style.rowTextForPharmacy);
					subway.SetHeight (48);
					subway.Gravity = GravityFlags.CenterVertical;
					if (string.IsNullOrEmpty (src)) {
						subway.Text = pharmacy.subway;
					} else {
						rpl = pharmacy.subway.Replace (src, @"<font color='red'>" + src + @"</font>");
						rpl = rpl.Replace (srcWithCap, @"<font color='red'>" + srcWithCap + @"</font>");
						subway.TextFormatted = Html.FromHtml (rpl);						
					}
					row.AddView (subway);

					TextView phone = new TextView (Activity);
					//					phone.SetPadding(0, 0, 56, 0);
					//					phone.SetBackgroundResource(Resource.Drawable.bottomline);
					phone.SetTextAppearance (Activity, Resource.Style.rowTextForPharmacy);
					phone.SetHeight (48);
					phone.Gravity = GravityFlags.CenterVertical;
					if (string.IsNullOrEmpty (src)) {
						phone.Text = pharmacy.phone;
					} else {
						rpl = pharmacy.phone.Replace (src, @"<font color='red'>" + src + @"</font>");
						rpl = rpl.Replace (srcWithCap, @"<font color='red'>" + srcWithCap + @"</font>");
						phone.TextFormatted = Html.FromHtml (rpl);						
					}
					row.AddView (phone);

					TextView email = new TextView (Activity);
					//					email.SetPadding(0, 0, 56, 0);
					//					email.SetBackgroundResource(Resource.Drawable.bottomline);
					email.SetTextAppearance (Activity, Resource.Style.rowTextForPharmacy);
					email.SetHeight (48);
					email.Gravity = GravityFlags.CenterVertical;
					if (string.IsNullOrEmpty (src)) {
						email.Text = pharmacy.email;
					} else {
						rpl = pharmacy.email.Replace (src, @"<font color='red'>" + src + @"</font>");
						rpl = rpl.Replace (srcWithCap, @"<font color='red'>" + srcWithCap + @"</font>");
						email.TextFormatted = Html.FromHtml (rpl);						
					}
					row.AddView (email);

					//					Button delete = new Button (this.Activity);
					//					delete.SetTextAppearance (this.Activity, global::Android.Resource.Style.TextAppearanceLarge); //?android:attr/textAppearanceLarge			
					//					delete.SetPadding(0, 16, 24, 16);
					//					delete.Text = @"Del";
					//					delete.Id = pharmacy.id;
					//					delete.Click += PharmacyDelete_Click;
					//
					//					row.AddView (delete);

					pfPharmacyTable.AddView(row);
				}

			}
		}

		private void RefreshTableContent()
		{
			
			if (pfPharmacyTable != null) {
				int childCount = pfPharmacyTable.ChildCount;

				// Remove all rows except the first one
				if (childCount > 1) {
					pfPharmacyTable.RemoveViews(1, childCount - 1);
				}

				pharmacies = (List<Pharmacy>)PharmacyManager.GetPharmacies (pfSearchEdit.Text, 20);

				foreach (var pharmacy in pharmacies) {
					string src = pfSearchEdit.Text;
					string srcWithCap = UppercaseFirst(pfSearchEdit.Text);
					string rpl = "";

					TableRow row = new TableRow (Activity);
					row.SetBackgroundResource(Resource.Drawable.bottomline);


					TextView id = GetItem(ColumnPosition.cpFirst);
					id.Gravity = GravityFlags.Center;
					id.Text = pharmacy.id.ToString ();
					row.AddView (id);

					TextView fullName = GetItem(ColumnPosition.cpMiddle);
					fullName.Gravity = GravityFlags.CenterVertical;
					if (string.IsNullOrEmpty (src)) {
						fullName.Text = pharmacy.fullName;
					} else {
						rpl = pharmacy.fullName.Replace (src, @"<font color='red'>" + src + @"</font>");
						rpl = rpl.Replace (srcWithCap, @"<font color='red'>" + srcWithCap + @"</font>");
						fullName.TextFormatted = Html.FromHtml (rpl);						
					}
					row.AddView (fullName);

					TextView shortName = GetItem(ColumnPosition.cpMiddle);
					shortName.Gravity = GravityFlags.CenterVertical;
					if (string.IsNullOrEmpty (src)) {
						shortName.Text = pharmacy.shortName;
					} else {
						rpl = pharmacy.shortName.Replace (src, @"<font color='red'>" + src + @"</font>");
						rpl = rpl.Replace (srcWithCap, @"<font color='red'>" + srcWithCap + @"</font>");
						shortName.TextFormatted = Html.FromHtml (rpl);						
					}
					row.AddView (shortName);

					TextView officialName = GetItem(ColumnPosition.cpMiddle);
					officialName.Gravity = GravityFlags.CenterVertical;
					if (string.IsNullOrEmpty (src)) {
						officialName.Text = pharmacy.officialName;
					} else {
						rpl = pharmacy.officialName.Replace (src, @"<font color='red'>" + src + @"</font>");
						rpl = rpl.Replace (srcWithCap, @"<font color='red'>" + srcWithCap + @"</font>");
						officialName.TextFormatted = Html.FromHtml (rpl);						
					}
					row.AddView (officialName);

					TextView address = GetItem(ColumnPosition.cpMiddle);
					address.Gravity = GravityFlags.CenterVertical;
					if (string.IsNullOrEmpty (src)) {
						address.Text = pharmacy.address;
					} else {
						rpl = pharmacy.address.Replace (src, @"<font color='red'>" + src + @"</font>");
						rpl = rpl.Replace (srcWithCap, @"<font color='red'>" + srcWithCap + @"</font>");
						address.TextFormatted = Html.FromHtml (rpl);						
					}
					row.AddView (address);

					TextView subway = GetItem(ColumnPosition.cpMiddle);
					subway.Gravity = GravityFlags.CenterVertical;
					if (string.IsNullOrEmpty (src)) {
						subway.Text = pharmacy.subway;
					} else {
						rpl = pharmacy.subway.Replace (src, @"<font color='red'>" + src + @"</font>");
						rpl = rpl.Replace (srcWithCap, @"<font color='red'>" + srcWithCap + @"</font>");
						subway.TextFormatted = Html.FromHtml (rpl);						
					}
					row.AddView (subway);

					TextView phone = GetItem(ColumnPosition.cpMiddle);
					phone.Gravity = GravityFlags.CenterVertical;
					if (string.IsNullOrEmpty (src)) {
						phone.Text = pharmacy.phone;
					} else {
						rpl = pharmacy.phone.Replace (src, @"<font color='red'>" + src + @"</font>");
						rpl = rpl.Replace (srcWithCap, @"<font color='red'>" + srcWithCap + @"</font>");
						phone.TextFormatted = Html.FromHtml (rpl);						
					}
					row.AddView (phone);

					TextView email = GetItem(ColumnPosition.cpLast);
					email.Gravity = GravityFlags.CenterVertical;
					if (string.IsNullOrEmpty (src)) {
						email.Text = pharmacy.email;
					} else {
						rpl = pharmacy.email.Replace (src, @"<font color='red'>" + src + @"</font>");
						rpl = rpl.Replace (srcWithCap, @"<font color='red'>" + srcWithCap + @"</font>");
						email.TextFormatted = Html.FromHtml (rpl);						
					}
					row.AddView (email);

//					Button delete = new Button (this.Activity);
//					delete.SetTextAppearance (this.Activity, global::Android.Resource.Style.TextAppearanceLarge); //?android:attr/textAppearanceLarge			
//					delete.SetPadding(0, 16, 24, 16);
//					delete.Text = @"Del";
//					delete.Id = pharmacy.id;
//					delete.Click += PharmacyDelete_Click;
//
//					row.AddView (delete);

					pfPharmacyTable.AddView(row);
				}
//				SyncHeader ();
//				pfPharmacyTable.RemoveView (row2);
//				pfPharmacyTableHeader.AddView (row2);
//				pfContent.Add(row2);
//				pfContent.AddView(row2, 1, new ViewGroup.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent));
//				((ViewGroup) pfContent).AddView(row2, 1);
				//pfPharmacyTableHeaderRow = row2;
			}
		}

		void SyncHeader()
		{
			List<int> colWidths = new List<int> ();
			for ( int rownum = 0; rownum < pfPharmacyTable.ChildCount; rownum++ )
			{
				TableRow row = (TableRow) pfPharmacyTable.GetChildAt( rownum );
				for ( int cellnum = 0; cellnum < row.ChildCount; cellnum++ )
				{
					View cell = row.GetChildAt( cellnum );
					TableRow.LayoutParams p1 = (TableRow.LayoutParams)cell.LayoutParameters;
					int cellWidth = p1.Span == 1 ? cell.Width : 0;
					if ( colWidths.Count <= cellnum )
					{
						colWidths.Add( cellWidth );
					}
				}
			}
//					else
//					{
//						Integer current = colWidths.get( cellnum );
//						if( cellWidth &gt; current )
//						{
//							colWidths.remove( cellnum );
//							colWidths.add( cellnum, cellWidth );
//						}
//					}
//				}
//			}
//
			for ( int rownum = 0; rownum < pfPharmacyTableHeader.ChildCount; rownum++ )
			{
				TableRow row = (TableRow) pfPharmacyTableHeader.GetChildAt( rownum );
				for ( int cellnum = 0; cellnum < row.ChildCount; cellnum++ )
				{
					View cell = row.GetChildAt( cellnum );
					TableRow.LayoutParams p2 = (TableRow.LayoutParams)cell.LayoutParameters;
					p2.Width = colWidths [cellnum];
//					p2.Width = 0;
//					for( int span = 0; span &lt; params.span; span++ )
//					{
//						params.width += colWidths.get( cellnum + span );
//					}
				}
			}
		}

		TextView GetHeadItem (ColumnPosition columnPosition)
		{
			TextView textView = new TextView (Activity);
			textView.SetTextAppearance (Activity, Resource.Style.headerTextForPharmacy);
			textView.SetHeight (56);

			switch (columnPosition) {
			case ColumnPosition.cpFirst:
				textView.LayoutParameters = new TableRow.LayoutParams () { RightMargin = 24, LeftMargin = 24 };
				break;
			case ColumnPosition.cpMiddle:
				textView.LayoutParameters = new TableRow.LayoutParams () { RightMargin = 56 };
				break;		
			case ColumnPosition.cpLast:
				textView.LayoutParameters = new TableRow.LayoutParams () { LeftMargin = 24 };
				break;						
			default:
				break;
			}
			return textView;
		}

		TextView GetItem (ColumnPosition columnPosition)
		{
			TextView textView = new TextView (Activity);
			textView.SetTextAppearance (Activity, Resource.Style.rowTextForPharmacy);
			textView.SetHeight (48);
//			textView.

			switch (columnPosition) {
			case ColumnPosition.cpFirst:
				textView.LayoutParameters = new TableRow.LayoutParams () { RightMargin = 24, LeftMargin = 24 };
				break;
			case ColumnPosition.cpMiddle:
				textView.LayoutParameters = new TableRow.LayoutParams () { RightMargin = 56 };
				break;		
			case ColumnPosition.cpLast:
				textView.LayoutParameters = new TableRow.LayoutParams () { LeftMargin = 24 };
				break;						
			default:
				break;
			}
			return textView;
		}

		string UppercaseFirst(string s)
		{
			if (string.IsNullOrEmpty (s)) {
				return string.Empty;
			}

			//pfPharmacyTable.

			return char.ToUpper (s [0]) + s.Substring (1);
		}

		void PharmacyDelete_Click (object sender, EventArgs e)
		{
			int id = PharmacyManager.DeletePharmacy (((Button)sender).Id);
			Log.Info("PharmacyManager.DeletePharmacy", id.ToString());
			RefreshTableContent ();
		}

		void PharmacyDialog_SuccessSaved (object sender, EventArgs e)
		{
			RefreshTableContent ();
//			View view = this.Activity.CurrentFocus;
//			if (view != null) {
//				InputMethodManager imm = (InputMethodManager)this.Activity.GetSystemService (Context.InputMethodService);
//				imm.HideSoftInputFromWindow (view.WindowToken, 0);
//			}
		}

		public override void OnResume ()
		{
			base.OnResume ();
			RefreshTableContent ();
		}
	}
}

