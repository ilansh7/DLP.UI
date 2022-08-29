using AutoMapper;
using DLP.Common;
using DLP.Services;
using DLP.UI.Models;
using System.Collections.Generic;
using System.Web.Mvc;

namespace DLP.UI.Controllers
{
    public class HomeController : Controller
    {
        #region variables
        public Entities.DlpTls _model;
        private static DLPServices _srv;
        //private SecurityServices _security;
        private static AutoMapper.MapperConfiguration _searchVmConfig;
        private static AutoMapper.Mapper _searchVmMapper;
        private static string _controllereName = "Home";
        private static string _methodeName = "";
        #endregion variables
        #region propertirs
        //public SecurityServices security
        //{
        //    get { return _security; }
        //}
        public static AutoMapper.MapperConfiguration searchVmConfig
        {
            get { return _searchVmConfig; }
            set { _searchVmConfig = value; }
        }
        public static DLPServices service
        {
            get
            {
                if (_srv == null)
                {
                    return _srv = new DLPServices();
                }
                else
                {
                    return _srv;
                }
            }
        }
        #endregion propertirs
        #region c'tor
        public HomeController()
        {
            _srv = new DLPServices();
            //_security = new SecurityServices();
        }
        #endregion c'tor
        #region methods
        private object ConvertModelToEntity(object source)
        {
            switch (source.GetType().ToString())
            {
                //case typeof(DLP.Entities.DlpTls):
                case "DLP.Entities.DlpTls":
                    if (searchVmConfig == null)
                    {
                        searchVmConfig = new AutoMapper.MapperConfiguration(cfg => cfg.CreateMap<Entities.DlpTls, Models.SearchVM>());
                        _searchVmMapper = (Mapper)searchVmConfig.CreateMapper();
                    }
                    return _searchVmMapper.Map<Models.SearchVM>(source);
                //break;
                case "DLP.Entities.V_DlpTlsInfo":
                    if (searchVmConfig == null)
                    {
                        searchVmConfig = new AutoMapper.MapperConfiguration(cfg => cfg.CreateMap<Entities.V_DlpTlsInfo, Models.V_DlpTlsInfo>());
                        _searchVmMapper = (Mapper)searchVmConfig.CreateMapper();
                    }
                    return _searchVmMapper.Map<Models.V_DlpTlsInfo>(source);
                //break;
                default:
                    return null;
                    //break;
            }
        }

        private object ConvertListModelToEntity(object source, string type)
        {
            switch (type)//source.GetType().ToString())
            {
                case "DLP.Entities.V_DlpTlsInfo":
                    if (searchVmConfig == null)
                    {
                        //searchVmConfig = new MapperConfiguration(cfg => cfg.CreateMap<List<Entities.V_DlpTlsInfo>, List<Models.V_DlpTlsInfo>>());
                        searchVmConfig = new MapperConfiguration(cfg => cfg.CreateMap<Entities.V_DlpTlsInfo, Models.V_DlpTlsInfo>());
                        _searchVmMapper = (Mapper)searchVmConfig.CreateMapper();
                    }
                    return _searchVmMapper.Map<List<Models.V_DlpTlsInfo>>(source);
                //break;
                default:
                    return null;
                    //break;
            }
            //List<PersonView> personViews = Mapper.Map<List<Person>, List<PersonView>>(people);
            return null;
        }
        #endregion methods
        #region webmethods
        public ActionResult Index()
        {
            var model = _model; ;
            //model.SomeText = "";
            return View(model);
        }

        [HttpGet]
        public ActionResult Index2()
        {
            return View();
        }


        [HttpPost]
        public ActionResult Index2(Models.SearchDomains model)
        {
            //using System.Security.Claims;
            //System.Security.Principal.NTAccount nt;

            _methodeName = "Index2";
            Log.Logging(LoggingMode.Debug, "Starting {controller}\\{methode}", _controllereName, _methodeName);
            Log.Logging(LoggingMode.Debug, "\t Parameters : Model:{@model}", model);

            if (model.SearchText != null)
            {
                model.isSitePermissionManagment = SecurityServices.isSitePermissionManagment();
                model.EmpCompany = SecurityServices.IsUserInSGY() ? 2 : 1;
                if (model.isSitePermissionManagment)
                {
                    model.EmpCompany += 100;
                }
                Dictionary<string, object> modelParams = new Dictionary<string, object>();
                IEnumerable<Entities.V_DlpTlsInfo> retModel = service.SearchDomains(model.SearchText, model.EmpCompany, out modelParams);
                model.v_DlpTlsInfo = (List<Entities.V_DlpTlsInfo>)retModel;
                //var v_DlpTlsInfo1 = (List<Models.V_DlpTlsInfo>)ConvertListModelToEntity(retModel, "DLP.Entities.V_DlpTlsInfo");
                //model.v_DlpTlsInfo1 = v_DlpTlsInfo1;
                string[] domain = model.SearchText.Split('@');
                if (domain.Length > 1)
                {
                    model.SearchDomain = domain[1];
                    model.IsDomainSearch = true;
                }
                else
                {
                    model.SearchDomain = model.SearchText;
                    model.IsEmailSearch = true;
                }

                if (string.IsNullOrEmpty(model.SearchDomain))
                {
                    model.SearchDomain = modelParams.ContainsKey("SearchDomain") ? (string)modelParams["SearchDomain"] : null;
                }
                model.IsSafetPass = modelParams.ContainsKey("modelParams") ? (bool)modelParams["modelParams"] : false;
                model.IsDomainDlp = modelParams.ContainsKey("IsDomainDlp") ? (bool)modelParams["IsDomainDlp"] : false;
                model.IsEmailDlp = modelParams.ContainsKey("IsEmailDlp") ? (bool)modelParams["IsEmailDlp"] : false;

                if (model.IsDomainDlp)
                {
                    model.RecipientCompany = 5;
                }
                if (model.v_DlpTlsInfo.Count == 0)
                {
                    //Entities.V_DlpTlsInfo dlptls = new Entities.V_DlpTlsInfo();
                    //dlptls.EmailAddress = (domain.Length > 1) ? domain[1] : String.Empty;
                    //model.v_DlpTlsInfo.Add(dlptls);
                }
                //Entities.DlpTls dlpTls = service.SearchDomains(model.SearchText, empCompany, isSitePermissionManagment, out viewName, out reditect);
                //Models.V_DlpTlsInfo mmm = (Models.V_DlpTlsInfo)ConvertModelToEntity(retModel);
                //model.v_DlpTlsInfo = (Models.V_DlpTlsInfo)ConvertModelToEntity(service.SearchDomain(model.SearchText));
                Log.Logging(LoggingMode.Debug, "Exit {controller}\\{methode}", _controllereName, _methodeName);
                return View(model);
            }
            Log.Logging(LoggingMode.Information, "\t Parsing new data for DlpTlsInfo.");
            //service.ParseTlsDataFile();
            Log.Logging(LoggingMode.Debug, "Exit {controller}\\{methode} (no Search Text)", _controllereName, _methodeName);
            return View("Index");
        }

        [HttpPost]
        public ActionResult Index(Entities.DlpTls model)
        {
            _methodeName = "Index";
            Log.Logging(LoggingMode.Debug, "Starting {controller}\\{methode}", _controllereName, _methodeName);
            Log.Logging(LoggingMode.Debug, "\t Parameters : Model:{@model}", model);

            Models.SearchVM retModel = null;

            if (ModelState.IsValid)
            {
                string viewName = string.Empty;
                bool reditect = false;

                if (model.SearchText != null)
                {
                    bool isSitePermissionManagment = SecurityServices.isSitePermissionManagment();
                    int empCompany = SecurityServices.IsUserInSGY() ? 2 : 1;

                    Entities.DlpTls dlpTls = service.SearchDomains(model.SearchText, empCompany, isSitePermissionManagment, out viewName, out reditect);
                    retModel = (Models.SearchVM)ConvertModelToEntity(dlpTls);
                }
                Log.Logging(LoggingMode.Debug, "Exit {controller}\\{methode}", _controllereName, _methodeName);
                if (reditect)
                {
                    return RedirectToAction(viewName);
                }
                else
                {
                    if (string.IsNullOrEmpty(viewName))
                    {
                        return View(retModel);
                    }
                    else
                    {
                        return View(viewName, retModel);
                    }
                }
            }
            else
            {
                return View(retModel);
                //return View("Index", model);
            }
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Search(string searchText)
        {
            _methodeName = "Search";
            Log.Logging(LoggingMode.Debug, "Starting {controller}\\{methode}", _controllereName, _methodeName);
            Log.Logging(LoggingMode.Debug, "\t Parameters : Search Text:{searchText}", searchText);
            ViewBag.Message = "Search the world.";

            Models.SearchDomains model = new SearchDomains();
            //model.SearchText = searchText;
            model.SearchText = searchText;
            if (model.SearchText != null)
            {
                model.isSitePermissionManagment = SecurityServices.isSitePermissionManagment();
                model.EmpCompany = SecurityServices.IsUserInSGY() ? 2 : 1;
                if (model.isSitePermissionManagment)
                {
                    model.EmpCompany += 100;
                }
                Dictionary<string, object> modelParams = new Dictionary<string, object>();
                IEnumerable<Entities.V_DlpTlsInfo> retModel = service.SearchDomains(model.SearchText, model.EmpCompany, out modelParams);
                model.v_DlpTlsInfo = (List<Entities.V_DlpTlsInfo>)retModel;
                string[] domain = model.SearchText.Split('@');
                if (domain.Length > 1)
                {
                    model.SearchDomain = domain[1];
                    model.IsDomainSearch = true;
                }
                else
                {
                    model.SearchDomain = model.SearchText;
                    model.IsEmailSearch = true;
                }

                if (string.IsNullOrEmpty(model.SearchDomain))
                {
                    model.SearchDomain = modelParams.ContainsKey("SearchDomain") ? (string)modelParams["SearchDomain"] : null;
                }
                model.IsSafetPass = modelParams.ContainsKey("modelParams") ? (bool)modelParams["modelParams"] : false;
                model.IsDomainDlp = modelParams.ContainsKey("IsDomainDlp") ? (bool)modelParams["IsDomainDlp"] : false;
                model.IsEmailDlp = modelParams.ContainsKey("IsEmailDlp") ? (bool)modelParams["IsEmailDlp"] : false;

                if (model.IsDomainDlp)
                {
                    model.RecipientCompany = 5;
                }
                //Log.Logging(LoggingMode.Debug, "Exit {controller}\\{methode}", _controllereName, _methodeName);
                //return View(model);
            }
            Log.Logging(LoggingMode.Information, "\t Parsing new data for DlpTlsInfo.");
            service.ParseTlsDataFile();
            Log.Logging(LoggingMode.Debug, "Exit {controller}\\{methode} ", _controllereName, _methodeName);
            //RedirectToAction("Index2", model);
            return View("Index2", model);
        }
        #endregion webmethods
    }
}