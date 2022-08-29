using DLP.Common;
using System.Collections.Generic;

namespace DLP.Services
{
    public class DLPServices
    {
        #region variables
        private static Data.TlsDlpData _data;
        private static string _controllereName = "DLPServices";
        private static string _methodeName = "";
        #endregion variables
        #region propertirs
        public Data.TlsDlpData Data
        {
            get
            {
                if (_data == null)
                {
                    return _data = new Data.TlsDlpData();
                }
                else
                {
                    return _data;
                }
            }
        }
        #endregion propertirs
        #region constractor
        public DLPServices()
        {
            _data = new Data.TlsDlpData();
        }
        #endregion constractor
        #region methods
        public IEnumerable<Entities.V_DlpTlsInfo> ExcludeDomainsReport(int emailType)
        {
            _methodeName = "ExcludeDomainsReport";
            Log.Logging(LoggingMode.Debug, "Starting {controller}\\{methode}", _controllereName, _methodeName);
            Log.Logging(LoggingMode.Debug, "\t Parameters : EmailType:{emailType}", emailType);
            try
            {
                Log.Logging(LoggingMode.Information, "\t Getting context of V_DlpTlsInfo. ({controller}\\{methode})", _controllereName, _methodeName);
                var ret = Data.ExcludeDomainsReport(emailType);
                Log.Logging(LoggingMode.Debug, "Exit {controller}\\{methode}", _controllereName, _methodeName);
                return ret;
            }
            catch (System.Exception ex)
            {
                Log.Logging(LoggingMode.Error, "\t Exception on {controller}\\{methode} : {@ex}", _controllereName, _methodeName, ex);
                throw ex;
            }
        }

        public IEnumerable<Entities.V_DlpTlsInfo> SearchDomains(string searchText, int empCompany, out Dictionary<string, object> modelParams)
        {
            _methodeName = "SearchDomains";
            Log.Logging(LoggingMode.Debug, "Starting {controller}\\{methode}", _controllereName, _methodeName);
            Log.Logging(LoggingMode.Debug, "\t Parameters :  Search Text:{searchText}, Emp Company:{empCompany}", searchText, empCompany);

            if (string.IsNullOrEmpty(searchText))
            {
                IEnumerable<Entities.V_DlpTlsInfo> ret = new List<Entities.V_DlpTlsInfo>();
                Log.Logging(LoggingMode.Error, "\t Parameter Search Cannot be Empty");
                modelParams = null;
                Log.Logging(LoggingMode.Debug, "Exit {controller}\\{methode}", _controllereName, _methodeName);
                return ret;
            }
            try
            {
                Log.Logging(LoggingMode.Information, "\t Getting context of V_DlpTlsInfo. ({controller}\\{methode})", _controllereName, _methodeName);
                var ret = Data.SearchDomains(searchText, empCompany, out modelParams);
                Log.Logging(LoggingMode.Debug, "\t Out Parameters : model properties:{modelParams}", modelParams);
                Log.Logging(LoggingMode.Debug, "Exit {controller}\\{methode}", _controllereName, _methodeName);
                return ret;
            }
            catch (System.Exception ex)
            {
                Log.Logging(LoggingMode.Error, "\t Exception on {controller}\\{methode} : {@ex}", _controllereName, _methodeName, ex);
                throw ex;
            }
        }

        public Entities.V_DlpTlsInfo SearchDomain(string searchText)
        {
            return Data.SearchDomain(searchText);
        }

        public Entities.DlpTls SearchDomains(string domain, int empCompany, bool isSitePermissionManagment, out string viewName, out bool reditect)
        {
            //string name = viewName;
            return Data.SearchDomains(domain, empCompany, isSitePermissionManagment, out viewName, out reditect);
        }

        public void ParseTlsDataFile()
        {
            Data.ParseTlsDataFile();
        }

        public List<string> GetDomainsAndMails(string searchText)
        {
            //List<string> result = new List<string>();
            return Data.GetDomainsAndMails(searchText);
        }

        //public async Task<List<Entities.DLP>> ExcludeDomainsReport1(int emailType)
        //{
        //    //ConStr = cs;
        //    //var sql = Conn;
        //    var tst = _data.ExcludeDomainsReport(emailType);
        //    return null;
        //}

        //public string 
        //public async Task<List<UserDLP>> GetUsersDLP()
        //{
        //    var userDLP = new UserDLP();
        //    userDLP.TlsID = 0;
        //    var userDLPList = new List<UserDLP>();
        //    userDLPList.Add(userDLP);
        //    return userDLPList;
        //}
        #endregion methods
    }
}
