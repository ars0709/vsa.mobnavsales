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


namespace vsa.mobnavsales.android 
{
    [BroadcastReceiver]
    public class DataReceiver : BroadcastReceiver
    {
        public event Action<Context, Intent> actionRecv;

        #region implemented abstract members of BroadcastReceiver
        public override void OnReceive(Context context, Intent intent)
        {
            if (this.actionRecv != null)
            {
                this.actionRecv(context, intent);
            }
        }
        #endregion
    }
}


 