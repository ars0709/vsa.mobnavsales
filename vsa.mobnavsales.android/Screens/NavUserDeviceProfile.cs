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
using Android.Telephony;
 

using vsa.mobnavsales.Library;


namespace vsa.mobnavsales.android.Screens
{
    [Activity(Label = "User Credential Setup")]

    public class NavUserDeviceProfile : Activity
    {

        IFungsiDB db = null;
        DataReceiver broadcastRecv;
        int EditID = -1;

        protected EditText xtxtdomain;
        protected EditText xtxtdomainname;
        protected EditText xtxtuser;
        protected EditText xtxtpass;
        protected EditText xtxtnric;
        protected EditText xtxtemployee;
        string g_strHardwareNtwID;
        string g_OfflineBillBeginNbr;


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

            g_strHardwareNtwID = GetHardwareNetID();


            broadcastRecv = new DataReceiver();
            broadcastRecv.actionRecv += OnDataReceived;
            RegisterReceiver(broadcastRecv, new IntentFilter(Common.ACTION_NEW_DATA));

        }


        private string GetHardwareNetID()
        {
            TelephonyManager telephonyManager = (TelephonyManager)GetSystemService(Context.TelephonyService);


            //takes the first network adapter
            string xDeviceId = telephonyManager.DeviceId.ToString();
            return xDeviceId;

        }


        void cleanobject()
        {
            xtxtdomain = FindViewById<EditText>(Resource.Id.txtdomain);
            xtxtdomainname = FindViewById<EditText>(Resource.Id.txtdomainuser);
            xtxtpass = FindViewById<EditText>(Resource.Id.txtpass);
            xtxtemployee = FindViewById<EditText>(Resource.Id.txtemployee);
            xtxtnric = FindViewById<EditText>(Resource.Id.txtnric);
            xtxtdomain.Text = string.Empty;
            xtxtdomainname.Text = string.Empty;
            xtxtpass.Text = string.Empty;
            xtxtemployee.Text = string.Empty;
            xtxtnric.Text = string.Empty;



        }

        private void xbtnconfirm_click(object sender, EventArgs e)
        {

            if (string.IsNullOrEmpty(g_strHardwareNtwID))
            {
                Toast a = Toast.MakeText(this.BaseContext, "Please get ID first.,No Device Id", ToastLength.Short);
                a.Show();
            }

            else
            {

                CheckDataEnterByUser();



            }
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




        private void CheckDataEnterByUser()
        {

            bool ShowError = false;
            System.Exception MyException = new Exception();

            xtxtdomain = FindViewById<EditText>(Resource.Id.txtdomain);
            xtxtdomainname = FindViewById<EditText>(Resource.Id.txtdomainuser);
            xtxtpass = FindViewById<EditText>(Resource.Id.txtpass);
            xtxtemployee = FindViewById<EditText>(Resource.Id.txtemployee);
            xtxtnric = FindViewById<EditText>(Resource.Id.txtnric);

            //-- check important fields:
            try
            {


                if (string.IsNullOrEmpty(xtxtdomain.Text) || string.IsNullOrEmpty(xtxtdomainname.Text) || string.IsNullOrEmpty(xtxtpass.Text) || string.IsNullOrEmpty(xtxtemployee.Text))
                {
                    Toast a = Toast.MakeText(this.BaseContext, "Please enter all required fields. NRIC is optional,Enter required fields", ToastLength.Short);
                    a.Show();
                }

                else
                {

                    string[] strArrayId = g_strHardwareNtwID.Split('-');

                    g_OfflineBillBeginNbr = strArrayId[0];

                    AddDeviceUser();



                }
            }
            catch (Exception ex)
            {
                ShowError = true;
                MyException = ex;
            }
            if (ShowError)
            {
                Toast a = Toast.MakeText(this.BaseContext, "Encountered error: " + MyException.Message + "Device and User Info", ToastLength.Short);
                a.Show();

            }
        }







        private void AddDeviceUser()
        {
            bool v_result = false;
            string strUserDevice = "Device100";
            TabHost host1 = FindViewById<TabHost>(Resource.Id.tabHost1);

            xtxtdomain = FindViewById<EditText>(Resource.Id.txtdomain);
            xtxtdomainname = FindViewById<EditText>(Resource.Id.txtdomainuser);
            xtxtpass = FindViewById<EditText>(Resource.Id.txtpass);
            xtxtemployee = FindViewById<EditText>(Resource.Id.txtemployee);
            xtxtnric = FindViewById<EditText>(Resource.Id.txtnric);




            if (EditID < 0)
            {
                //New Data 
                string[] FieldName = { "Username", "EmployeeName", "NRIC", "UserLoginDomain", "UserPwd", "DeviceId", "OfflineBillNo" };
                object[] FieldData = { xtxtdomainname.Text, xtxtemployee.Text, xtxtnric.Text, xtxtdomain.Text, xtxtpass.Text, g_strHardwareNtwID, g_OfflineBillBeginNbr };
                v_result = db.InsertData("DeviceUser", FieldName, FieldData);


            }
            else
            {
                string[] FieldNameID = { "id" };
                object[] FieldDataID = { EditID };
                string[] FieldName = { "Username", "EmployeeName", "NRIC", "UserLoginDomain", "UserPwd", "DeviceId", "OfflineBillNo" };
                object[] FieldData = { xtxtdomainname.Text, xtxtemployee.Text, xtxtnric.Text, xtxtdomain.Text, xtxtpass.Text, g_strHardwareNtwID, g_OfflineBillBeginNbr };
                v_result = db.UpdateRecord("DeviceUser", FieldNameID, FieldDataID, FieldName, FieldData);


            }

            string v_message = string.Empty;

            if (v_result)
            {
                v_message = "This Device User : " + xtxtdomainname.Text + " has been created. Quick Access User Profile Created";
                Activity tabs = (Activity)this.Parent;
                TabHost tabHost = tabs.FindViewById<TabHost>(Resource.Id.tabHost1);
                tabHost.CurrentTab = 0;
                RefreshData(true);
            }
            else
            {
                v_message = "This Device user : " + xtxtdomainname.Text + " has been Failed.Please Try Again";
            }
            Toast a = Toast.MakeText(this.BaseContext, v_message, ToastLength.Short);
            a.Show();

        }



        private void LoadDetail()
        {
            try
            {
                xtxtdomain = FindViewById<EditText>(Resource.Id.txtdomain);
                xtxtdomainname = FindViewById<EditText>(Resource.Id.txtdomainuser);
                xtxtpass = FindViewById<EditText>(Resource.Id.txtpass);
                xtxtemployee = FindViewById<EditText>(Resource.Id.txtemployee);
                xtxtnric = FindViewById<EditText>(Resource.Id.txtnric);



                DataTable dt = db.RetrieveData("select * from DeviceUser");
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        EditID = Convert.ToInt32(dr["id"].ToString());
                        xtxtdomain.Text = dr["UserLoginDomain"].ToString();
                        xtxtdomainname.Text = dr["Username"].ToString();
                        xtxtpass.Text = dr["UserPwd"].ToString();
                        xtxtemployee.Text = dr["EmployeeName"].ToString();
                        xtxtnric.Text = dr["NRIC"].ToString();



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