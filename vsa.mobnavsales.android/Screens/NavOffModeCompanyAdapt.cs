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
namespace vsa.mobnavsales.android.Screens
{
    public class NavOffModeCompanyAdapt : BaseAdapter<DataRow>
    {
        DataTable items;
        Activity context;
        public NavOffModeCompanyAdapt(Activity context, DataTable items)
            : base()
        {
            this.context = context;
            this.items = items;
        }
        public override long GetItemId(int position)
        {
            return position;
        }
        public override DataRow this[int position]
        {
            get { return items.Rows[position]; }
        }
        public override int Count
        {
            get { return items.Rows.Count; }
        }
        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View view = convertView; // re-use an existing view, if one is available
            if (view == null) // otherwise create a new one
                view = context.LayoutInflater.Inflate(Resource.Layout.NavOffModeCompanyItem, null);
            view.FindViewById<TextView>(Resource.Id.dtCompanyName).Text = items.Rows[position]["OfflineCompanyName"].ToString();
            view.FindViewById<TextView>(Resource.Id.dtCompanyNote).Text = items.Rows[position]["Note"].ToString();
            view.FindViewById<ImageView>(Resource.Id.dtEdit).SetImageResource(Resource.Drawable.icon_edit);            
            return view;
        }
    }
}