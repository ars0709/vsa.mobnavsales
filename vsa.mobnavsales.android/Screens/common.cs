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
           // DBSqliteFunction db = new DBSqliteFunction( DBSqliteFunction. .getExternalStorageDirectory().toString()+"/data/my_package_name/databases/";)
            if (!db.isDBExists)
            {
                //db.ExecuteNonQuery("CREATE TABLE IF NOT EXISTS [NavNetworkCredential] (id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, NavWebServiceName NTEXT,NavWebServer NTEXT,NavPort INT,NavCompany NTEXT, Note NTEXT, NavDefaultCredential INT, NavFormAuthentication INT, NavLoginDomain NTEXT, NavUserName NTEXT , NavUserPwd NTEXT, UseSetupCompany INT);");
                //db.ExecuteNonQuery("CREATE TABLE IF NOT EXISTS [OfflineModeCompany] (id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, OfflineCompanyName NTEXT, NOTE NTEXT);");

                db.ExecuteNonQuery("CREATE TABLE IF NOT EXISTS [NavNetworkCredential] (id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, NavWebServiceName NTEXT,NavWebServer NTEXT,NavPort INT,NavCompany NTEXT, Note NTEXT, NavDefaultCredential INT, NavFormAuthentication INT, NavLoginDomain NTEXT, NavUserName NTEXT , NavUserPwd NTEXT, UseSetupCompany INT);");
                db.ExecuteNonQuery("CREATE TABLE IF NOT EXISTS [OfflineModeCompany] (id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, OfflineCompanyName NTEXT, NOTE NTEXT);");
                db.ExecuteNonQuery("CREATE TABLE IF NOT EXISTS [BalanceRemainder] (id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, bid INTEGER,cashid INTEGER , ReceiveDate Datetime, BalanceReceived DECIMAL, PaymentDate Datetime);");
                db.ExecuteNonQuery("CREATE TABLE IF NOT EXISTS [CashReceived] (CashId INTEGER, SalesId INTEGER , CustId INTEGER , SalesDate DATETIME,InvNo VARCHAR,OnlineCashSales INTEGER, OfflineCashSales INTEGER, TTLAmt FLOAT, TTLGST FLOAT, TTLAmtReceived Float, Balance FLOAT )");
                db.ExecuteNonQuery("CREATE TABLE IF NOT EXISTS [Category] (CateId INTEGER, CompanyName VARCHAR , CustId INTEGER , Item_Category_Code VARCHAR, Category_Description VARCHAR, InsertDate DATETIME )");
                db.ExecuteNonQuery("CREATE TABLE IF NOT EXISTS [Customer] (Id integer primary key autoincrement not null ,No varchar , CompanyName varchar ,Name varchar ,Address varchar ,Address2 varchar ,Post_Code varchar ,City varchar ,Country varchar, Country_Region_Code varchar ,Phone_No varchar ,Fax_No varchar , E_Mail varchar ,Contact varchar , CreditLimit float , Currency_Code varchar , Salesperson_Code varchar , Vat_Bus_Posting_Group varchar , InsertDateTime datetime)");
                db.ExecuteNonQuery("CREATE TABLE IF NOT EXISTS [DbSyncDate] ( Id integer primary key autoincrement not null , SyncDateTime datetime , Company varchar , TblName varchar )");
                db.ExecuteNonQuery("CREATE TABLE IF NOT EXISTS [DeviceUser]( Id integer primary key autoincrement not null ,Username varchar , EmployeeName varchar , NRIC varchar , UserLoginDomain varchar , UserPwd varchar , DeviceId varchar , OfflineBillNo varchar ,Note varchar )");
                db.ExecuteNonQuery("CREATE TABLE IF NOT EXISTS [InvStartingNo] (Id integer primary key autoincrement not null ,Prefix  varchar ,PrefixYear varchar , Note varchar , CheckFirstTime integer , StartingNo integer , LastNo integer )");
                db.ExecuteNonQuery("CREATE TABLE IF NOT EXISTS [Item] ( Id integer primary key autoincrement not null , No varchar ,CompanyName varchar , PictureFilename varchar , Description varchar , Description2 varchar , Unit_Price float)");
                db.ExecuteNonQuery("CREATE TABLE IF NOT EXISTS [ItemSalesPrice] ( SpId integer primary key autoincrement not null ,CompanyName varchar ,ItemNo varchar ,SalesCode varchar , CurrencyCode varchar , StartingDate datetime , UnitPrice float , PriceIncludesVAT integer , AllowInvoiceDisc integer , VATBusPostingGr varchar , SalesType integer , MinimumQuantity float , EndingDate datetime , UnitOfMeasure varchar , VariantCode varchar , AllowLineDisc integer , PublishedPrice float , Cost float , CostPlus float , DiscountAmount float , InsertDate datetime )");
                db.ExecuteNonQuery("CREATE TABLE IF NOT EXISTS [ItemUoM] ( UId integer primary key autoincrement not null ,CompanyName varchar , ItemNo varchar , UoM varchar , ConversionRate  float , InsertDate datetime )");
                db.ExecuteNonQuery("CREATE TABLE IF NOT EXISTS [Location] ( Id integer primary key autoincrement not null , UserId varchar , CompanyName varchar , SalesPersonnel_Purch_Code varchar , Default_Location varchar , Note varchar)");
                db.ExecuteNonQuery("CREATE TABLE IF NOT EXISTS [PaymentCode] ( PId integer primary key autoincrement not null , Company varchar , Code varchar , Code_Description varchar , Note varchar )");
                db.ExecuteNonQuery("CREATE TABLE IF NOT EXISTS [ProductCode]( PId integer primary key autoincrement not null , CompanyName varchar , Item_Category_Code varchar , Product_Code varchar , Product_Description varchar ,InsertDate  datetime )");
                db.ExecuteNonQuery("CREATE TABLE IF NOT EXISTS [SalesOrderHeader] (SId integer primary key autoincrement not null , CustId integer ,CompanyName varchar , Document_Type integer ,No varchar , DataEntryComplete integer , Off_DocType integer , Off_Cancel integer , InvNbrBeforeSync varchar , Is_Offline_Trans integer , Is_SyncToNAV integer , Sell_to_Customer_No varchar , Order_Date datetime , Is_PaymentReceived integer , Employee varchar , Payment_Code varchar ,Cheque_Nbr varchar , Salesperson_Code varchar , Location_Code varchar , Currency_Code varchar ,Note varchar )");
                db.ExecuteNonQuery("CREATE TABLE IF NOT EXISTS [TransactionLine] ( TId integer primary key autoincrement not null , SId integer , DocKey varchar , DocType integer , DocNo varchar , LineNo integer ,No varchar , Description varchar , Quantity float , Unit_of_Measure_Code varchar , UnitPrice float , LineDisPerc float , LineDiscAmt float , Amount float , Amount_Including_VAT float , Inv_Discount_Amount float , LineAmount float , LineGST float , Note varchar )");
                db.ExecuteNonQuery("CREATE TABLE IF NOT EXISTS [VAT] ( VatId integer primary key autoincrement not null , CompanyName varchar , VATBusPostingGroup varchar , VATProdPostingGroup varchar , VATIdentifier varchar , VATPercent float , VATCalculationType integer , UnrealizedVatType integer , AdjustForPaymentDiscount integer , SalesVatAccount varchar , SalesVatUnrealAccount varchar , PurchaseVatAccount varchar , PurchVatUnrealAccount varchar , ReverseChrgVatAcc varchar , ReverseChrgVatUnrealAcc varchar , EUService integer)");

            }
            else
            {

                /*
                db.ExecuteNonQuery("CREATE TABLE IF NOT EXISTS [NavNetworkCredential] (id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, NavWebServiceName NTEXT,NavWebServer NTEXT,NavPort INT,NavCompany NTEXT, Note NTEXT, NavDefaultCredential INT, NavFormAuthentication INT, NavLoginDomain NTEXT, NavUserName NTEXT , NavUserPwd NTEXT, UseSetupCompany INT);");
                db.ExecuteNonQuery("CREATE TABLE IF NOT EXISTS [OfflineModeCompany] (id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, OfflineCompanyName NTEXT, NOTE NTEXT);");
                db.ExecuteNonQuery("CREATE TABLE IF NOT EXISTS [BalanceRemainder] (id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, bid INTEGER,cashid INTEGER , ReceiveDate Datetime, BalanceReceived DECIMAL, PaymentDate Datetime);");
                db.ExecuteNonQuery("CREATE TABLE IF NOT EXISTS [CashReceived] (CashId INTEGER, SalesId INTEGER , CustId INTEGER , SalesDate DATETIME,InvNo VARCHAR,OnlineCashSales INTEGER, OfflineCashSales INTEGER, TTLAmt FLOAT, TTLGST FLOAT, TTLAmtReceived Float, Balance FLOAT )");
                db.ExecuteNonQuery("CREATE TABLE IF NOT EXISTS [Category] (CateId INTEGER, CompanyName VARCHAR , CustId INTEGER , Item_Category_Code VARCHAR, Category_Description VARCHAR, InsertDate DATETIME )");
                db.ExecuteNonQuery("CREATE TABLE IF NOT EXISTS [Customer] (Id integer primary key autoincrement not null ,No varchar , CompanyName varchar ,Name varchar ,Address varchar ,Address2 varchar ,Post_Code varchar ,City varchar ,Country varchar, Country_Region_Code varchar ,Phone_No varchar ,Fax_No varchar , E_Mail varchar ,Contact varchar , CreditLimit float , Currency_Code varchar , Salesperson_Code varchar , Vat_Bus_Posting_Group varchar , InsertDateTime datetime)");
                db.ExecuteNonQuery("CREATE TABLE IF NOT EXISTS [DbSyncDate] ( Id integer primary key autoincrement not null , SyncDateTime datetime , Company varchar , TblName varchar )");
                db.ExecuteNonQuery("CREATE TABLE IF NOT EXISTS [DeviceUser]( Id integer primary key autoincrement not null ,Username varchar , EmployeeName varchar , NRIC varchar , UserLoginDomain varchar , UserPwd varchar , DeviceId varchar , OfflineBillNo varchar ,Note varchar )");
                db.ExecuteNonQuery("CREATE TABLE IF NOT EXISTS [InvStartingNo] (Id integer primary key autoincrement not null ,Prefix  varchar ,PrefixYear varchar , Note varchar , CheckFirstTime integer , StartingNo integer , LastNo integer )");
                db.ExecuteNonQuery("CREATE TABLE IF NOT EXISTS [Item] ( Id integer primary key autoincrement not null , No varchar ,CompanyName varchar , PictureFilename varchar , Description varchar , Description2 varchar , Unit_Price float)");
                db.ExecuteNonQuery("CREATE TABLE IF NOT EXISTS [ItemSalesPrice] ( SpId integer primary key autoincrement not null ,CompanyName varchar ,ItemNo varchar ,SalesCode varchar , CurrencyCode varchar , StartingDate datetime , UnitPrice float , PriceIncludesVAT integer , AllowInvoiceDisc integer , VATBusPostingGr varchar , SalesType integer , MinimumQuantity float , EndingDate datetime , UnitOfMeasure varchar , VariantCode varchar , AllowLineDisc integer , PublishedPrice float , Cost float , CostPlus float , DiscountAmount float , InsertDate datetime )");
                db.ExecuteNonQuery("CREATE TABLE IF NOT EXISTS [ItemUoM] ( UId integer primary key autoincrement not null ,CompanyName varchar , ItemNo varchar , UoM varchar , ConversionRate  float , InsertDate datetime )");
                db.ExecuteNonQuery("CREATE TABLE IF NOT EXISTS [Location] ( Id integer primary key autoincrement not null , UserId varchar , CompanyName varchar , SalesPersonnel_Purch_Code varchar , Default_Location varchar , Note varchar)");
                db.ExecuteNonQuery("CREATE TABLE IF NOT EXISTS [PaymentCode] ( PId integer primary key autoincrement not null , Company varchar , Code varchar , Code_Description varchar , Note varchar )");
                db.ExecuteNonQuery("CREATE TABLE IF NOT EXISTS [ProductCode]( PId integer primary key autoincrement not null , CompanyName varchar , Item_Category_Code varchar , Product_Code varchar , Product_Description varchar ,InsertDate  datetime )");
                db.ExecuteNonQuery("CREATE TABLE IF NOT EXISTS [SalesOrderHeader] (SId integer primary key autoincrement not null , CustId integer ,CompanyName varchar , Document_Type integer ,No varchar , DataEntryComplete integer , Off_DocType integer , Off_Cancel integer , InvNbrBeforeSync varchar , Is_Offline_Trans integer , Is_SyncToNAV integer , Sell_to_Customer_No varchar , Order_Date datetime , Is_PaymentReceived integer , Employee varchar , Payment_Code varchar ,Cheque_Nbr varchar , Salesperson_Code varchar , Location_Code varchar , Currency_Code varchar ,Note varchar )");
                db.ExecuteNonQuery("CREATE TABLE IF NOT EXISTS [TransactionLine] ( TId integer primary key autoincrement not null , SId integer , DocKey varchar , DocType integer , DocNo varchar , LineNo integer ,No varchar , Description varchar , Quantity float , Unit_of_Measure_Code varchar , UnitPrice float , LineDisPerc float , LineDiscAmt float , Amount float , Amount_Including_VAT float , Inv_Discount_Amount float , LineAmount float , LineGST float , Note varchar )");
                db.ExecuteNonQuery("CREATE TABLE IF NOT EXISTS [VAT] ( VatId integer primary key autoincrement not null , CompanyName varchar , VATBusPostingGroup varchar , VATProdPostingGroup varchar , VATIdentifier varchar , VATPercent float , VATCalculationType integer , UnrealizedVatType integer , AdjustForPaymentDiscount integer , SalesVatAccount varchar , SalesVatUnrealAccount varchar , PurchaseVatAccount varchar , PurchVatUnrealAccount varchar , ReverseChrgVatAcc varchar , ReverseChrgVatUnrealAcc varchar , EUService integer)");
                */


            }


            return db;
        }

            
    }
}