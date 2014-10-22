//Written by Adiel, Copyright (c) 2013 Gravicode

using System;
using System.Linq;
using System.Collections.Generic;
using Mono.Data.Sqlite;
using System.IO;
using System.Data;
using System.Data.Common;

namespace vsa.mobnavsales.Library
{

    public class DBSqliteFunction:IFungsiDB
    {
        static object locker = new object();

        public bool isDBExists { set; get; }

        private string DBPath;

        public DBSqliteFunction(string dbPath)
        {
            var output = "";
            DBPath = dbPath;
            // create the tables
            isDBExists = File.Exists(dbPath);
			//"CREATE TABLE [Ejekan] (_id INTEGER PRIMARY KEY ASC, Penghubung NTEXT, Hinaan NTEXT);"
				
            Console.WriteLine(output);
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

        public static string GetDBFilePath(string sqliteFilename)
        {
            if (string.IsNullOrEmpty(sqliteFilename))
            {
                sqliteFilename = "MobNavSales.db3";
            }


#if NETFX_CORE
				var path = Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, sqliteFilename);
#else

#if SILVERLIGHT
				// Windows Phone expects a local path, not absolute
				var path = sqliteFilename;
#else

#if __ANDROID__
				// Just use whatever directory SpecialFolder.Personal returns
				string libraryPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal); 
#else
            // we need to put in /Library/ on iOS5.1 to meet Apple's iCloud terms
            // (they don't want non-user-generated data in Documents)
            string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal); // Documents folder
            string libraryPath = Path.Combine(documentsPath, "..", "Library"); // Library folder
#endif
            var path = Path.Combine(libraryPath, sqliteFilename);
#endif

#endif
            return path;

        }
        
        public bool InsertData(string TableName, string[] FieldName, object[] FieldValue)
        {
            lock (locker)
            {
                string FieldNames = string.Empty;
                string FieldValues = string.Empty;
                string ParamNames = string.Empty;
                string[] ParamName = new string[FieldName.Length];
                int Counter = 0;
                foreach (string item in FieldName)
                {
                    ParamName[Counter] = "@" + item.Trim().Replace(" ", "_");
                    if (Counter > 0)
                    {
                        ParamNames += ",";
                        FieldNames += ",";
                    }
                    ParamNames += ParamName[Counter];
                    FieldNames += item.IndexOf("[") > 0 ? item : "[" + item + "]";
                    Counter++;
                }
                string StrQuery = string.Format(@"INSERT INTO {0} ({1}) VALUES ({2})", TableName, FieldNames, ParamNames);
                SqliteConnection MyConnection = new SqliteConnection("Data Source=" + DBPath);
                SqliteCommand MyCommand = null;
                try
                {
                    MyCommand = MyConnection.CreateCommand();
                    MyCommand.CommandText = StrQuery;
                    MyCommand.CommandType = CommandType.Text;
                    if (ParamName != null && FieldValue != null)
                    {
                        for (int i = 0; i <= ParamName.GetUpperBound(0); i++)
                        {
                            DbParameter param1 = MyCommand.CreateParameter();
                            param1.ParameterName = ParamName[i];
                            param1.Value = FieldValue[i];
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
                catch (Exception ex) // catches without assigning to a variable
                {
                    string err = ex.Message + "-" + ex.StackTrace;
                    Console.WriteLine(err);
                    return false;
                }
            }
        }
        
        public bool UpdateRecord(string TableName, string[] ColumnID, object[] IDValues, string[] Column, object[] ColumnValues)
        {
            lock (locker)
            {
                SqliteCommand cmd = null;
                SqliteConnection conn = null;
                conn = new SqliteConnection("Data Source=" + DBPath);
                List<string> ParNames = new List<string>();
                List<string> ParNameIDs = new List<string>();
                List<object> ParValues = new List<object>();

                string QueryStr = "";
                string ValueStr = "";


                if (ColumnID == null || IDValues == null || Column == null || ColumnValues == null)
                    return false;
                else
                {
                    QueryStr = "";
                    ValueStr = "";
                    for (int i = 0; i <= ColumnID.GetUpperBound(0); i++)
                    {
                        string ParName = "@" + ColumnID[i].Trim().Replace(" ", "_");
                        ParNames.Add(ParName);
                        string FieldName = ColumnID[i].IndexOf("[") > 0 ? ColumnID[i] : "[" + ColumnID[i] + "]";
                        ParValues.Add(IDValues[i]);
                        QueryStr = QueryStr + FieldName + " = " + ParName;
                        if (i < ColumnID.GetUpperBound(0))
                        {
                            QueryStr = QueryStr + " and ";
                        }

                    }

                    for (int i = 0; i <= Column.GetUpperBound(0); i++)
                    {
                        string ParName = "@" + Column[i].Trim().Replace(" ", "_");
                        ParNames.Add(ParName);

                        string FieldName = Column[i].IndexOf("[") > 0 ? Column[i] : "[" + Column[i] + "]";

                        ValueStr = ValueStr + FieldName + " = " + ParName;
                        if (i < Column.GetUpperBound(0))
                        {
                            ValueStr = ValueStr + ",";
                        }
                        ParValues.Add(ColumnValues[i]);
                    }
                    cmd = new SqliteCommand("UPDATE " + TableName + " SET " + ValueStr + " " +
                                     "WHERE " + QueryStr, conn);
                    if (ParNames.Count > 0 && ColumnValues != null)
                    {
                        for (int i = 0; i < ParNames.Count; i++)
                        {
                            DbParameter param1 = cmd.CreateParameter();
                            param1.ParameterName = ParNames[i];
                            param1.Value = ParValues[i];
                            cmd.Parameters.Add(param1);
                        }
                    }
                }
                conn.Open();
                try
                {
                    if (cmd.ExecuteNonQuery() > 0)
                    {
                        conn.Close();
                        return true;
                    }
                    else return false;
                }
                catch (Exception ex)
                {
                    Console.Write(ex.Message);
                    return false;
                }
            }
        }

        public bool DeleteData(string TableName, string[] ColumnID, object[] IDValues)
        {
            lock (locker)
            {
                SqliteCommand cmd = null;
                SqliteConnection conn = null;
                conn = new SqliteConnection("Data Source=" + DBPath);
                string FieldNames = string.Empty;
                string FieldValues = string.Empty;
                string ParamNames = string.Empty;
                string[] ParamName = new string[ColumnID.Length];
                int Counter = 0;
                foreach (string item in ColumnID)
                {
                    ParamName[Counter] = "@" + item.Trim().Replace(" ", "_");
                    if (Counter > 0)
                    {
                        ParamNames += ",";
                        FieldNames += ",";
                    }
                    ParamNames += ParamName[Counter];
                    FieldNames += item.IndexOf("[") > 0 ? item : "[" + item + "]";
                    Counter++;
                }
                string QueryStr = "";


                if (ColumnID == null || IDValues == null)
                    return false;
                else
                {
                    QueryStr = "";
                    for (int i = 0; i < ColumnID.Length; i++)
                    {

                        QueryStr = QueryStr + ColumnID[i] + " = " + ParamName[i];
                        if (i < ColumnID.GetUpperBound(0))
                        {
                            QueryStr = QueryStr + " and ";
                        }

                    }

                    cmd = new SqliteCommand("delete from " + TableName + " WHERE " + QueryStr, conn);
                    if (ParamName != null && IDValues != null)
                    {
                        for (int i = 0; i <= ParamName.GetUpperBound(0); i++)
                        {
                            DbParameter param1 = cmd.CreateParameter();
                            param1.ParameterName = ParamName[i];
                            param1.Value = IDValues[i];
                            cmd.Parameters.Add(param1);
                        }
                    }
                }
                conn.Open();
                try
                {
                    if (cmd.ExecuteNonQuery() > 0)
                    {
                        conn.Close();
                        return true;
                    }
                    else return false;
                }
                catch (Exception ex)
                {
                    Console.Write(ex.Message);
                    return false;
                }
            }
        }

        public bool DeleteData(string Tabel, string KolPk, object PkValue)
        {
            lock (locker)
            {
                SqliteConnection MyConnection = null;
                SqliteCommand MyCommand = null;
                MyConnection = new SqliteConnection("Data Source=" + DBPath);
                string StrQuery = "";
                string ParamName = "@" + KolPk.Trim().Replace(" ", "_");
                StrQuery = "Delete from " + Tabel + " where " + KolPk.Trim() + " = " + ParamName;
                try
                {
                    MyCommand = new SqliteCommand();
                    MyCommand.CommandText = StrQuery;
                    MyCommand.CommandType = CommandType.Text;
                    MyCommand.Connection = MyConnection;
                    if (ParamName != null && PkValue != null)
                    {
                        DbParameter param1 = MyCommand.CreateParameter();
                        param1.ParameterName = ParamName;
                        param1.Value = PkValue;
                        MyCommand.Parameters.Add(param1);
                    }
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

        public DataTable RetrieveData(string SqliteSyntax, string[] ParamName, object[] ParamValue)
        {
            lock (locker)
            {
                SqliteConnection MyConnection = null;
                SqliteCommand MyCommand = null;
                SqliteDataAdapter MyAdapter = null;
                DataTable MyTable = new DataTable();
                MyConnection = new SqliteConnection("Data Source=" + DBPath);

                MyCommand = new SqliteCommand();
                MyCommand.CommandText = SqliteSyntax;
                MyCommand.CommandType = CommandType.Text;
                MyCommand.Connection = MyConnection;
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
                MyAdapter = new SqliteDataAdapter();
                MyAdapter.SelectCommand = MyCommand;
                MyAdapter.Fill(MyTable);
                return MyTable;
            }
        }

        public object RetrieveOneData(string SqliteSyntax)
        {
            lock (locker)
            {
                object value = null;
                SqliteConnection MyConnection = null;
                SqliteCommand MyCommand = null;
                MyConnection = new SqliteConnection("Data Source=" + DBPath);
                MyCommand = new SqliteCommand();
                MyCommand.CommandText = SqliteSyntax;
                MyCommand.CommandType = CommandType.Text;
                MyCommand.Connection = MyConnection;
                MyConnection.Open();
                value = MyCommand.ExecuteScalar() != null ? MyCommand.ExecuteScalar() : null;
                MyConnection.Close();
                return value;
            }
        }

        public string getUniqueNumber(string AwalStr, string Tabel, string Kolom)
        {
            lock (locker)
            {
                int MaxNo = 0;
                int Pjg = 0;
                string RsltStr;
                SqliteConnection MyConnection = null;
                SqliteCommand MyCommand = null;
                SqliteDataAdapter MyAdapter = null;
                DataTable MyTable = null;
                MyTable = new DataTable();
                MyConnection = new SqliteConnection("Data Source=" + DBPath);
                MyCommand = new SqliteCommand();
                MyCommand.CommandText = "SELECT Count (" + Kolom + ") from dbo." + Tabel;
                MyCommand.CommandType = CommandType.Text;
                MyCommand.Connection = MyConnection;
                MyAdapter = new SqliteDataAdapter();
                MyAdapter.SelectCommand = MyCommand;
                MyAdapter.Fill(MyTable);
                if (MyTable.Rows[0][0].ToString() == "0" || MyTable.Rows.Count <= 0) MaxNo = 0;
                else
                    MaxNo = Convert.ToInt32(MyTable.Rows[0][0].ToString());
                MaxNo = MaxNo + 1;
                Pjg = MaxNo.ToString().Trim().Length;
                RsltStr = AwalStr;
                if (Pjg < 3)
                {
                    for (int i = 1; i < 4 - Pjg; i++) RsltStr = RsltStr + "0";
                }
                RsltStr = RsltStr + MaxNo.ToString().Trim();
                return RsltStr;
            }
        }
       
        public string GenerateNewKey(string TableName, string FieldName, string Prefix, int KeyLength)
        {
            lock (locker)
            {
                try
                {
                    string SqliteSyntax = "Select Max(Substring(" + FieldName + "," + Convert.ToString(Prefix.Length + 1) + "," + Convert.ToString(KeyLength - Prefix.Length) + ")) From " + TableName;
                    SqliteConnection MyConnection = null;
                    SqliteCommand MyCommand = null;
                    SqliteDataAdapter MyAdapter = null;
                    DataTable MyTable = new DataTable();
                    MyConnection = new SqliteConnection("Data Source=" + DBPath);
                    MyCommand = new SqliteCommand();
                    MyCommand.CommandText = SqliteSyntax;
                    MyCommand.CommandType = CommandType.Text;
                    MyCommand.Connection = MyConnection;
                    MyAdapter = new SqliteDataAdapter();
                    MyAdapter.SelectCommand = MyCommand;
                    MyAdapter.Fill(MyTable);
                    if (MyTable.Rows.Count > 0)
                    {
                        if (MyTable.Rows[0][0] == DBNull.Value)
                            return Prefix + FillWithZero(KeyLength - Prefix.Length, "0", 1);
                        int Cnt = Convert.ToInt32(MyTable.Rows[0][0]) + 1;
                        return Prefix + FillWithZero(KeyLength - Prefix.Length, Cnt.ToString(), 1);
                    }
                    return "";
                }
                catch 
                {
                    return "";
                }
            }
        }

        public string FillWithZero(int NumberLength, string Number, int InsertType)
        {
            string Tmp = "";
            for (int i = 0; i < NumberLength - Number.Trim().Length; i++)
            {
                Tmp += "0";
            }
            if (InsertType == 1)
            {
                return Tmp + Number.Trim();
            }
            else
            {
                return Number.Trim() + Tmp;
            }
        }

    }
}