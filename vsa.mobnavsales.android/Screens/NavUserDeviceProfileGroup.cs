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
using System.Data;

using vsa.mobnavsales.Library;


namespace vsa.mobnavsales.android.Screens
{
    [Activity(Label = "Setup User Device")]

    public class NavUserDeviceProfileGroup : Activity, TabHost.IOnTabChangeListener
    {
        protected override void OnCreate(Bundle bundle)
        {

            SetContentView(Resource.Layout.NavOffModeCompanyGroup);
            base.OnCreate(bundle);

            CreateTabs(bundle);

        }

        private void CreateTabs(Bundle bundle)
        {

            LocalActivityManager localActMgr = new LocalActivityManager(this, false);
            localActMgr.DispatchCreate(bundle);

            TabHost tabHost = FindViewById<TabHost>(Resource.Id.tabHost1);
            tabHost.Setup(localActMgr);

            TabHost.TabSpec tabSpec = null;
            Intent intent = new Intent();
            intent.SetFlags(ActivityFlags.NewTask);
            intent.SetClass(this, typeof(NavUserDeviceProfileList));
            tabSpec = tabHost.NewTabSpec("User Device List");
            tabSpec.SetContent(intent);
            tabSpec.SetIndicator("", Resources.GetDrawable(Resource.Drawable.icon_button_round_yellow1));
            tabHost.AddTab(tabSpec);

            intent = new Intent();
            intent.SetFlags(ActivityFlags.NewTask);
            intent.SetClass(this, typeof(NavUserDeviceProfile));
            tabSpec = tabHost.NewTabSpec("User Device Edit");
            tabSpec.SetContent(intent);
            tabSpec.SetIndicator("", Resources.GetDrawable(Resource.Drawable.icon_edit));
            tabHost.AddTab(tabSpec);

            tabHost.SetOnTabChangedListener(this);

        }

        void TabHost.IOnTabChangeListener.OnTabChanged(string tabId)
        {

        }
    }
}