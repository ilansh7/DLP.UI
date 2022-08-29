using DLP.Common;
using Dapper;
using IniParser;
using Newtonsoft;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Linq;
using System.Text;
using System;
using System.Dynamic;
using System.Reflection;
using Newtonsoft.Json;

namespace DLP.Data
{
    public static class StringExtensions
    {
        public static string FirstCharToUpper(this string input)
        {
            switch (input)
            {
                case null: throw new ArgumentNullException(nameof(input));
                case "": throw new ArgumentException($"{nameof(input)} cannot be empty", nameof(input));
                default:
                    string[] inp = input.Split('_');
                    if (inp.Length == 1)
                    {
                        return input[0].ToString().ToUpper() + input.Substring(1);
                    }
                    else
                    {
                        string ret = string.Empty;
                        foreach (var str in inp)
                        {
                            if (!string.IsNullOrEmpty(ret))
                            {
                                ret = string.Concat(ret, "_");
                            }
                            ret = string.Concat(ret, str[0].ToString().ToUpper(), str.Substring(1));
                        }
                        return ret;
                    }
            }
        }
    }

    public class TlsDlpData
    {
        #region variables
        private readonly string TLSDLP_CONNSTR = "TlsDlpWebContext";
        private static Data.SafeT _safeT;
        private static string _controllereName = "TlsDlpData";
        private static string _methodeName = "";
        #endregion variables
        #region propertirs
        public string TlsDlpConnStr
        {
            //get { return ConfigurationManager.ConnectionStrings[TLSDLP_CONNSTR].ToString(); }
            get { return ConfigurationManager.ConnectionStrings["TlsDlpWebContext"].ToString(); }
        }

        public static Data.SafeT SafeT
        {
            get
            {
                if (_safeT == null)
                {
                    return _safeT = new Data.SafeT();
                }
                else
                {
                    return _safeT;
                }
            }
        }
        #endregion propertirs
        #region constractor
        public TlsDlpData()
        {
            _safeT = new Data.SafeT();
        }
        #endregion constractor
        #region methods
        //To Handle connection related activities      
        //private void connection()
        //{
        //    //string cStr = ConfigurationManager.ConnectionStrings[ConStr].ToString();
        //    _conn = new SqlConnection(TlsDlpConnStr);
        //}

        public IEnumerable<Entities.V_DlpTlsInfo> ExcludeDomainsReport(int emailType)
        {
            _methodeName = "ExcludeDomainsReport";
            Log.Logging(LoggingMode.Debug, "Starting {controller}\\{methode}", _controllereName, _methodeName);
            Log.Logging(LoggingMode.Debug, "\t Parameters : EmailType:{emailType}", emailType);

            try
            {
                using (var connection = new SqlConnection(TlsDlpConnStr))
                {
                    var dlps = connection.GetList<Entities.V_DlpTlsInfo>(new { EmailType = emailType }).AsList();
                    //var dlps = connection.Query<Entities.DLP>("dbo.SP_ExcludeDomainsReport",
                    //    param: new { emailType },
                    //    commandType: CommandType.StoredProcedure).AsList();
                    Log.Logging(LoggingMode.Debug, "Exit {controller}\\{methode}", _controllereName, _methodeName);
                    return dlps;
                }
                //return null;
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
            Log.Logging(LoggingMode.Debug, "\t Parameters : SearchText:{searchText}", searchText);

            try
            {
                bool isEmailDlpFlag = false;
                string query = string.Empty;
                modelParams = new Dictionary<string, object>();
                //dictDlpTls.Add("Tls_Name", sec_name);

                string[] domain = searchText.Split('@');
                if (domain.Length > 1)
                {
                    //query = string.Concat("where LOWER(EmailAddress) = LOWER('", domain[1], "')");
                    query = string.Concat("where LOWER(EmailAddress) = LOWER('", searchText, "')");
                    isEmailDlpFlag = true;
                    modelParams.Add("SearchDomain", (string)domain[1]);
                    //modelParams.Add("IsEmailDlpFlag", true);
                }
                else
                {
                    query = string.Concat("where LOWER(Tls_Name) = LOWER('", searchText, "')");
                }
                query = string.Concat(query, " order by EmailType desc");
                using (var connection = new SqlConnection(TlsDlpConnStr))
                {
                    var dlps = connection.GetList<Entities.V_DlpTlsInfo>(query).AsList();
                    // no results by mail, check the domain
                    if (dlps.Count <= 0)
                    {
                        if (domain.Length > 1)
                        {
                            query = string.Concat("where LOWER(EmailAddress) = LOWER('", domain[1], "')", " order by EmailType desc");
                            dlps = connection.GetList<Entities.V_DlpTlsInfo>(query).AsList();
                        }
                        else
                        {
                            modelParams.Add("IsDomainDlp", false);
                        }
                    }
                    else
                    {
                        //isDomainDlp = dlps.FirstOrDefault().EmailType;
                        modelParams.Add("IsDomainDlp", (bool)dlps.FirstOrDefault().EmailType);
                    }
                    // something found
                    if (dlps.Count > 0)
                    {
                        if (domain.Length > 1)
                        {
                            //isSafetPass = SafeT.IsSafetPass(domain[1], empCompany);
                            modelParams.Add("IsSafetPass", (bool)SafeT.IsSafeTPass(domain[1], empCompany));
                        }
                        else
                        {
                            //isSafetPass = SafeT.IsSafetPass(searchText, empCompany);
                            modelParams.Add("IsSafetPass", (bool)SafeT.IsSafeTPass(searchText, empCompany));
                        }
                        //isEmailDlp = isDomainDlp && isEmailDlpFlag;
                        modelParams.Add("IsEmailDlp", (bool)modelParams["IsDomainDlp"] & isEmailDlpFlag);
                        //var dlps = connection.GetList<Entities.V_DlpTlsInfo>(new { EmailType = emailType }).AsList();
                        //var dlps = connection.Query<Entities.DLP>("dbo.SP_ExcludeDomainsReport",
                        //    param: new { emailType },
                        //    commandType: CommandType.StoredProcedure).AsList();
                    }
                    Log.Logging(LoggingMode.Debug, "Exit {controller}\\{methode}", _controllereName, _methodeName);
                    return dlps;
                }
                //return new[] { new Entities.V_DlpTlsInfo() };
            }
            catch (System.Exception ex)
            {
                Log.Logging(LoggingMode.Error, "\t Exception on {controller}\\{methode} : {@ex}", _controllereName, _methodeName, ex);
                throw ex;
            }
        }

        public List<string> GetDomainsAndMails(string searchText)
        {
            _methodeName = "GetDomainsAndMails";
            Log.Logging(LoggingMode.Debug, "Starting {controller}\\{methode}", _controllereName, _methodeName);
            Log.Logging(LoggingMode.Debug, "\t Parameters : SearchText:{searchText}", searchText);

            try
            {
                var query = string.Concat("select EmailAddress from V_DlpTlsInfo where EmailAddress like '%", searchText, "%'");
                using (var connection = new SqlConnection(TlsDlpConnStr))
                {
                    var dlps = connection.Query<string>(query).ToList<string>();
                    return dlps;
                }
            }
            catch (Exception ex)
            {
                Log.Logging(LoggingMode.Error, "\t Exception on {controller}\\{methode} : {@ex}", _controllereName, _methodeName, ex);
                throw ex;
            }
            throw new NotImplementedException();
        }

        public Entities.V_DlpTlsInfo SearchDomain(string searchText)
        {
            _methodeName = "SearchDomains";
            Log.Logging(LoggingMode.Debug, "Starting {controller}\\{methode}", _controllereName, _methodeName);
            Log.Logging(LoggingMode.Debug, "\t Parameters : SearchText:{searchText}", searchText);

            try
            {
                string query = string.Empty;
                string[] domain = searchText.Split('@');
                if (domain.Length > 1)
                {
                    //query = string.Concat("where LOWER(EmailAddress) = LOWER('", domain[1], "')");
                    query = string.Concat("where LOWER(EmailAddress) = LOWER('", searchText, "')");
                }
                else
                {
                    query = string.Concat("where LOWER(Tls_Name) = LOWER('", searchText, "')");
                }
                using (var connection = new SqlConnection(TlsDlpConnStr))
                {
                    var dlps = connection.GetList<Entities.V_DlpTlsInfo>(query).AsList();

                    //var dlps = connection.GetList<Entities.V_DlpTlsInfo>(new { EmailType = emailType }).AsList();
                    //var dlps = connection.Query<Entities.DLP>("dbo.SP_ExcludeDomainsReport",
                    //    param: new { emailType },
                    //    commandType: CommandType.StoredProcedure).AsList();
                    Log.Logging(LoggingMode.Debug, "Exit {controller}\\{methode}", _controllereName, _methodeName);
                    return dlps.FirstOrDefault();
                }
                //return new[] { new Entities.V_DlpTlsInfo() };
            }
            catch (System.Exception ex)
            {
                Log.Logging(LoggingMode.Error, "\t Exception on {controller}\\{methode} : {@ex}", _controllereName, _methodeName, ex);
                throw ex;
            }
        }

        //public Entities.V_DlpTlsInfo SearchDomains()
        //{
        //    return null;
        //}
        public Entities.DlpTls SearchDomains(string dmn, int empCompany, bool isSitePermissionManagment, out string viewName, out bool reditect)
        {
            _methodeName = "SearchDomains";
            Log.Logging(LoggingMode.Debug, "Starting {controller}\\{methode}", _controllereName, _methodeName);
            Log.Logging(LoggingMode.Debug, "\t Parameters : SearchText:{dmn} ; EmpCompany:{empCompany}", dmn, empCompany);
            //string connStr = ConfigurationManager.ConnectionStrings[TLSDLP_CONNSTR].ToString();
            //Model model = new Model();

            Entities.DlpTls model = new Entities.DlpTls();
            //IEnumerable<Entities.DlpTls> model = new List<Entities.DlpTls>();
            model.SearchText = dmn;
            model.EmpCompany = empCompany;
            string query = string.Empty;
            IEnumerable<Entities.DLP> dlp;
            IEnumerable<Entities.TLS> tls;
            reditect = false;
            try
            {
                using (var connection = new SqlConnection(TlsDlpConnStr))
                {
                    //dlp = connection.GetList<Entities.DLP>().AsList();
                    //tls = connection.GetList<Entities.TLS>().AsList();
                    //var dlptls = connection.GetList<Entities.V_DlpTlsInfo>().AsList();

                    //Check user company MOVED TO CONTROLLER
                    //model.EmpCompany = Security.IsInGroup(@"SW1\SgyUsers") ? 2 : 1;

                    if (!string.IsNullOrEmpty(model.SearchText))
                    {
                        string[] domain = model.SearchText.Split('@');
                        model.IsEmailSearch = domain.Count() > 1 ? true : false;
                        if (model.IsEmailSearch) //Full email address entered
                        {
                            model.SearchDomain = domain[1].ToLower();
                            query = string.Concat("where LOWER(Name) = LOWER('", model.SearchDomain, "')");
                            tls = connection.GetList<Entities.TLS>(query).AsList();
                            if (tls.Count() > 0) //TLS Exist
                            {
                                model.IsTls = true;
                                query = string.Concat("where LOWER(EmailAddress) = LOWER('", model.SearchText, "')");
                                dlp = connection.GetList<Entities.DLP>(query).AsList();
                                if (dlp.Count() < 1) //Email Address NOT Exist on DLP DB
                                {
                                    model.IsSafetPass = SafeT.IsSafeTPass(model.SearchText, model.EmpCompany);
                                    query = string.Concat("where EmailAddress like '%", model.SearchDomain, "'");
                                    dlp = connection.GetList<Entities.DLP>(query).AsList(); //Check if other addresses exclude
                                    if (dlp.Count() > 0)  //Other mail excluded in the domain
                                    {
                                        if (dlp.FirstOrDefault().EmailType) //Check if all domain exclude
                                        {
                                            model.RecipientCompany = 5;
                                            model.IsDomainDlp = true;
                                            model.IsDlp = true;
                                            if (dlp.FirstOrDefault().IsAllowEncrypted)
                                            {
                                                model.IsAllowEncryption = true;
                                            }
                                        }
                                        else
                                        {
                                            model.IsDomainDlp = false;
                                            model.IsDlp = true;
                                        }
                                    }
                                    else //TLS exist but not Dlp
                                    {
                                        model.IsDomainDlp = false;
                                        model.IsDlp = false;
                                        model.IsTls = true;
                                    }
                                }
                                else //Email EXIST in Dlp DB
                                {
                                    model.IsDlp = true;
                                    model.IsEmailDlp = true;

                                    model.RecipientCompany = int.Parse(dlp.FirstOrDefault().CompanyType);
                                    if (dlp.Count() > 0 && dlp.FirstOrDefault().EmailType)
                                    {
                                        model.IsDomainDlp = true;
                                    }
                                    if (dlp.FirstOrDefault().IsAllowEncrypted)
                                    {
                                        model.IsAllowEncryption = true;
                                    }
                                }
                            }

                            ////else //NO TLS
                            ////{
                            ////    model.IsSafetPass = Helper.IsSafetPass(model.SearchText, model.EmpCompany);
                            ////    //if(model.IsSafetPass)
                            ////    //{
                            ////    //    //return View("search", model);
                            ////    //    return View(model);
                            ////    //}

                            ////}
                        }
                        else //Search for Domain
                        {
                            model.IsDomainSearch = true;
                            model.SearchDomain = domain[0].ToLower();
                            query = string.Concat("where Name like '%", model.SearchDomain, "'");
                            tls = connection.GetList<Entities.TLS>(query).AsList();
                            if (tls.Count() > 0) //Tls EXIST
                            {
                                model.IsTls = true;
                                query = string.Concat("where EmailAddress like '%", model.SearchDomain, "'");
                                dlp = connection.GetList<Entities.DLP>(query).AsList();
                                if (dlp.Count() > 0) //Domain EXIST in DLP DB
                                {
                                    if (dlp.FirstOrDefault().EmailType) //All domain Exclude
                                    {
                                        model.IsDomainDlp = true;
                                        model.IsEmailDlp = true;
                                        if (dlp.FirstOrDefault().IsAllowEncrypted)
                                        {
                                            model.IsAllowEncryption = true;
                                        }
                                    }
                                    else //All domain NOT Exclude
                                    {
                                        model.IsDomainDlp = false;
                                        model.IsDlp = true;
                                        if (dlp.FirstOrDefault().IsAllowEncrypted)
                                        {
                                            model.IsAllowEncryption = true;
                                        }
                                    }
                                    model.Tls = tls.ToList();
                                    model.Dlp = dlp.ToList();
                                }
                                else //Domain NOT EXIST on Dlp DB
                                {
                                }
                            }
                        }
                        if (model.IsDomainDlp)
                        {
                            model.RecipientCompany = 5;
                        }
                        if (isSitePermissionManagment)
                        //if (Security.IsInGroup(@"SW1\TlsDlpSitePermissionManagment")) //Show all Dlp 
                        {
                            model.EmpCompany += 100;
                        }

                        if (tls != null && tls.Count() > 0)
                        {
                            if (model.IsEmailSearch)
                            {
                                //return View(model);
                                Log.Logging(LoggingMode.Debug, "Exit {controller}\\{methode} 0", _controllereName, _methodeName);
                                viewName = "Index";
                                return model;
                            }
                            else if (model.IsDomainSearch)
                            {
                                //return View("Search", model);
                                Log.Logging(LoggingMode.Debug, "Exit {controller}\\{methode} 1", _controllereName, _methodeName);
                                viewName = "Search";
                                return model;
                            }
                        }
                        else //No Tls
                        {
                            if (model.IsEmailSearch)
                            {
                                model.IsSafetPass = SafeT.IsSafeTPass(model.SearchText, model.EmpCompany);
                                if (model.IsSafetPass)
                                {
                                    model.RecipientCompany = model.EmpCompany;
                                }
                                else
                                {
                                    //return RedirectToAction("Privacy");
                                    Log.Logging(LoggingMode.Debug, "Exit {controller}\\{methode} 2", _controllereName, _methodeName);
                                    reditect = true;
                                    viewName = "Privacy";
                                    return model;
                                }
                                //return View(model);
                                Log.Logging(LoggingMode.Debug, "Exit {controller}\\{methode} 3", _controllereName, _methodeName);
                                viewName = "Index";
                                return model;
                            }
                            else
                            {
                                //return View(model);
                                Log.Logging(LoggingMode.Debug, "Exit {controller}\\{methode} 4", _controllereName, _methodeName);
                                viewName = "Index";
                                return model;
                            }
                        }
                        if (model.IsEmailSearch)
                        {
                            //return View(model);
                            Log.Logging(LoggingMode.Debug, "Exit {controller}\\{methode} 5", _controllereName, _methodeName);
                            viewName = "Index";
                            return model;
                        }
                        else if (model.IsDomainSearch)
                        {
                            //return View("search", model);
                            Log.Logging(LoggingMode.Debug, "Exit {controller}\\{methode} 6", _controllereName, _methodeName);
                            viewName = "Search";
                            return model;
                        }
                    }
                }
                Log.Logging(LoggingMode.Debug, "Exit {controller}\\{methode} 7", _controllereName, _methodeName);
                viewName = "";
                return model;
            }
            catch (System.Exception ex)
            {
                Log.Logging(LoggingMode.Error, "\t Exception on {controller}\\{methode} : {@ex}", _controllereName, _methodeName, ex);
                throw ex;
            }
        }
        public static void AddProperty(ExpandoObject expando, string propertyName, object propertyValue)
        {
            // ExpandoObject supports IDictionary so we can extend it like this
            var expandoDict = expando as IDictionary<string, object>;
            if (expandoDict.ContainsKey(propertyName))
                expandoDict[propertyName] = propertyValue;
            else
                expandoDict.Add(propertyName, propertyValue);
        }
        public void ParseTlsDataFile()
        {
            _methodeName = "ParseTlsDataFile";
            Log.Logging(LoggingMode.Debug, "Starting {controller}\\{methode}", _controllereName, _methodeName);
            //Log.Logging(LoggingMode.Debug, "\t Parameters : This methode has No Parametrs");

            int? dlps;
            string tlsDataFile = ConfigurationManager.AppSettings["TlsDataFile"].ToString();
            try
            {
                var parser = new FileIniDataParser();
                IniParser.Model.IniData data = parser.ReadFile(tlsDataFile);

                using (var connection = new SqlConnection(TlsDlpConnStr))
                {
                    foreach (var section in data.Sections)
                    {
                        Dictionary<string, object> dictDlpTls = new Dictionary<string, object>();
                        //string keys = string.Empty;
                        //string values = string.Empty;
                        var sec_name = section.SectionName.ToString();
                        dictDlpTls.Add("Tls_Name", sec_name);
                        dictDlpTls.Add("Tls_Id", 0);
                        foreach (var key in section.Keys)
                        {
                            var keyName = StringExtensions.FirstCharToUpper(key.KeyName);
                            //keys += (keys == string.Empty ? keyName : string.Concat(", ", keyName));
                            //values += (values == string.Empty ? string.Concat("'", key.Value, "'") : string.Concat(", ", "'", key.Value, "'"));
                            dictDlpTls.Add(keyName, key.Value.ToString());
                        }

                        var json = JsonConvert.SerializeObject(dictDlpTls, Newtonsoft.Json.Formatting.Indented);
                        DLP.Entities.DlpTlsInfo dlpTlsInfo = JsonConvert.DeserializeObject<DLP.Entities.DlpTlsInfo>(json);

                        var entityDlpTlsInfo = connection.GetList<Entities.DlpTlsInfo>(new { Tls_Name = sec_name }).ToList();
                        if (entityDlpTlsInfo.Count > 0)
                        {
                            dlpTlsInfo.ID = entityDlpTlsInfo[0].ID;
                            dlps = connection.Update<Entities.DlpTlsInfo>(dlpTlsInfo);
                        }
                        else
                        {
                            dlps = connection.Insert<Entities.DlpTlsInfo>(dlpTlsInfo);
                        }
                    }
                    string query = "update [dbo].[DlpTlsInfo] set Tls_Id = t.ID from [dbo].[TLS] t where UPPER(t.Name) = UPPER(Tls_Name)";
                    var ret = connection.Query<Entities.DlpTlsInfo>(query);
                }
                System.IO.File.Delete(tlsDataFile);
                Log.Logging(LoggingMode.Debug, "Exit {controller}\\{methode}", _controllereName, _methodeName);
            }
            catch (IniParser.Exceptions.ParsingException exParsingException)
            {
                Log.Logging(LoggingMode.Error, "\t Exception on {controller}\\{methode} : {@ex}", _controllereName, _methodeName, exParsingException);
                //throw exParsingException;
                Log.Logging(LoggingMode.Debug, "Exit {controller}\\{methode}", _controllereName, _methodeName);
            }
            catch (System.IO.FileNotFoundException exFileNotFound)
            {
                Log.Logging(LoggingMode.Error, "\t Exception on {controller}\\{methode} : {@ex}", _controllereName, _methodeName, exFileNotFound);
                throw exFileNotFound;
            }
        }
        #endregion methods
    }
}
