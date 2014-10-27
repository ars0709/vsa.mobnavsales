using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Graphics;

namespace vsa.mobnavsales.android.Screens
{
    [Activity(Label = "NavAdmin")]
    public class NavAdmin : Activity
    {
        protected ImageButton ximgsetup = null;
        protected ImageButton ximgoffline = null;
        protected ImageButton xuserprofile = null;
        protected ImageButton xinvoice = null;


        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Create your application here

            View titleView = Window.FindViewById(Android.Resource.Id.Title);
            if (titleView != null)
            {

 
                IViewParent parent = titleView.Parent;
                if (parent != null && (parent is View))
                {
                    View parentView = (View)parent;
                    parentView.SetBackgroundColor(Color.Rgb(0x26, 0x75, 0xFF)); //38, 117 ,255
                }
            }


            // set our layout to be the home screen
            SetContentView(Resource.Layout.navadmin);

            ximgsetup = FindViewById<ImageButton>(Resource.Id.imgbtn1);
            ximgsetup.Click += (sender, e) =>
            {
                StartActivity(typeof(navsetup));
            };

            ximgoffline = FindViewById<ImageButton>(Resource.Id.imgbtn2);
            ximgoffline.Click += (sender, e) =>
            {
                StartActivity(typeof(navsetup));
            };

        }
    }
}