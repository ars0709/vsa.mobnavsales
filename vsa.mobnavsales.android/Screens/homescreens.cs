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
using vsa.mobnavsales.Library;




namespace vsa.mobnavsales.android.Screens
{
    [Activity(Label = "Mobile Navsales Main", MainLauncher = true, Icon="@drawable/icon")]
    public class homescreens : Activity
    {
        protected Button xbtnsetup = null;
        IFungsiDB db = null;
       

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
            SetContentView(Resource.Layout.Main);

            if (db == null)
            {
                db = Common.setupDatabase();
            }


            xbtnsetup = FindViewById<Button>(Resource.Id.btnsetup);

            // wire up add task button handler
            if (xbtnsetup != null)
            {
                xbtnsetup.Click += (sender, e) =>
                {
                    StartActivity(typeof(navsetup));
                };
            }
			


        }
    }
}