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
using Android.Widget;
using Android.Graphics;
using vsa.mobnavsales.Library;


namespace vsa.mobnavsales.android.Screens
{
    [Activity(Label = "Edit Offline Company Setup")]

    public class NavOffModeCompanyList : Activity
    {
        string[] items;
        IFungsiDB db = null;
        DataTable dt = null;
        DataReceiver broadcastRecv;
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.NavOffModeCompanyList);
            if (db == null)
            {
                db = Common.setupDatabase();
            }

            
            //Load Data from offlinemodecompany
            Button btnAdd = FindViewById<Button>(Resource.Id.BtnAdd);
            btnAdd.Click += btnAdd_Click;

            LoadData();
            broadcastRecv = new DataReceiver();
            broadcastRecv.actionRecv += OnDataReceived;
            RegisterReceiver(broadcastRecv, new IntentFilter(Common.ACTION_REFRESH_DATA));

        }

        protected override void OnResume()
        {
            base.OnResume();
            RegisterReceiver(broadcastRecv, new IntentFilter(Common.ACTION_REFRESH_DATA));
        }

        protected override void OnPause()
        {
            base.OnPause();
            UnregisterReceiver(broadcastRecv);
        }

        void OnDataReceived(Context arg1, Intent arg2)
        {
            if (arg2 != null)
            {
                bool isRefresh = arg2.GetBooleanExtra("Refresh", true);
                if (isRefresh) LoadData();
            }
        }


        void btnAdd_Click(object sender, EventArgs e)
        {
            Activity tabs = (Activity)this.Parent;
            TabHost tabHost = tabs.FindViewById<TabHost>(Resource.Id.tabHost1);
            tabHost.CurrentTab = 1;
            SendData(-1);
        }
        void SendData(int EditID)
        {
            var NewData = new Intent(Common.ACTION_NEW_DATA);
            NewData.PutExtra("EditID", EditID);
            SendBroadcast(NewData);
        }

        private void LoadData()
        {
            try
            {
                ListView lv = FindViewById<ListView>(Resource.Id.lstCompany);
                dt = db.RetrieveData("select * from OfflineModeCompany");
                if (dt != null && dt.Rows.Count > 0)
                {
                    NavOffModeCompanyAdapt ListAdapter = new NavOffModeCompanyAdapt(this, dt);
                    lv.Adapter = ListAdapter;
                    lv.ItemClick += lv_ItemClick;
                }
            }
            catch (Exception ex)
            {
                Toast a = Toast.MakeText(this.BaseContext, ex.Message + ":" + ex.StackTrace, ToastLength.Short);
                a.Show();
            }
        }

        void lv_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            if (dt == null) return;
            var t = dt.Rows[e.Position];
            Activity tabs = (Activity)this.Parent;
            TabHost tabHost = tabs.FindViewById<TabHost>(Resource.Id.tabHost1);
            int EditID = Convert.ToInt32(t["id"]);
            tabHost.CurrentTab = 1;
            SendData(EditID);
        }


    }
}