using System;
using System.Data;

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
 


namespace vsa.mobnavsales.android
{
    [Activity(Label = "Navision Credential Setup")]
    public class navsetup : Activity
    {

        IFungsiDB db = null;
        DataTable dt = null;
        DataReceiver broadcastRecv;
        int EditID = -1;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

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
            SetContentView(Resource.Layout.NavSetup);
            Button xbtnsave = FindViewById<Button>(Resource.Id.btnsave);
            
            xbtnsave.Click += xbtnsave_click;
              if (db == null)
            {
                db = Common.setupDatabase();
            }


            broadcastRecv = new DataReceiver();
            broadcastRecv.actionRecv += OnDataReceived;
            RegisterReceiver(broadcastRecv, new IntentFilter(Common.ACTION_NEW_DATA));
        }


        void cleanobject()
        {
            EditText xtxtws = FindViewById<EditText>(Resource.Id.txtwebservice);
            EditText xtxtwserver = FindViewById<EditText>(Resource.Id.txtwebserver);
            EditText xtxtwport = FindViewById<EditText>(Resource.Id.txtport);
            xtxtws.Text = string.Empty;
            xtxtwserver.Text = string.Empty;
            xtxtwport.Text = string.Empty;

        }

        protected override void OnResume()
        {
            base.OnResume();
            RegisterReceiver(broadcastRecv, new IntentFilter(Common.ACTION_NEW_DATA));
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
                EditID = arg2.GetIntExtra("EditID", -1);
                if (EditID <= 0)
                    cleanobject();
                else
                    LoadDetail();

            }
        }

        void RefreshData(bool State)
        {
            var NewData = new Intent(Common.ACTION_REFRESH_DATA);
            NewData.PutExtra("Refresh", State);
            SendBroadcast(NewData);
        }


        private void xbtnsave_click(object sender, System.EventArgs e)
        {
 	         bool v_result = false;

            EditText xtxtws = FindViewById<EditText>(Resource.Id.txtwebservice);
            EditText xtxtwserver = FindViewById<EditText>(Resource.Id.txtwebserver);
            EditText xtxtwport = FindViewById<EditText>(Resource.Id.txtport);

            if (EditID < 0)
            {
                //new data
                string[] FieldName = { "NavWebServiceName", "NavWebServer", "NavPort" };
                object[] FieldData = { xtxtws.Text, xtxtwserver.Text, xtxtwport.Text };
                v_result = db.InsertData("NavNetworkCredential", FieldName, FieldData);

            }
            else
            {
                //update
                string[] FieldNameID = { "_ID" };
                object[] FieldDataID = { EditID };
                string[] FieldName = { "NavWebServiceName", "NavWebServer", "NavPort" };
                object[] FieldData = { xtxtws.Text, xtxtwserver.Text, xtxtwport.Text };
                v_result = db.UpdateRecord("NavNetworkCredential", FieldNameID, FieldDataID, FieldName, FieldData);
            }

            string v_message = string.Empty;
            if (v_result)
            {
                v_message = "Nav Network Credential Created.";
                Activity tabs = (Activity)this.Parent;
               
                RefreshData(true);
            }
            else
            {
                v_message = "Nav Network Credential Failed";
            }
            Toast a = Toast.MakeText(this.BaseContext, v_message, ToastLength.Short);
            a.Show();
        }



        private void LoadDetail()
        {
            try
            {
                EditText xtxtws = FindViewById<EditText>(Resource.Id.txtwebservice);
                EditText xtxtwserver = FindViewById<EditText>(Resource.Id.txtwebserver);
                EditText xtxtwport = FindViewById<EditText>(Resource.Id.txtport);

                DataTable dt = db.RetrieveData("select * from NavNetworkCredential where id=" + EditID);
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        xtxtws.Text = dr["NavWebServiceName"].ToString();
                        xtxtwserver.Text = dr["NavWebServer"].ToString();
                        xtxtwport.Text = dr["NavPort"].ToString();
                        
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                Toast a = Toast.MakeText(this.BaseContext, ex.Message + ":" + ex.StackTrace, ToastLength.Short);
                a.Show();
            }
        }

     

      
       


    }
}