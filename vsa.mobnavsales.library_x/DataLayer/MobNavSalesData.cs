using System;
using System.Linq;
using vsa.mobnavsales.library.BussinessLogic;
using System.Collections.Generic;

using Mono.Data.Sqlite;
using System.IO;

namespace vsa.mobnavsales.library.DataLayer
{
     public class MobNavSalesData
    {
         static object locker = new object();
         public SqliteConnection connection;
         public string path;

		/// <summary>
		 
		/// if the database doesn't exist, it will create the database and all the tables.
		/// </summary>
		/// <param name='path'>
		/// Path.
		/// </param>
         public MobNavSalesData(string dbPath) 
		    {
			var output = "";
			path = dbPath;
			// create the tables
			bool exists = File.Exists (dbPath);

			if (!exists) {
				connection = new SqliteConnection ("Data Source=" + dbPath);

				connection.Open ();
				var commands = new[] {
					"CREATE TABLE [NavNetworkCredential] (id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, NavWebServiceName NTEXT,NavWebServer NTEXT,NavPort INT,NavCompany NTEXT, Note NTEXT, NavDefaultCredential INT, NavFormAuthentication INT, NavLoginDomain NTEXT, NavUserName NTEXT , NavUserPwd NTEXT, UseSetupCompany INT);",
                    "CREATE TABLE [DeviceUser] (id INTEGER PRIMARY KEY AUTOINCREMENT, UserName NTEXT,EmployeeName NTEXT, NRIC NTEXT,UserLoginDomain NTEXT,UserPwd NTEXT,DeviceID NTEXT, OfflineBillNo NTEXT, Note NTEXT);"
				};
				foreach (var command in commands) {
					using (var c = connection.CreateCommand ()) {
						c.CommandText = command;
						var i = c.ExecuteNonQuery ();
					}
				}
			} else {
				// already exists
			}
			Console.WriteLine (output);
		    }


         public DataTable RetrieveData(String SQLSyntax)
         {
             SqliteConnection MyConnection = null;
             SqliteCommand MyCommand = null;
             SqliteDataAdapter MyAdapter = null;
             DataTable MyTable = new DataTable();
             MyConnection = new SqliteConnection("Data Source=" + DBPath);
             //MyConnection.ConnectionString = KoneksiStr;
             MyCommand = new SqliteCommand();
             MyCommand.CommandText = SQLSyntax;
             MyCommand.CommandType = CommandType.Text;
             MyCommand.Connection = MyConnection;
             MyAdapter = new SqliteDataAdapter();
             MyAdapter.SelectCommand = MyCommand;
             MyAdapter.Fill(MyTable);
             return MyTable;
         }

         public bool ExecuteNonQuery(string StrQuery)
         {
             lock (locker)
             {
                 SqliteConnection MyConnection = null;
                 SqliteCommand MyCommand = null;
                 MyConnection = new SqliteConnection("Data Source=" + DBPath);
                 try
                 {

                     MyCommand = new SqliteCommand();
                     MyCommand.CommandText = StrQuery;
                     MyCommand.CommandType = CommandType.Text;
                     MyCommand.Connection = MyConnection;
                     MyConnection.Open();
                     if (MyCommand.ExecuteNonQuery() > 0)
                     {
                         MyCommand.Dispose();
                         MyConnection.Dispose();
                         return true;
                     }
                     else return false;
                 }
                 catch (Exception) // catches without assigning to a variable
                 {
                     return false;
                 }
             }
         }

         public bool ExecuteNonQuery(string StrQuery, string[] ParamName, object[] ParamValue)
         {
             lock (locker)
             {
                 SqliteConnection MyConnection = null;
                 SqliteCommand MyCommand = null;
                 MyConnection = new SqliteConnection("Data Source=" + DBPath);
                 try
                 {

                     MyCommand = new SqliteCommand();
                     MyCommand.CommandText = StrQuery;
                     MyCommand.CommandType = CommandType.Text;
                     if (ParamName != null && ParamValue != null)
                     {
                         for (int i = 0; i <= ParamName.GetUpperBound(0); i++)
                         {
                             DbParameter param1 = MyCommand.CreateParameter();
                             param1.ParameterName = ParamName[i];
                             param1.Value = ParamValue[i];
                             MyCommand.Parameters.Add(param1);
                         }
                     }
                     MyCommand.Connection = MyConnection;
                     MyConnection.Open();
                     if (MyCommand.ExecuteNonQuery() > 0)
                     {
                         MyCommand.Dispose();
                         MyConnection.Dispose();
                         return true;
                     }
                     else return false;
                 }
                 catch (Exception) // catches without assigning to a variable
                 {
                     return false;
                 }
             }
         }

         public object ExecuteScalar(string StrQuery)
         {
             lock (locker)
             {
                 SqliteConnection MyConnection = null;
                 SqliteCommand MyCommand = null;
                 MyConnection = new SqliteConnection("Data Source=" + DBPath);
                 try
                 {

                     MyCommand = new SqliteCommand();
                     MyCommand.CommandText = StrQuery;
                     MyCommand.CommandType = CommandType.Text;
                     MyCommand.Connection = MyConnection;
                     MyConnection.Open();
                     object Res = MyCommand.ExecuteScalar();
                     if (Res != null)
                     {
                         MyCommand.Dispose();
                         MyConnection.Dispose();
                         return Res;
                     }
                     else return null;
                 }
                 catch (Exception) // catches without assigning to a variable
                 {
                     return null;
                 }
             }
         }

         public object ExecuteScalar(string StrQuery, string[] ParamName, object[] ParamValue)
         {
             lock (locker)
             {
                 SqliteConnection MyConnection = null;
                 SqliteCommand MyCommand = null;
                 MyConnection = new SqliteConnection("Data Source=" + DBPath);
                 try
                 {

                     MyCommand = new SqliteCommand();
                     MyCommand.CommandText = StrQuery;
                     MyCommand.CommandType = CommandType.Text;
                     if (ParamName != null && ParamValue != null)
                     {
                         for (int i = 0; i <= ParamName.GetUpperBound(0); i++)
                         {
                             DbParameter param1 = MyCommand.CreateParameter();
                             param1.ParameterName = ParamName[i];
                             param1.Value = ParamValue[i];
                             MyCommand.Parameters.Add(param1);
                         }
                     }
                     MyCommand.Connection = MyConnection;
                     MyConnection.Open();
                     object Res = MyCommand.ExecuteScalar();
                     if (Res != null)
                     {
                         MyCommand.Dispose();
                         MyConnection.Dispose();
                         return Res;
                     }
                     else return null;
                 }
                 catch (Exception) // catches without assigning to a variable
                 {
                     return null;
                 }
             }
         }
 
    }
}
