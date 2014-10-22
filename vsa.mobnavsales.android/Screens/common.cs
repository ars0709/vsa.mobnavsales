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
using vsa.mobnavsales.Library;

namespace vsa.mobnavsales.android
{
    class Common
    {
        public const string ACTION_NEW_DATA = "DATASEND";
        public const string ACTION_REFRESH_DATA = "DATAREFRESH";

        public static DBSqliteFunction  setupDatabase()
        {
            DBSqliteFunction db = new DBSqliteFunction(DBSqliteFunction.GetDBFilePath("MobNavSales.db3"));
            if (!db.isDBExists)
            {
                db.ExecuteNonQuery("CREATE TABLE [NavNetworkCredential] (id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, NavWebServiceName NTEXT,NavWebServer NTEXT,NavPort INT,NavCompany NTEXT, Note NTEXT, NavDefaultCredential INT, NavFormAuthentication INT, NavLoginDomain NTEXT, NavUserName NTEXT , NavUserPwd NTEXT, UseSetupCompany INT);");
            }
            return db;
        }
    }
}