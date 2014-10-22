//Written by Adiel, Copyright (c) 2013 Gravicode
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

namespace vsa.mobnavsales.Library
{
    public interface IFungsiDB
    {
        bool isDBExists{set;get;}

        object RetrieveOneData(string SQLSyntax);

        DataTable RetrieveData(string SQLSyntax, string[] ParamName, object[] ParamValue);

        DataTable RetrieveData(string SQLSyntax);

        bool InsertData(string TableName, string[] FieldName, object[] FieldValue);

        bool UpdateRecord(string TableName, string[] ColumnID, object[] IDValues, string[] Column, object[] ColumnValues);

        bool DeleteData(string Tabel, string KolPk, object PkValue);

        bool DeleteData(string TableName, string[] ColumnID, object[] IDValues);

        string getUniqueNumber(string AwalStr, string Tabel, string Kolom);

        bool ExecuteNonQuery(string StrQuery);

        bool ExecuteNonQuery(string StrQuery, string[] ParamName, object[] ParamValue);

        string GenerateNewKey(string TableName, string FieldName, string Prefix, int KeyLength);

        string FillWithZero(int NumberLength, string Number, int InsertType);

        object ExecuteScalar(string StrQuery);

        object ExecuteScalar(string StrQuery, string[] ParamName, object[] ParamValue);
    }
}