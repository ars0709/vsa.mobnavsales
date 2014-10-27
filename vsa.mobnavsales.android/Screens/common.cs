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
                //db.ExecuteNonQuery("CREATE TABLE IF NOT EXISTS [NavNetworkCredential] (id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, NavWebServiceName NTEXT,NavWebServer NTEXT,NavPort INT,NavCompany NTEXT, Note NTEXT, NavDefaultCredential INT, NavFormAuthentication INT, NavLoginDomain NTEXT, NavUserName NTEXT , NavUserPwd NTEXT, UseSetupCompany INT);");
                //db.ExecuteNonQuery("CREATE TABLE IF NOT EXISTS [OfflineModeCompany] (id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, OfflineCompanyName NTEXT, NOTE NTEXT);");

            }
            else
            {
                db.ExecuteNonQuery("CREATE TABLE IF NOT EXISTS [NavNetworkCredential] (id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, NavWebServiceName NTEXT,NavWebServer NTEXT,NavPort INT,NavCompany NTEXT, Note NTEXT, NavDefaultCredential INT, NavFormAuthentication INT, NavLoginDomain NTEXT, NavUserName NTEXT , NavUserPwd NTEXT, UseSetupCompany INT);");
                db.ExecuteNonQuery("CREATE TABLE IF NOT EXISTS [OfflineModeCompany] (id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, OfflineCompanyName NTEXT, NOTE NTEXT);");
                db.ExecuteNonQuery("CREATE TABLE IF NOT EXISTS [BalanceRemainder] (id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, bid INTEGER,cashid INTEGER , ReceiveDate Datetime, BalanceReceived DECIMAL, PaymentDate Datetime);");
               // db.ExecuteNonQuery("CREATE TABLE IF NOT EXISTS [CashReceive] (id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL)");

            }


            return db;
        }

            
    }
}