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
    public class ViewAdapt : BaseAdapter<DataRow>
    {
        DataTable items;
        Activity context;
        public ViewAdapt(Activity context, DataTable items)
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
                view = context.LayoutInflater.Inflate(Resource.Layout.ViewAdapt, null);
            view.FindViewById<TextView>(Resource.Id.textView1).Text = items.Rows[position]["NavWebServiceName"].ToString();
            view.FindViewById<TextView>(Resource.Id.textView2).Text = items.Rows[position]["NavWebServer"].ToString();
            view.FindViewById<TextView>(Resource.Id.textView3).Text = items.Rows[position]["NavPort"].ToString();
            return view;
        }
    }
}