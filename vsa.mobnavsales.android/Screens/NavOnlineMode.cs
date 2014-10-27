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


         // use code for we
    }
    
}