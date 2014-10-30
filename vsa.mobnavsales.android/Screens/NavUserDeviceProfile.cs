using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

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
    [Activity(Label = "User Credential Setup")]
 
     public class NavUserDeviceProfile : Activity
    {

        IFungsiDB db = null;
        DataReceiver broadcastRecv;
        int EditID = -1;
        protected EditText xtxtcompany;
        protected EditText xtxtnote;

         

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


            SetContentView(Resource.Layout.NavUserDeviceProfile);
            Button xbtnconfirm = FindViewById<Button>(Resource.Id.btnconfirm);

            xbtnconfirm.Click += xbtnconfirm_click;
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
            EditText xtxtcompany = FindViewById<EditText>(Resource.Id.txtcompanyname);
            EditText xtxtnote = FindViewById<EditText>(Resource.Id.txtnote);
            xtxtcompany.Text = string.Empty;
            xtxtnote.Text = string.Empty;

        }

        private void xbtnconfirm_click(object sender, EventArgs e)
        {

            CheckDataEnterByUser();             
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

        
       

        private   void CheckDataEnterByUser()
        {
            xtxtcompany = FindViewById<EditText>(Resource.Id.txtcompanyname);

            //-- check important fields:
            if (string.IsNullOrEmpty(xtxtcompany.Text))
            {
                  Toast a = Toast.MakeText(this.BaseContext,"Please enter offline company for offline mode transaction, Offline Company name required", ToastLength.Short);
                     a.Show();
            }
 
            else
            {

                CheckDuplicateCompany();

               

            }
        }



        private  void CheckDuplicateCompany()
        {

            //-- Dont trim As user can enter space in the front 
            EditText xtxtcompany = FindViewById<EditText>(Resource.Id.txtcompanyname);

            string Cpy = xtxtcompany.Text;
            DataTable dt = db.RetrieveData("select * from OfflineModeCompany where OfflineCompanyName='" + xtxtcompany.Text + "'");
            if (dt != null && dt.Rows.Count > 0)
            {
                Toast a = Toast.MakeText(this.BaseContext, "Please enter a different company name.Duplicate Offline Mode Company", ToastLength.Short);
                 a.Show() ;
            }
            else
            {
                //add company
                AddCompany();

            }  
         }
        
        
         private void AddCompany()
        {
            bool v_result = false;
            TabHost host1 = FindViewById<TabHost>(Resource.Id.tabHost1);
            xtxtcompany = FindViewById<EditText>(Resource.Id.txtcompanyname);
            xtxtnote = FindViewById<EditText>(Resource.Id.txtnote);
      

       


            if (EditID < 0) 
            {
                //New Data 
                string[] FieldName = { "OfflineCompanyName", "Note" };
                object[] FieldData = { xtxtcompany.Text, xtxtnote.Text };
                v_result = db.InsertData("OfflineModeCompany", FieldName, FieldData);

                //v_message = "This company : " + xtxtcompany.Text + " has been created. You can add another company.Offline Mode Company Created";


                //RefreshData(true);
            }
            else
            {
                string[] FieldNameID = { "id" };
                object[] FieldDataID = { EditID };
                string[] FieldName = { "OfflineCompanyName", "Note" };
                object[] FieldData = { xtxtcompany.Text, xtxtnote.Text };
                v_result = db.UpdateRecord("OfflineModeCompany", FieldNameID, FieldDataID,FieldName, FieldData);
       
              //  v_message = "This company : " + xtxtcompany.Text + " has been Failed.Please Try Again";
            }

            string v_message = string.Empty;

            if (v_result)
            {
                v_message = "This company : " + xtxtcompany.Text + " has been created. You can add another company.Offline Mode Company Created";
                Activity tabs = (Activity)this.Parent;
                TabHost tabHost = tabs.FindViewById<TabHost>(Resource.Id.tabHost1);
                tabHost.CurrentTab = 0;
                RefreshData(true);
            }
            else
            {
                v_message = "This company : " + xtxtcompany.Text + " has been Failed.Please Try Again";
            }
            Toast a = Toast.MakeText(this.BaseContext, v_message, ToastLength.Short);
            a.Show();

        }



         private void LoadDetail()
         {
             try
             {
                 xtxtcompany = FindViewById<EditText>(Resource.Id.txtcompanyname);
                 xtxtnote = FindViewById<EditText>(Resource.Id.txtnote);


                 DataTable dt = db.RetrieveData("select * from OfflineModeCompany");
                 if (dt != null && dt.Rows.Count > 0)
                 {
                     foreach (DataRow dr in dt.Rows)
                     {
                         EditID = Convert.ToInt32(dr["id"].ToString());
                         xtxtcompany.Text = dr["OfflineCompanyName"].ToString();
                         xtxtnote.Text = dr["Note"].ToString();


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