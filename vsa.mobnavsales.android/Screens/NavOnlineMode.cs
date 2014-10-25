using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;




namespace vsa.mobnavsales.android.Screens
{
     class NavOnlineMode
    {

        //--- Data pass from MainPage when user login

        public static string UserName { get; set; }
        public static string UserPwd { get; set; }
        public static string Company { get; set; }
        public static string LoginDomain { get; set; }
        public static string WebServer { get; set; }
        public static string WebServiceName { get; set; }
        public static int PortNo { get; set; }
        public static int UseSetupCompany { get; set; }

        public enum EnumWSType { System, Page, CodeUnit, OData, XML }

        public static string _webserviceurlsystem = "http://{0}:{1}/{2}/WS/{3}";
        public static string _webserviceurlpage = "http://{0}:{1}/{2}/WS/{3}/Page/{4}";
        public static string _webserviceurlcodeunit = "http://{0}:{1}/{2}/WS/{3}/Codeunit/{4}";

        public static Uri _webserviceurl = null;

        public static bool LogInasSuperUser;

        //public static Uri _webserviceuricodeunit = null;



        #region WebService
        public class WebServiceBinding
        {
            public static System.ServiceModel.BasicHttpBinding _basicHttpBinding(EnumWSType _enumWSType, string _strCompany, string _strServiceNameSpace)
            {

                //*************************************************************************************
                //1)  this to return  URL ( Http:// ) to get the resources in NAV
                //2) _locbasicHttpBinding;
                //*************************************************************************************

                //eg : http://NavisionServerName:NavisionPort/NavisionServiceName/WS/NavisionCompanyName/Page/WebServiceNameSpace
                //eg : http://mscrm:7047/DynamicsNAV_ASA/WS/Arista Singapore Pte. Ltd/Page/Customer_List


                switch (_enumWSType)
                {
                    //--- create URI:

                    case EnumWSType.System:
                        _webserviceurl = new Uri(string.Format(_webserviceurlsystem, WebServer, PortNo, WebServiceName, _strServiceNameSpace));
                        break;
                    case EnumWSType.Page:
                        _webserviceurl = new Uri(string.Format(_webserviceurlpage, WebServer, PortNo, WebServiceName, Uri.EscapeDataString(_strCompany), _strServiceNameSpace));
                        break;
                    case EnumWSType.CodeUnit:
                        _webserviceurl = new Uri(string.Format(_webserviceurlcodeunit, WebServer, PortNo, WebServiceName, Uri.EscapeDataString(_strCompany), _strServiceNameSpace));
                        break;

                    case EnumWSType.OData: break;
                    case EnumWSType.XML: break;

                }

                System.ServiceModel.BasicHttpBinding _locbasicHttpBinding = new System.ServiceModel.BasicHttpBinding();

                _locbasicHttpBinding.Security.Mode = System.ServiceModel.BasicHttpSecurityMode.TransportCredentialOnly;
                _locbasicHttpBinding.Security.Transport.ClientCredentialType = System.ServiceModel.HttpClientCredentialType.Windows;
                _locbasicHttpBinding.MaxBufferSize = Int32.MaxValue;
                _locbasicHttpBinding.MaxReceivedMessageSize = Int32.MaxValue;

                //--- set timeout here??
                // _locbasicHttpBinding.OpenTimeout = System.TimeSpan.Parse("3");

                return _locbasicHttpBinding;
            }


            public static System.Net.NetworkCredential _networkCredential()
            {
                //return new System.Net.NetworkCredential("tun", "msc1234", "msc-consulting");
                // return new System.Net.NetworkCredential("edward.chow", "4545eeaa", "msc-consulting");

                return new System.Net.NetworkCredential(UserName, UserPwd, LoginDomain);

            }

        }
        #endregion


        public class CodeUnitService
        {
            public static  wsPDACuGenWebServices.PDA_General_Web_Service_PortClient GetServcie()
            {
                //wsPDACuGenWebServices.PDA_General_Web_Service_PortClient _ws = new wsPDACuGenWebServices.PDA_General_Web_Service_PortClient(WebServiceBinding._basicHttpBinding(EnumWSType.CodeUnit, "CRONUS International Ltd.", "PDA_General_Web_Service"), new System.ServiceModel.EndpointAddress(_webserviceurl));

                //--using PDA_General_Web_Service                                                                                                                                                             //company
                wsPDACuGenWebServices.PDA_General_Web_Service_PortClient _ws = new wsPDACuGenWebServices.PDA_General_Web_Service_PortClient(WebServiceBinding._basicHttpBinding(EnumWSType.CodeUnit, Company, "PDA_General_Web_Service"), new System.ServiceModel.EndpointAddress(_webserviceurl));

                _ws.ClientCredentials.Windows.AllowedImpersonationLevel = System.Security.Principal.TokenImpersonationLevel.Delegation;

                _ws.ClientCredentials.Windows.ClientCredential = WebServiceBinding._networkCredential();

                return _ws;

            }
        }


        #region Companies
        public class Companies
        {
            public class Card
            {
                //Do something for Card Type //////
            }


            public class Listing
            {
                public static wsSystemService.SystemService_PortClient GetService()
                {
                    //** using NAV SystemService **

                    //--using NAV's systemService to return:
                    //-- Url ( Http:// ) : 
                    // at start:  webserviceurl is null 
                    //-- Use the WebServiceBinding to return :_webserviceurl <--base on what param u pass in

                    wsSystemService.SystemService_PortClient _ws = new wsSystemService.SystemService_PortClient(WebServiceBinding._basicHttpBinding(EnumWSType.System, string.Empty, "SystemService"), new System.ServiceModel.EndpointAddress(_webserviceurl));

                    //-- using token as security
                    _ws.ClientCredentials.Windows.AllowedImpersonationLevel = System.Security.Principal.TokenImpersonationLevel.Delegation;

                    //-- enter the username,pwd and DomainName
                    _ws.ClientCredentials.Windows.ClientCredential = WebServiceBinding._networkCredential();

                    //throw new Exception(_ws.Endpoint.Address.Uri.ToString());
                    return _ws;

                }



                public static async Task<bool> LogInSuccess()
                {

                    bool _blnIsLogIn = false;
                    string _strErrorMsg = string.Empty;

                    wsSystemService.SystemService_PortClient _ws = null;
                    wsSystemService.Companies_Result _companies_Result = null;

                    try
                    {
                        //-1- get a portclient back.
                        //--- a get a specific URL (Http:// webservice )

                        _ws = GetService();

                        //-2- get Companies setUp in NAV thru portclient
                        _companies_Result = await _ws.CompaniesAsync();

                        _blnIsLogIn = true;

                        if (_ws.State == System.ServiceModel.CommunicationState.Opened)

                            await _ws.CloseAsync();

                    }
                    catch (System.Exception _ex)
                    {
                        _strErrorMsg = _ex.InnerException.Message;
                        throw new Exception("LogInSuccess() " + _strErrorMsg);

                    }
                    finally
                    {
                        //---- this is the problem if you set it to null.  ErrMsg : Object set to a null reference
                        // _ws = null;
                    }

                    //MessageDialog Msg1 = new MessageDialog("Login :  " + _strErrorMsg + _ws.Endpoint.Address.Uri.ToString(), "Login URL");
                    //await Msg1.ShowAsync();

                    return _blnIsLogIn;
                }



                public static async Task<bool> LogInasSuperUser(string _strDomain, string _strUserName)
                {
                    bool _blnIsLogInasSuperUser = false;

                    if (string.IsNullOrWhiteSpace(_strDomain) && string.IsNullOrWhiteSpace(_strUserName)) return _blnIsLogInasSuperUser;

                    wsPDACuGenWebServices.PDA_General_Web_Service_PortClient _ws = Class1.CodeUnitService.GetServcie();

                    Task<wsPDACuGenWebServices.G_wsfnGetPDARole_Result> _getPDARole_Result = null;

                    if (!string.IsNullOrWhiteSpace(_strDomain))
                        _getPDARole_Result = _ws.G_wsfnGetPDARoleAsync(string.Format("{0}\\{1}", _strDomain, _strUserName));
                    else
                        _getPDARole_Result = _ws.G_wsfnGetPDARoleAsync(string.Format("{0}", _strUserName));

                    await _getPDARole_Result;

                    if (_getPDARole_Result.IsCompleted)

                        _blnIsLogInasSuperUser = _getPDARole_Result.Result.return_value;

                    return _blnIsLogInasSuperUser;

                }


                public static async Task<string[]> GetAsyncRecords()
                {
                    wsSystemService.SystemService_PortClient _ws = GetService();
                    wsSystemService.Companies_Result _companies_Result = await _ws.CompaniesAsync();
                    string[] _strCompanies = _companies_Result.return_value;

                    if (_ws.State == System.ServiceModel.CommunicationState.Opened)
                        await _ws.CloseAsync();
                    return _strCompanies;
                }
            }
        }

        #endregion

        #region Customer
        public class Customer
        {
            public class Card
            {
                //Do something for Card Type //////
            }

            public class Listing
            {
                public static wsPDACustomerList.PDA_Customer_List_PortClient GetService()
                {
                    //wsPDACustomerList.PDA_Customer_List_PortClient _ws = new wsPDACustomerList.PDA_Customer_List_PortClient(WebServiceBinding._basicHttpBinding(EnumWSType.Page, "CRONUS International Ltd.", "PDA_Customer_List"), new System.ServiceModel.EndpointAddress(_webserviceurl));

                    wsPDACustomerList.PDA_Customer_List_PortClient _ws = new wsPDACustomerList.PDA_Customer_List_PortClient(WebServiceBinding._basicHttpBinding(EnumWSType.Page, Company, "PDA_Customer_List"), new System.ServiceModel.EndpointAddress(_webserviceurl));

                    _ws.ClientCredentials.Windows.AllowedImpersonationLevel = System.Security.Principal.TokenImpersonationLevel.Delegation;

                    _ws.ClientCredentials.Windows.ClientCredential = WebServiceBinding._networkCredential();

                    return _ws;

                }


                public static async Task<wsPDACustomerList.PDA_Customer_List> GetAsyncRecords(string _No)
                {
                    wsPDACustomerList.PDA_Customer_List_PortClient _ws = GetService();
                    wsPDACustomerList.PDA_Customer_List _List = (await _ws.ReadAsync(_No)).PDA_Customer_List;
                    if (_ws.State == System.ServiceModel.CommunicationState.Opened)
                        await _ws.CloseAsync();
                    return _List;
                }


                public static async Task<wsPDACustomerList.PDA_Customer_List[]> GetAsyncRecords(wsPDACustomerList.PDA_Customer_List_Filter[] _filters)
                {
                    wsPDACustomerList.PDA_Customer_List_PortClient _ws = GetService();
                    wsPDACustomerList.PDA_Customer_List[] _List;
                    List<wsPDACustomerList.PDA_Customer_List_Filter> _filterArray = new List<wsPDACustomerList.PDA_Customer_List_Filter>();
                    _filterArray.AddRange(_filters);
                    _List = _ws.ReadMultipleAsync(_filterArray.ToArray(), null, 0).Result.ReadMultiple_Result1;
                    if (_ws.State == System.ServiceModel.CommunicationState.Opened)
                        await _ws.CloseAsync();

                    return _List;
                }

                public static async Task<wsPDACustomerList.PDA_Customer_List[]> GetAsyncRecords(wsPDACustomerList.PDA_Customer_List_Filter[] _filters, string _strbookmarkkey)
                {
                    wsPDACustomerList.PDA_Customer_List_PortClient _ws = GetService();
                    wsPDACustomerList.PDA_Customer_List[] _List;
                    List<wsPDACustomerList.PDA_Customer_List_Filter> _filterArray = new List<wsPDACustomerList.PDA_Customer_List_Filter>();
                    _filterArray.AddRange(_filters);
                    _List = _ws.ReadMultipleAsync(_filterArray.ToArray(), _strbookmarkkey, 0).Result.ReadMultiple_Result1;
                    if (_ws.State == System.ServiceModel.CommunicationState.Opened)
                        await _ws.CloseAsync();
                    return _List;
                }


                public static async Task<wsPDACustomerList.PDA_Customer_List[]> GetAsyncRecords(wsPDACustomerList.PDA_Customer_List_Filter[] _filters, string _strbookmarkkey, int _intsetsize)
                {
                    //-- get the PortClient and Bind it with HttpBinding
                    wsPDACustomerList.PDA_Customer_List_PortClient _ws = GetService();

                    wsPDACustomerList.PDA_Customer_List[] _List;

                    List<wsPDACustomerList.PDA_Customer_List_Filter> _filterArray = new List<wsPDACustomerList.PDA_Customer_List_Filter>();

                    _filterArray.AddRange(_filters);

                    _List = _ws.ReadMultipleAsync(_filterArray.ToArray(), _strbookmarkkey, _intsetsize).Result.ReadMultiple_Result1;

                    if (_ws.State == System.ServiceModel.CommunicationState.Opened)

                        await _ws.CloseAsync();

                    return _List;
                }
            }
        }

        #endregion

        #region Uom
        public class ItemUoM
        {
            public class Card
            {
                //Do something for Card Type
            }

            public class Listing
            {
                public static wsPDAItemUoMList.PDA_Item_Unit_Of_Measure_List_PortClient GetService()
                {
                    //--call this ws
                    //wsPDAItemUoMList.PDA_Item_Unit_Of_Measure_List_PortClient _ws = new wsPDAItemUoMList.PDA_Item_Unit_Of_Measure_List_PortClient(WebServiceBinding._basicHttpBinding(EnumWSType.Page, "CRONUS International Ltd.", "PDA_Item_Unit_Of_Measure_List"), new System.ServiceModel.EndpointAddress(_webserviceurl));

                    wsPDAItemUoMList.PDA_Item_Unit_Of_Measure_List_PortClient _ws = new wsPDAItemUoMList.PDA_Item_Unit_Of_Measure_List_PortClient(WebServiceBinding._basicHttpBinding(EnumWSType.Page, Company, "PDA_Item_Unit_Of_Measure_List"), new System.ServiceModel.EndpointAddress(_webserviceurl));

                    _ws.ClientCredentials.Windows.AllowedImpersonationLevel = System.Security.Principal.TokenImpersonationLevel.Delegation;

                    _ws.ClientCredentials.Windows.ClientCredential = WebServiceBinding._networkCredential();

                    return _ws;

                }


                public static async Task<wsPDAItemUoMList.PDA_Item_Unit_Of_Measure_List> GetAsyncRecords(string _ItemNo, string _Code)
                {

                    wsPDAItemUoMList.PDA_Item_Unit_Of_Measure_List_PortClient _ws = GetService();

                    wsPDAItemUoMList.PDA_Item_Unit_Of_Measure_List _List = (await _ws.ReadAsync(_ItemNo, _Code)).PDA_Item_Unit_Of_Measure_List;

                    if (_ws.State == System.ServiceModel.CommunicationState.Opened)
                        await _ws.CloseAsync();

                    return _List;
                }



                public static async Task<wsPDAItemUoMList.PDA_Item_Unit_Of_Measure_List[]> GetAsyncRecords(wsPDAItemUoMList.PDA_Item_Unit_Of_Measure_List_Filter[] _filters)
                {

                    wsPDAItemUoMList.PDA_Item_Unit_Of_Measure_List_PortClient _ws = GetService();
                    wsPDAItemUoMList.PDA_Item_Unit_Of_Measure_List[] _List;

                    List<wsPDAItemUoMList.PDA_Item_Unit_Of_Measure_List_Filter> _filterArray = new List<wsPDAItemUoMList.PDA_Item_Unit_Of_Measure_List_Filter>();
                    _filterArray.AddRange(_filters);

                    _List = _ws.ReadMultipleAsync(_filterArray.ToArray(), null, 0).Result.ReadMultiple_Result1;

                    if (_ws.State == System.ServiceModel.CommunicationState.Opened)
                        await _ws.CloseAsync();

                    return _List;
                }


                public static async Task<wsPDAItemUoMList.PDA_Item_Unit_Of_Measure_List[]> GetAsyncRecords(wsPDAItemUoMList.PDA_Item_Unit_Of_Measure_List_Filter[] _filters, string _strbookmarkkey)
                {
                    wsPDAItemUoMList.PDA_Item_Unit_Of_Measure_List_PortClient _ws = GetService();

                    wsPDAItemUoMList.PDA_Item_Unit_Of_Measure_List[] _List;

                    List<wsPDAItemUoMList.PDA_Item_Unit_Of_Measure_List_Filter> _filterArray = new List<wsPDAItemUoMList.PDA_Item_Unit_Of_Measure_List_Filter>();

                    _filterArray.AddRange(_filters);

                    _List = _ws.ReadMultipleAsync(_filterArray.ToArray(), _strbookmarkkey, 0).Result.ReadMultiple_Result1;

                    if (_ws.State == System.ServiceModel.CommunicationState.Opened)
                        await _ws.CloseAsync();
                    return _List;
                }

                public static async Task<wsPDAItemUoMList.PDA_Item_Unit_Of_Measure_List[]> GetAsyncRecords(wsPDAItemUoMList.PDA_Item_Unit_Of_Measure_List_Filter[] _filters, string _strbookmarkkey, int _intsetsize)
                {
                    wsPDAItemUoMList.PDA_Item_Unit_Of_Measure_List_PortClient _ws = GetService();

                    wsPDAItemUoMList.PDA_Item_Unit_Of_Measure_List[] _List;

                    List<wsPDAItemUoMList.PDA_Item_Unit_Of_Measure_List_Filter> _filterArray = new List<wsPDAItemUoMList.PDA_Item_Unit_Of_Measure_List_Filter>();
                    _filterArray.AddRange(_filters);

                    _List = _ws.ReadMultipleAsync(_filterArray.ToArray(), _strbookmarkkey, _intsetsize).Result.ReadMultiple_Result1;

                    if (_ws.State == System.ServiceModel.CommunicationState.Opened)
                        await _ws.CloseAsync();
                    return _List;
                }
            }
        }
        #endregion

        #region Item

        public class Item
        {
            public class Card
            {
                //Do something for Card Type
            }

            public class Listing
            {
                public static wsPDAItemList.PDA_Item_List_PortClient GetService()
                {
                    //wsPDAItemList.PDA_Item_List_PortClient _ws = new wsPDAItemList.PDA_Item_List_PortClient(WebServiceBinding._basicHttpBinding(EnumWSType.Page, "CRONUS International Ltd.", "PDA_Item_List"), new System.ServiceModel.EndpointAddress(_webserviceurl));

                    //-1- 
                    //-- create a portClient
                    //-- pass in an empty service Url 
                    //-- return a portClient with an actual WebSvc Url 

                    wsPDAItemList.PDA_Item_List_PortClient _ws = new wsPDAItemList.PDA_Item_List_PortClient(WebServiceBinding._basicHttpBinding(EnumWSType.Page, Company, "PDA_Item_List"), new System.ServiceModel.EndpointAddress(_webserviceurl));

                    _ws.ClientCredentials.Windows.AllowedImpersonationLevel = System.Security.Principal.TokenImpersonationLevel.Delegation;

                    _ws.ClientCredentials.Windows.ClientCredential = WebServiceBinding._networkCredential();

                    return _ws;

                }


                public static async Task<wsPDAItemList.PDA_Item_List> GetAsyncRecords(string _No)
                {
                    wsPDAItemList.PDA_Item_List_PortClient _ws = GetService();
                    wsPDAItemList.PDA_Item_List _List = (await _ws.ReadAsync(_No)).PDA_Item_List;

                    if (_ws.State == System.ServiceModel.CommunicationState.Opened)
                        await _ws.CloseAsync();
                    return _List;
                }


                public static async Task<wsPDAItemList.PDA_Item_List[]> GetAsyncRecords(wsPDAItemList.PDA_Item_List_Filter[] _filters)
                {

                    wsPDAItemList.PDA_Item_List_PortClient _ws = GetService();
                    wsPDAItemList.PDA_Item_List[] _List;
                    List<wsPDAItemList.PDA_Item_List_Filter> _filterArray = new List<wsPDAItemList.PDA_Item_List_Filter>();
                    _filterArray.AddRange(_filters);

                    _List = _ws.ReadMultipleAsync(_filterArray.ToArray(), null, 0).Result.ReadMultiple_Result1;

                    if (_ws.State == System.ServiceModel.CommunicationState.Opened)
                        await _ws.CloseAsync();

                    return _List;
                }


                public static async Task<wsPDAItemList.PDA_Item_List[]> GetAsyncRecords(wsPDAItemList.PDA_Item_List_Filter[] _filters, string _strbookmarkkey)
                {
                    wsPDAItemList.PDA_Item_List_PortClient _ws = GetService();

                    wsPDAItemList.PDA_Item_List[] _List;

                    List<wsPDAItemList.PDA_Item_List_Filter> _filterArray = new List<wsPDAItemList.PDA_Item_List_Filter>();

                    _filterArray.AddRange(_filters);

                    _List = _ws.ReadMultipleAsync(_filterArray.ToArray(), _strbookmarkkey, 0).Result.ReadMultiple_Result1;

                    if (_ws.State == System.ServiceModel.CommunicationState.Opened)
                        await _ws.CloseAsync();
                    return _List;
                }


                public static async Task<wsPDAItemList.PDA_Item_List[]> GetAsyncRecords(wsPDAItemList.PDA_Item_List_Filter[] _filters, string _strbookmarkkey, int _intsetsize)
                {
                    wsPDAItemList.PDA_Item_List_PortClient _ws = GetService();

                    wsPDAItemList.PDA_Item_List[] _List;

                    List<wsPDAItemList.PDA_Item_List_Filter> _filterArray = new List<wsPDAItemList.PDA_Item_List_Filter>();
                    _filterArray.AddRange(_filters);

                    _List = _ws.ReadMultipleAsync(_filterArray.ToArray(), _strbookmarkkey, _intsetsize).Result.ReadMultiple_Result1;

                    if (_ws.State == System.ServiceModel.CommunicationState.Opened)
                        await _ws.CloseAsync();
                    return _List;
                }
            }
        }

        #endregion

        #region ItemCategory
        public class ItemCategory
        {
            public class Card
            {
                //Do something for Card Type
            }

            public class Listing
            {
                public static wsPDAItemCategoryList.PDA_Item_Category_List_PortClient GetService()
                {
                    //--call this ws
                    //wsPDAItemCategoryList.PDA_Item_Category_List_PortClient _ws = new wsPDAItemCategoryList.PDA_Item_Category_List_PortClient(WebServiceBinding._basicHttpBinding(EnumWSType.Page, "CRONUS International Ltd.", "PDA_Item_Category_List"), new System.ServiceModel.EndpointAddress(_webserviceurl));

                    // tun's
                    wsPDAItemCategoryList.PDA_Item_Category_List_PortClient _ws = new wsPDAItemCategoryList.PDA_Item_Category_List_PortClient(WebServiceBinding._basicHttpBinding(EnumWSType.Page, Company, "PDA_Item_Category_List"), new System.ServiceModel.EndpointAddress(_webserviceurl));



                    _ws.ClientCredentials.Windows.AllowedImpersonationLevel = System.Security.Principal.TokenImpersonationLevel.Delegation;

                    _ws.ClientCredentials.Windows.ClientCredential = WebServiceBinding._networkCredential();
                    return _ws;

                }


                public static async Task<wsPDAItemCategoryList.PDA_Item_Category_List> GetAsyncRecords(string _Code)
                {

                    wsPDAItemCategoryList.PDA_Item_Category_List_PortClient _ws = GetService();

                    wsPDAItemCategoryList.PDA_Item_Category_List _List = (await _ws.ReadAsync(_Code)).PDA_Item_Category_List;
                    if (_ws.State == System.ServiceModel.CommunicationState.Opened)
                        await _ws.CloseAsync();
                    return _List;
                }


                public static async Task<wsPDAItemCategoryList.PDA_Item_Category_List[]> GetAsyncRecords(wsPDAItemCategoryList.PDA_Item_Category_List_Filter[] _filters)
                {

                    wsPDAItemCategoryList.PDA_Item_Category_List_PortClient _ws = GetService();
                    wsPDAItemCategoryList.PDA_Item_Category_List[] _List;

                    List<wsPDAItemCategoryList.PDA_Item_Category_List_Filter> _filterArray = new List<wsPDAItemCategoryList.PDA_Item_Category_List_Filter>();
                    _filterArray.AddRange(_filters);

                    _List = _ws.ReadMultipleAsync(_filterArray.ToArray(), null, 0).Result.ReadMultiple_Result1;
                    if (_ws.State == System.ServiceModel.CommunicationState.Opened)
                        await _ws.CloseAsync();

                    return _List;
                }


                public static async Task<wsPDAItemCategoryList.PDA_Item_Category_List[]> GetAsyncRecords(wsPDAItemCategoryList.PDA_Item_Category_List_Filter[] _filters, string _strbookmarkkey)
                {
                    wsPDAItemCategoryList.PDA_Item_Category_List_PortClient _ws = GetService();
                    wsPDAItemCategoryList.PDA_Item_Category_List[] _List;
                    List<wsPDAItemCategoryList.PDA_Item_Category_List_Filter> _filterArray = new List<wsPDAItemCategoryList.PDA_Item_Category_List_Filter>();

                    _filterArray.AddRange(_filters);

                    _List = _ws.ReadMultipleAsync(_filterArray.ToArray(), _strbookmarkkey, 0).Result.ReadMultiple_Result1;
                    if (_ws.State == System.ServiceModel.CommunicationState.Opened)
                        await _ws.CloseAsync();
                    return _List;
                }

                public static async Task<wsPDAItemCategoryList.PDA_Item_Category_List[]> GetAsyncRecords(wsPDAItemCategoryList.PDA_Item_Category_List_Filter[] _filters, string _strbookmarkkey, int _intsetsize)
                {
                    wsPDAItemCategoryList.PDA_Item_Category_List_PortClient _ws = GetService();
                    wsPDAItemCategoryList.PDA_Item_Category_List[] _List;
                    List<wsPDAItemCategoryList.PDA_Item_Category_List_Filter> _filterArray = new List<wsPDAItemCategoryList.PDA_Item_Category_List_Filter>();
                    _filterArray.AddRange(_filters);

                    _List = _ws.ReadMultipleAsync(_filterArray.ToArray(), _strbookmarkkey, _intsetsize).Result.ReadMultiple_Result1;
                    if (_ws.State == System.ServiceModel.CommunicationState.Opened)
                        await _ws.CloseAsync();
                    return _List;
                }
            }
        }
        #endregion


        #region ProductGroup
        public class ProductGroup
        {
            public class Card
            {
                //Do something for Card Type
            }

            public class Listing
            {
                public static wsPDAProductGroupList.PDA_Product_Group_List_PortClient GetService()
                {
                    //--call this ws
                    //wsPDAProductGroupList.PDA_Product_Group_List_PortClient _ws = new wsPDAProductGroupList.PDA_Product_Group_List_PortClient(WebServiceBinding._basicHttpBinding(EnumWSType.Page, "CRONUS International Ltd.", "PDA_Product_Group_List"), new System.ServiceModel.EndpointAddress(_webserviceurl));

                    wsPDAProductGroupList.PDA_Product_Group_List_PortClient _ws = new wsPDAProductGroupList.PDA_Product_Group_List_PortClient(WebServiceBinding._basicHttpBinding(EnumWSType.Page, Company, "PDA_Product_Group_List"), new System.ServiceModel.EndpointAddress(_webserviceurl));


                    _ws.ClientCredentials.Windows.AllowedImpersonationLevel = System.Security.Principal.TokenImpersonationLevel.Delegation;

                    _ws.ClientCredentials.Windows.ClientCredential = WebServiceBinding._networkCredential();

                    return _ws;

                }


                public static async Task<wsPDAProductGroupList.PDA_Product_Group_List> GetAsyncRecords(string _ItemCategoryCode, string _Code)
                {

                    wsPDAProductGroupList.PDA_Product_Group_List_PortClient _ws = GetService();

                    wsPDAProductGroupList.PDA_Product_Group_List _List = (await _ws.ReadAsync(_ItemCategoryCode, _Code)).PDA_Product_Group_List;
                    if (_ws.State == System.ServiceModel.CommunicationState.Opened)
                        await _ws.CloseAsync();
                    return _List;
                }


                public static async Task<wsPDAProductGroupList.PDA_Product_Group_List[]> GetAsyncRecords(wsPDAProductGroupList.PDA_Product_Group_List_Filter[] _filters)
                {

                    wsPDAProductGroupList.PDA_Product_Group_List_PortClient _ws = GetService();
                    wsPDAProductGroupList.PDA_Product_Group_List[] _List;

                    List<wsPDAProductGroupList.PDA_Product_Group_List_Filter> _filterArray = new List<wsPDAProductGroupList.PDA_Product_Group_List_Filter>();
                    _filterArray.AddRange(_filters);

                    _List = _ws.ReadMultipleAsync(_filterArray.ToArray(), null, 0).Result.ReadMultiple_Result1;
                    if (_ws.State == System.ServiceModel.CommunicationState.Opened)
                        await _ws.CloseAsync();

                    return _List;
                }


                public static async Task<wsPDAProductGroupList.PDA_Product_Group_List[]> GetAsyncRecords(wsPDAProductGroupList.PDA_Product_Group_List_Filter[] _filters, string _strbookmarkkey)
                {
                    wsPDAProductGroupList.PDA_Product_Group_List_PortClient _ws = GetService();
                    wsPDAProductGroupList.PDA_Product_Group_List[] _List;
                    List<wsPDAProductGroupList.PDA_Product_Group_List_Filter> _filterArray = new List<wsPDAProductGroupList.PDA_Product_Group_List_Filter>();

                    _filterArray.AddRange(_filters);

                    _List = _ws.ReadMultipleAsync(_filterArray.ToArray(), _strbookmarkkey, 0).Result.ReadMultiple_Result1;
                    if (_ws.State == System.ServiceModel.CommunicationState.Opened)
                        await _ws.CloseAsync();
                    return _List;
                }

                public static async Task<wsPDAProductGroupList.PDA_Product_Group_List[]> GetAsyncRecords(wsPDAProductGroupList.PDA_Product_Group_List_Filter[] _filters, string _strbookmarkkey, int _intsetsize)
                {
                    wsPDAProductGroupList.PDA_Product_Group_List_PortClient _ws = GetService();
                    wsPDAProductGroupList.PDA_Product_Group_List[] _List;
                    List<wsPDAProductGroupList.PDA_Product_Group_List_Filter> _filterArray = new List<wsPDAProductGroupList.PDA_Product_Group_List_Filter>();
                    _filterArray.AddRange(_filters);

                    _List = _ws.ReadMultipleAsync(_filterArray.ToArray(), _strbookmarkkey, _intsetsize).Result.ReadMultiple_Result1;
                    if (_ws.State == System.ServiceModel.CommunicationState.Opened)
                        await _ws.CloseAsync();
                    return _List;
                }
            }
        }
        #endregion


        #region PriceList
        public class PriceList
        {
            public class Card
            {
                //Do something for Card Type
            }

            public class Listing
            {

                public static wsPDASalesPriceService.PDA_Sales_Price_PortClient GetService()
                {
                    wsPDASalesPriceService.PDA_Sales_Price_PortClient _ws = new wsPDASalesPriceService.PDA_Sales_Price_PortClient(WebServiceBinding._basicHttpBinding(EnumWSType.Page, Company, "PDA_Sales_Price"), new System.ServiceModel.EndpointAddress(_webserviceurl));

                    _ws.ClientCredentials.Windows.AllowedImpersonationLevel = System.Security.Principal.TokenImpersonationLevel.Delegation;

                    _ws.ClientCredentials.Windows.ClientCredential = WebServiceBinding._networkCredential();

                    return _ws;
                }


                //-- this is not required:
                /*
                public static async Task<wsPDASalesPriceService.PDA_Sales_Price> GetAsyncRecords(string _Code)
                {
                    //-- this is not used:

                    wsPDASalesPriceService.PDA_Sales_Price_PortClient _ws = GetService();

                    wsPDASalesPriceService.PDA_Sales_Price _List = (await _ws.ReadAsync(_Code, "", "", new DateTime(1753, 1, 1, 0, 0, 0), "", "", "", 0)).PDA_Sales_Price;

                    if (_ws.State == System.ServiceModel.CommunicationState.Opened)
                        await _ws.CloseAsync();

                    return _List;
                }*/



                public static async Task<wsPDASalesPriceService.PDA_Sales_Price[]> GetAsyncRecords(wsPDASalesPriceService.PDA_Sales_Price_Filter[] _filters)
                {

                    wsPDASalesPriceService.PDA_Sales_Price_PortClient _ws = GetService();
                    wsPDASalesPriceService.PDA_Sales_Price[] _List;

                    List<wsPDASalesPriceService.PDA_Sales_Price_Filter> _filterArray = new List<wsPDASalesPriceService.PDA_Sales_Price_Filter>();
                    _filterArray.AddRange(_filters);

                    _List = _ws.ReadMultipleAsync(_filterArray.ToArray(), null, 0).Result.ReadMultiple_Result1;

                    if (_ws.State == System.ServiceModel.CommunicationState.Opened)
                        await _ws.CloseAsync();

                    return _List;

                }


                public static async Task<wsPDASalesPriceService.PDA_Sales_Price[]> GetAsyncRecords(wsPDASalesPriceService.PDA_Sales_Price_Filter[] _filters, string _strbookmarkkey)
                {

                    wsPDASalesPriceService.PDA_Sales_Price_PortClient _ws = GetService();

                    wsPDASalesPriceService.PDA_Sales_Price[] _List;

                    List<wsPDASalesPriceService.PDA_Sales_Price_Filter> _filterArray = new List<wsPDASalesPriceService.PDA_Sales_Price_Filter>();

                    _filterArray.AddRange(_filters);

                    _List = _ws.ReadMultipleAsync(_filterArray.ToArray(), _strbookmarkkey, 0).Result.ReadMultiple_Result1;

                    if (_ws.State == System.ServiceModel.CommunicationState.Opened)

                        await _ws.CloseAsync();

                    return _List;

                }



                public static async Task<wsPDASalesPriceService.PDA_Sales_Price[]> GetAsyncRecords(wsPDASalesPriceService.PDA_Sales_Price_Filter[] _filters, string _strbookmarkkey, int _intsetsize)
                {
                    wsPDASalesPriceService.PDA_Sales_Price_PortClient _ws = GetService();

                    wsPDASalesPriceService.PDA_Sales_Price[] _List;

                    List<wsPDASalesPriceService.PDA_Sales_Price_Filter> _filterArray = new List<wsPDASalesPriceService.PDA_Sales_Price_Filter>();
                    _filterArray.AddRange(_filters);

                    _List = _ws.ReadMultipleAsync(_filterArray.ToArray(), _strbookmarkkey, _intsetsize).Result.ReadMultiple_Result1;

                    if (_ws.State == System.ServiceModel.CommunicationState.Opened)

                        await _ws.CloseAsync();

                    return _List;
                }
            }
        }
        #endregion

        #region
        public class Location
        {
            public class Card
            {
                //Do something for Card Type
            }


            public class Listing
            {

                public static wsPDAUserSetupList.PDA_User_Setup_List_PortClient GetService()
                {
                    wsPDAUserSetupList.PDA_User_Setup_List_PortClient _ws = new wsPDAUserSetupList.PDA_User_Setup_List_PortClient(WebServiceBinding._basicHttpBinding(EnumWSType.Page, Company, "PDA_User_Setup_List"), new System.ServiceModel.EndpointAddress(_webserviceurl));

                    _ws.ClientCredentials.Windows.AllowedImpersonationLevel = System.Security.Principal.TokenImpersonationLevel.Delegation;

                    _ws.ClientCredentials.Windows.ClientCredential = WebServiceBinding._networkCredential();

                    return _ws;
                }


                public static async Task<wsPDAUserSetupList.PDA_User_Setup_List[]> GetAsyncRecords(wsPDAUserSetupList.PDA_User_Setup_List_Filter[] _filters)
                {
                    wsPDAUserSetupList.PDA_User_Setup_List_PortClient _ws = GetService();
                    wsPDAUserSetupList.PDA_User_Setup_List[] _List;

                    List<wsPDAUserSetupList.PDA_User_Setup_List_Filter> _filterArray = new List<wsPDAUserSetupList.PDA_User_Setup_List_Filter>();

                    _filterArray.AddRange(_filters);

                    _List = _ws.ReadMultipleAsync(_filterArray.ToArray(), null, 0).Result.ReadMultiple_Result1;

                    if (_ws.State == System.ServiceModel.CommunicationState.Opened)

                        await _ws.CloseAsync();

                    return _List;

                }


                public static async Task<wsPDAUserSetupList.PDA_User_Setup_List[]> GetAsyncRecords(wsPDAUserSetupList.PDA_User_Setup_List_Filter[] _filters, string _strbookmarkkey, int _intsetsize)
                {

                    wsPDAUserSetupList.PDA_User_Setup_List_PortClient _ws = GetService();

                    wsPDAUserSetupList.PDA_User_Setup_List[] _List;

                    List<wsPDAUserSetupList.PDA_User_Setup_List_Filter> _filterArray = new List<wsPDAUserSetupList.PDA_User_Setup_List_Filter>();

                    _filterArray.AddRange(_filters);

                    _List = _ws.ReadMultipleAsync(_filterArray.ToArray(), _strbookmarkkey, _intsetsize).Result.ReadMultiple_Result1;


                    if (_ws.State == System.ServiceModel.CommunicationState.Opened)

                        await _ws.CloseAsync();

                    return _List;

                }

            }

        }
        #endregion


        #region VatList
        public class VatList
        {
            public class Card
            {
                //Do something for Card Type
            }

            public class Listing
            {
                public static wsPDAVatPosting.PDA_VAT_Posting_Setup_PortClient GetService()
                {

                    wsPDAVatPosting.PDA_VAT_Posting_Setup_PortClient _ws = new wsPDAVatPosting.PDA_VAT_Posting_Setup_PortClient(WebServiceBinding._basicHttpBinding(EnumWSType.Page, Company, "PDA_VAT_Posting_Setup"), new System.ServiceModel.EndpointAddress(_webserviceurl));

                    _ws.ClientCredentials.Windows.AllowedImpersonationLevel = System.Security.Principal.TokenImpersonationLevel.Delegation;

                    _ws.ClientCredentials.Windows.ClientCredential = WebServiceBinding._networkCredential();

                    return _ws;
                }



                public static async Task<wsPDAVatPosting.PDA_VAT_Posting_Setup[]> GetAsyncRecords(wsPDAVatPosting.PDA_VAT_Posting_Setup_Filter[] _filters)
                {
                    wsPDAVatPosting.PDA_VAT_Posting_Setup_PortClient _ws = GetService();

                    wsPDAVatPosting.PDA_VAT_Posting_Setup[] _List;  // = await _ws.ReadMultipleAsync();


                    List<wsPDAVatPosting.PDA_VAT_Posting_Setup_Filter> _filterArray = new List<wsPDAVatPosting.PDA_VAT_Posting_Setup_Filter>();
                    _filterArray.AddRange(_filters);

                    _List = _ws.ReadMultipleAsync(_filterArray.ToArray(), null, 0).Result.ReadMultiple_Result1;

                    if (_ws.State == System.ServiceModel.CommunicationState.Opened)

                        await _ws.CloseAsync();

                    return _List;
                }

                public static async Task<wsPDAVatPosting.PDA_VAT_Posting_Setup[]> GetAsyncRecords(wsPDAVatPosting.PDA_VAT_Posting_Setup_Filter[] _filters, string _strbookmarkkey)
                {

                    wsPDAVatPosting.PDA_VAT_Posting_Setup_PortClient _ws = GetService();

                    wsPDAVatPosting.PDA_VAT_Posting_Setup[] _List;  // = await _ws.ReadMultipleAsync();


                    List<wsPDAVatPosting.PDA_VAT_Posting_Setup_Filter> _filterArray = new List<wsPDAVatPosting.PDA_VAT_Posting_Setup_Filter>();
                    _filterArray.AddRange(_filters);

                    _List = _ws.ReadMultipleAsync(_filterArray.ToArray(), _strbookmarkkey, 0).Result.ReadMultiple_Result1;

                    if (_ws.State == System.ServiceModel.CommunicationState.Opened)

                        await _ws.CloseAsync();

                    return _List;
                }


                public static async Task<wsPDAVatPosting.PDA_VAT_Posting_Setup[]> GetAsyncRecords(wsPDAVatPosting.PDA_VAT_Posting_Setup_Filter[] _filters, string _strbookmarkkey, int _intsetsize)
                {
                    wsPDAVatPosting.PDA_VAT_Posting_Setup_PortClient _ws = GetService();

                    wsPDAVatPosting.PDA_VAT_Posting_Setup[] _List;  // = await _ws.ReadMultipleAsync();


                    List<wsPDAVatPosting.PDA_VAT_Posting_Setup_Filter> _filterArray = new List<wsPDAVatPosting.PDA_VAT_Posting_Setup_Filter>();
                    _filterArray.AddRange(_filters);

                    _List = _ws.ReadMultipleAsync(_filterArray.ToArray(), _strbookmarkkey, _intsetsize).Result.ReadMultiple_Result1;

                    if (_ws.State == System.ServiceModel.CommunicationState.Opened)

                        await _ws.CloseAsync();

                    return _List;
                }


            }
        }
        #endregion "Vat List"


        #region "Payment code"
        public class PaymentCode
        {
            public class Card
            {
                //Do something for Card Type
            }

            public class Listing
            {
                public static wsPDAPaymentCode.PDA_Payment_Methods_PortClient GetService()
                {

                    wsPDAPaymentCode.PDA_Payment_Methods_PortClient _ws = new wsPDAPaymentCode.PDA_Payment_Methods_PortClient(WebServiceBinding._basicHttpBinding(EnumWSType.Page, Company, "PDA_Payment_Methods"), new System.ServiceModel.EndpointAddress(_webserviceurl));

                    _ws.ClientCredentials.Windows.AllowedImpersonationLevel = System.Security.Principal.TokenImpersonationLevel.Delegation;

                    _ws.ClientCredentials.Windows.ClientCredential = WebServiceBinding._networkCredential();

                    return _ws;
                }

                public static async Task<wsPDAPaymentCode.PDA_Payment_Methods[]> GetAsyncRecords(wsPDAPaymentCode.PDA_Payment_Methods_Filter[] _filters)
                {
                    wsPDAPaymentCode.PDA_Payment_Methods_PortClient _ws = GetService();

                    wsPDAPaymentCode.PDA_Payment_Methods[] _List;

                    List<wsPDAPaymentCode.PDA_Payment_Methods_Filter> _filterArray = new List<wsPDAPaymentCode.PDA_Payment_Methods_Filter>();

                    _filterArray.AddRange(_filters);

                    _List = _ws.ReadMultipleAsync(_filterArray.ToArray(), null, 0).Result.ReadMultiple_Result1;

                    if (_ws.State == System.ServiceModel.CommunicationState.Opened)

                        await _ws.CloseAsync();

                    return _List;
                }

                public static async Task<wsPDAPaymentCode.PDA_Payment_Methods[]> GetAsyncRecords(wsPDAPaymentCode.PDA_Payment_Methods_Filter[] _filters, string _strbookmarkkey)
                {
                    wsPDAPaymentCode.PDA_Payment_Methods_PortClient _ws = GetService();

                    wsPDAPaymentCode.PDA_Payment_Methods[] _List;

                    List<wsPDAPaymentCode.PDA_Payment_Methods_Filter> _filterArray = new List<wsPDAPaymentCode.PDA_Payment_Methods_Filter>();

                    _filterArray.AddRange(_filters);

                    _List = _ws.ReadMultipleAsync(_filterArray.ToArray(), _strbookmarkkey, 0).Result.ReadMultiple_Result1;

                    if (_ws.State == System.ServiceModel.CommunicationState.Opened)

                        await _ws.CloseAsync();

                    return _List;
                }

                public static async Task<wsPDAPaymentCode.PDA_Payment_Methods[]> GetAsyncRecords(wsPDAPaymentCode.PDA_Payment_Methods_Filter[] _filters, string _strbookmarkkey, int _intsetsize)
                {
                    wsPDAPaymentCode.PDA_Payment_Methods_PortClient _ws = GetService();

                    wsPDAPaymentCode.PDA_Payment_Methods[] _List;

                    List<wsPDAPaymentCode.PDA_Payment_Methods_Filter> _filterArray = new List<wsPDAPaymentCode.PDA_Payment_Methods_Filter>();


                    _filterArray.AddRange(_filters);

                    _List = _ws.ReadMultipleAsync(_filterArray.ToArray(), _strbookmarkkey, _intsetsize).Result.ReadMultiple_Result1;

                    if (_ws.State == System.ServiceModel.CommunicationState.Opened)

                        await _ws.CloseAsync();

                    return _List;
                }


            }
        }
        #endregion "Payment code"


        #region "Sales Quote/Order/Invoice"
        public class Sales
        {
            public class Card
            {
                //Do something for Card Type
                public static wsPDASalesHeader.PDA_Sales_Header_Card_PortClient GetService()
                {
                    //wsPDASalesHeader.PDA_Sales_Header_Card_PortClient _ws = new wsPDASalesHeader.PDA_Sales_Header_Card_PortClient(WebServiceBinding._basicHttpBinding(EnumWSType.Page, "CRONUS International Ltd.", "PDA_Sales_Header_Card"), new System.ServiceModel.EndpointAddress(_webserviceurl));

                    //-- this tun's
                    wsPDASalesHeader.PDA_Sales_Header_Card_PortClient _ws = new wsPDASalesHeader.PDA_Sales_Header_Card_PortClient(WebServiceBinding._basicHttpBinding(EnumWSType.Page, Company, "PDA_Sales_Header_Card"), new System.ServiceModel.EndpointAddress(_webserviceurl));


                    _ws.ClientCredentials.Windows.AllowedImpersonationLevel = System.Security.Principal.TokenImpersonationLevel.Delegation;
                    _ws.ClientCredentials.Windows.ClientCredential = WebServiceBinding._networkCredential();
                    return _ws;

                }

                public static async Task<wsPDASalesHeader.PDA_Sales_Header_Card> GetAsyncRecords(wsPDASalesHeader.Document_Type _DocumentType, string _No)
                {
                    //-- using the returned _ws

                    wsPDASalesHeader.PDA_Sales_Header_Card_PortClient _ws = GetService();
                    wsPDASalesHeader.PDA_Sales_Header_Card _List = _ws.ReadAsync(_DocumentType.ToString(), _No).Result.PDA_Sales_Header_Card;
                    if (_ws.State == System.ServiceModel.CommunicationState.Opened)
                        await _ws.CloseAsync();
                    return _List;
                }

                public static async Task<wsPDASalesHeader.PDA_Sales_Header_Card[]> GetAsyncRecords(wsPDASalesHeader.PDA_Sales_Header_Card_Filter[] _filters)
                {
                    wsPDASalesHeader.PDA_Sales_Header_Card_PortClient _ws = GetService();
                    wsPDASalesHeader.PDA_Sales_Header_Card[] _List;

                    List<wsPDASalesHeader.PDA_Sales_Header_Card_Filter> _filterArray = new List<wsPDASalesHeader.PDA_Sales_Header_Card_Filter>();
                    _filterArray.AddRange(_filters);

                    _List = _ws.ReadMultipleAsync(_filterArray.ToArray(), null, 0).Result.ReadMultiple_Result1;
                    if (_ws.State == System.ServiceModel.CommunicationState.Opened)
                        await _ws.CloseAsync();

                    return _List;
                }

                public static async Task<wsPDASalesHeader.PDA_Sales_Header_Card[]> GetAsyncRecords(wsPDASalesHeader.PDA_Sales_Header_Card_Filter[] _filters, string _strBookmark)
                {
                    wsPDASalesHeader.PDA_Sales_Header_Card_PortClient _ws = GetService();
                    wsPDASalesHeader.PDA_Sales_Header_Card[] _List;
                    List<wsPDASalesHeader.PDA_Sales_Header_Card_Filter> _filterArray = new List<wsPDASalesHeader.PDA_Sales_Header_Card_Filter>();
                    _filterArray.AddRange(_filters);
                    _List = _ws.ReadMultipleAsync(_filterArray.ToArray(), _strBookmark, 0).Result.ReadMultiple_Result1;
                    if (_ws.State == System.ServiceModel.CommunicationState.Opened)
                        await _ws.CloseAsync();

                    return _List;
                }

                public static async Task<wsPDASalesHeader.PDA_Sales_Header_Card[]> GetAsyncRecords(wsPDASalesHeader.PDA_Sales_Header_Card_Filter[] _filters, string _strBookmark, int _intSetSize)
                {
                    wsPDASalesHeader.PDA_Sales_Header_Card_PortClient _ws = GetService();
                    wsPDASalesHeader.PDA_Sales_Header_Card[] _List;
                    List<wsPDASalesHeader.PDA_Sales_Header_Card_Filter> _filterArray = new List<wsPDASalesHeader.PDA_Sales_Header_Card_Filter>();
                    _filterArray.AddRange(_filters);
                    _List = _ws.ReadMultipleAsync(_filterArray.ToArray(), _strBookmark, _intSetSize).Result.ReadMultiple_Result1;
                    if (_ws.State == System.ServiceModel.CommunicationState.Opened)
                        await _ws.CloseAsync();

                    return _List;
                }

                public static bool DocumentIsOpen(wsPDASalesHeader.PDA_Sales_Header_Card _Card)
                {
                    wsPDASalesHeader.Status _enumStatus = _Card.Status;
                    return (_enumStatus == wsPDASalesHeader.Status.Open);
                }


                public static async Task<bool> DocumentIsOpen(wsPDASalesHeader.Document_Type _DocumentType, string _No)
                {
                    wsPDASalesHeader.Status _enumStatus = await GetDocumentStatus(_DocumentType, _No);
                    return (_enumStatus == wsPDASalesHeader.Status.Open);
                }

                public static async Task<wsPDASalesHeader.Status> GetDocumentStatus(wsPDASalesHeader.Document_Type _DocumentType, string _No)
                {
                    wsPDASalesHeader.PDA_Sales_Header_Card _pDA_Sales_Header_Card = await GetAsyncRecords(_DocumentType, _No);
                    return _pDA_Sales_Header_Card.Status;

                }


                public static async Task<wsPDASalesHeader.PDA_Sales_Header_Card> Create(wsPDASalesHeader.Document_Type _DocumentType, string _No)
                {
                    wsPDASalesHeader.PDA_Sales_Header_Card_PortClient _ws = GetService();

                    wsPDASalesHeader.PDA_Sales_Header_Card _pDA_Sales_Header_Card = new wsPDASalesHeader.PDA_Sales_Header_Card();

                    //-- specify properties to the card

                    _pDA_Sales_Header_Card.Document_TypeSpecified = true;

                    _pDA_Sales_Header_Card.Document_Type = _DocumentType;

                    if (!string.IsNullOrWhiteSpace(_No))

                        _pDA_Sales_Header_Card.No = _No;

                    //--- continue if No is null or empty

                    wsPDASalesHeader.Create _create = new wsPDASalesHeader.Create(_pDA_Sales_Header_Card);

                    wsPDASalesHeader.Create_Result _create_Result = await _ws.CreateAsync(_create);

                    if (_ws.State == System.ServiceModel.CommunicationState.Opened)
                        await _ws.CloseAsync();

                    return _create_Result.PDA_Sales_Header_Card;

                }

                public static async Task<wsPDASalesHeader.PDA_Sales_Header_Card> Modify(wsPDASalesHeader.PDA_Sales_Header_Card _Card)
                {
                    wsPDASalesHeader.PDA_Sales_Header_Card_PortClient _ws = GetService();
                    wsPDASalesHeader.Update _update = new wsPDASalesHeader.Update(_Card);
                    wsPDASalesHeader.Update_Result _update_Result = await _ws.UpdateAsync(_update);
                    if (_ws.State == System.ServiceModel.CommunicationState.Opened)
                        await _ws.CloseAsync();
                    return _update_Result.PDA_Sales_Header_Card;
                }

                public static async Task<bool> Delete(string _Key)
                {
                    wsPDASalesHeader.PDA_Sales_Header_Card_PortClient _ws = GetService();
                    wsPDASalesHeader.Delete_Result _delete_Result = await _ws.DeleteAsync(_Key);
                    if (_ws.State == System.ServiceModel.CommunicationState.Opened)
                        await _ws.CloseAsync();
                    return _delete_Result.Delete_Result1;
                }
            }



            public class SubForm
            {
                public static int GetEditLineIndex(wsPDASalesHeader.PDA_Sales_Header_Card _Card, int _intLineNo)
                {
                    int _intIndex = -1;
                    wsPDASalesHeader.PDA_Sales_Line_Line[] _SubFormList = _Card.PDA_Sales_Line_Line;
                    if (_SubFormList != null)
                    {
                        _intIndex = _SubFormList.Select((_SubForm, i) => new { _SubForm = _SubForm, Index = i })
                                    .First(x => x._SubForm.Line_No == _intLineNo).Index;
                    }
                    return _intIndex;
                }

                public static int GetEditLineIndex(wsPDASalesHeader.Document_Type _DocumentType, string _No, int _intLineNo)
                {
                    int _intIndex = -1;
                    wsPDASalesHeader.PDA_Sales_Line_Line[] _SubFormList = Class1.Sales.Card.GetAsyncRecords(_DocumentType, _No).Result.PDA_Sales_Line_Line;
                    if (_SubFormList != null)
                    {
                        _intIndex = _SubFormList.Select((_SubForm, i) => new { _SubForm = _SubForm, Index = i })
                                    .First(x => x._SubForm.Line_No == _intLineNo).Index;
                    }
                    return _intIndex;
                }

                public static async Task<wsPDASalesHeader.PDA_Sales_Line_Line> Create(wsPDASalesHeader.PDA_Sales_Header_Card _Card)
                {
                    wsPDASalesHeader.PDA_Sales_Header_Card_PortClient _ws = new wsPDASalesHeader.PDA_Sales_Header_Card_PortClient();
                    _Card.PDA_Sales_Line_Line = new wsPDASalesHeader.PDA_Sales_Line_Line[1];
                    _Card.PDA_Sales_Line_Line[0] = new wsPDASalesHeader.PDA_Sales_Line_Line();
                    _Card = await Sales.Card.Modify(_Card);
                    return _Card.PDA_Sales_Line_Line[_Card.PDA_Sales_Line_Line.Length - 1];
                }

                public static async Task<wsPDASalesHeader.PDA_Sales_Line_Line> Modify(wsPDASalesHeader.PDA_Sales_Header_Card _Card, int _intLineIndex)
                {
                    _Card = await Sales.Card.Modify(_Card);

                    return _Card.PDA_Sales_Line_Line[_intLineIndex];

                }

                public static async Task<bool> Delete(string _Key)
                {
                    wsPDASalesHeader.PDA_Sales_Header_Card_PortClient _ws = Sales.Card.GetService();
                    wsPDASalesHeader.Delete_PDA_Sales_Line_Line_Result _delete_Result = await _ws.Delete_PDA_Sales_Line_LineAsync(_Key);
                    if (_ws.State == System.ServiceModel.CommunicationState.Opened)
                        await _ws.CloseAsync();
                    return _delete_Result.Delete_PDA_Sales_Line_Line_Result1;
                }
            }
        }
        #endregion


    }
}