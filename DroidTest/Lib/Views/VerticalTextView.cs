
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
using Android.Text;

namespace DroidTest
{
	[Register("VerticalTextView")]
	public class VerticalTextView : TextView
	{
		private bool TopDown = false;

		public VerticalTextView (Context context, IAttributeSet attrs) :
			base (context, attrs)
		{
//			Initialize ();

			;
			//int gravity = this.Gravity
			if ( Android.Views.Gravity.IsVertical ( Gravity )
				&& ( Gravity & Android.Views.GravityFlags.VerticalGravityMask ) 
				== Android.Views.GravityFlags.Bottom )
			{
				Gravity = (
					( Gravity & Android.Views.GravityFlags.HorizontalGravityMask )
					| Android.Views.GravityFlags.Top );
				TopDown = false;
			}
			else
			{
				TopDown = true;
			}
		}

		protected override void OnMeasure (int widthMeasureSpec, int heightMeasureSpec)
		{
			base.OnMeasure (widthMeasureSpec, heightMeasureSpec);
			SetMeasuredDimension (MeasuredHeight, MeasuredWidth);
		}

		protected override void OnDraw (Android.Graphics.Canvas canvas)
		{
			base.OnDraw (canvas);

			TextPaint textPaint = Paint;
			textPaint.Color = new Android.Graphics.Color(CurrentTextColor);
			textPaint.DrawableState = GetDrawableState ();

			canvas.Save();

			if ( TopDown )
			{
				canvas.Translate( Width, 0 );
				canvas.Rotate( 90 );
			}
			else
			{
				canvas.Translate( 0, Height );
				canvas.Rotate( -90 );
			}

			canvas.Translate (CompoundPaddingLeft, ExtendedPaddingTop);

			Layout.Draw (canvas);
//			getLayout().draw( canvas );
			canvas.Restore ();
//			canvas.restore();
		}
	}
}

