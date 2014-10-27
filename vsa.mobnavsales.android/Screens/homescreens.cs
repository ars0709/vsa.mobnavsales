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
using Mono.Data.Sqlite;
using System.Threading.Tasks;
using System.Net;

namespace vsa.mobnavsales.android.Screens
{
    [Activity(Label = "Mobile Navsales Main", MainLauncher = true, Icon="@drawable/icon")]
    public class homescreens : Activity
    {

            protected Button xbtnsetup = null;
            protected Button xbtnlogin = null;
            protected EditText xtxtpass = null;
            protected EditText xtxtuser = null;


            string[] items;
            IFungsiDB db = null;
            DataTable dt = null;
            string v_message = string.Empty;
            string g_strDomain;
            string g_strUsername;
            string g_strPassword;

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
                SetContentView(Resource.Layout.Main);

                if (db == null)
                {
                    db = Common.setupDatabase();
                }


                xbtnsetup = FindViewById<Button>(Resource.Id.btnsetup);
                xbtnlogin = FindViewById<Button>(Resource.Id.btnlogin);
                xtxtpass = FindViewById<EditText>(Resource.Id.txtpass);
                xtxtuser = FindViewById<EditText>(Resource.Id.txtuserid);

                xbtnlogin.Click += xbtnlogin_click;
                
                //click setup button 
                xbtnsetup.Click += (sender, e) =>
                    {
                        StartActivity(typeof(NavAdmin));
                    };


            }
               
                
                /* xbtnsetup.Click += (sender, e) =>
                {
                    StartActivity(typeof(adminsetup));
                };*/

               private void xbtnlogin_click(object sender, EventArgs e)
                {
                 
                        if (string.IsNullOrEmpty(xtxtuser.Text))
                        {
                             //-1- check if MSC Code is entered in txtPwd
                             if (xtxtpass.Text.Length  > 0)
                                 {
                                     //-2- check the MSC Code
                                     ProcessMSC_Code(xtxtpass.Text);
                                 }
                        }
                        else
                        {
                                //-- at here, Username is entered.

                                //-- check if PWD is entered
                                if (xtxtpass.Text.Length > 0)
                                {
                         
                                xbtnlogin.Enabled  = false;

                                //-1- check If NAV Credential setUp in Local DB
                                CheckNAVCredentialDBSetUp();

                                //-2- check internet connection and Login
                                CheckForInternetConnection();

                                }
                                else
                                {
                                v_message = "Please enter Password (请输入密码), Password (密码)";
                                    Toast a = Toast.MakeText(this.BaseContext, v_message, ToastLength.Short);
                                    a.Show();
                                
                                }
                        }

                   
                }

             

                private void  ProcessMSC_Code(string strPwdName)
                {
                    //-- must be exact:
                    if (strPwdName.ToLower().Contains("msc1:"))
                    {
                       

                        string[] strParts = strPwdName.Split(':');
                        string strMain1 = strParts[0];
                        string strMain2 = strParts[1];

                        if (strMain1.ToLower() == "msc1")
                        {
                            if (strMain2.ToLower() == "db123" || strMain2 == "Db123")
                            {
                                StartActivity(typeof(NavAdmin));
                            }
                            else if (strMain2.ToLower() == "1688" || strMain2 == "1688")
                            {
                                StartActivity(typeof(NavAdmin));
                            }
                            else
                            {
                                //-- allow to setup Nav Configuration here:
                                xbtnsetup.Enabled =true ;
                               
                            }
                        }
                        else
                        {
                            //RingProgress.IsActive = false;
                            xbtnsetup.Enabled = true;
                        }
                    }
                    else
                    {
                        //RingProgress.IsActive = false;
                        xbtnsetup.Enabled = true;
                    }
                } //end
               
   

              private   void CheckNAVCredentialDBSetUp()
              {
                  
                  try
                  {
                       

                      DataTable dt = db.RetrieveData("select * from NavNetworkCredential");
                      if (dt != null && dt.Rows.Count > 0)
                      {
                          foreach (DataRow dr in dt.Rows)
                          {
                              xbtnlogin.Enabled = true;

                              break;
                          }
                      }
                  }
                  catch (Exception ex)
                  {
                      Toast a = Toast.MakeText(this.BaseContext, ex.Message + ":" + ex.StackTrace, ToastLength.Short);
                      a.Show();
                  }
              }//end


             private async void CheckForInternetConnection()
              {

                  EditText xtxtstatus = FindViewById<EditText>(Resource.Id.txtstatus);

                  bool Connected = await CheckForConnection();

                  if (Connected)
                  {
                    
                      xtxtstatus.Text = "Connected to Internet (连接到互联网).";

                      //ImgConnection.Visibility = Visibility.Visible;
                     // ImgConnection.Source = GetImage("ms-appx:///Icons/Connected.png");

                      //RingProgress.IsActive = false;

                      string strPwd = xtxtpass.Text;

                      //LoginNow(strPwd);


                  }
                  else
                  {

                      //RingProgress.IsActive = false;

                      xtxtstatus.Text = "Not Connected (没有连接到互联网)";

                      //ImgConnection.Visibility = Visibility.Visible;
                      //ImgConnection.Source = GetImage("ms-appx:///Icons/ConnectedFalse.png");

                  }
              }



             private async Task<bool> CheckForConnection()
              {
                  //--original
                  // bool isConnected = await IsConnectedToInternet();

                  Task<bool> ConnectTask = IsConnectedToInternet();

                  bool isConnected = await ConnectTask;

                  if (isConnected)
                  {
                      return true;
                  }
                  else
                  {
                      return false;
                  }
              } //end


            private async Task<bool> IsConnectedToInternet()
            {
                HttpWebRequest webReq;
                HttpWebResponse resp = null;
                Uri url = null;

                url = new Uri("http://google.com");
                webReq = (HttpWebRequest)WebRequest.Create(url);

                try
                {
                    resp = (HttpWebResponse)await webReq.GetResponseAsync();
                    webReq.Abort();
                    webReq = null;
                    url = null;
                    resp = null;

                    return true;
                }
                catch
                {
                    webReq.Abort();
                    webReq = null;

                    return false;
                }
            }




            }//end

       
 
    }  
    
    
    
 
    
         





        
      
              



            

              






            

          

             
 