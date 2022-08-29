using DLP.Services;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DLP.UI.Controllers
{
    public class ReportsController : Controller
    {
        #region variables
        private DLPServices _srv;
        private static AutoMapper.MapperConfiguration _searchVmConfig;
        private static AutoMapper.Mapper _searchVmMapper;
        #endregion variables
        #region propertirs
        //public DLPServices service1
        //{
        //    get
        //    {
        //        if (_srv == null)
        //        {
        //            return _srv = new DLPServices();
        //        }
        //        else
        //        {
        //            return _srv;
        //        }
        //    }
        //}
        public static AutoMapper.MapperConfiguration searchVmConfig
        {
            get { return _searchVmConfig; }
            set { _searchVmConfig = value; }
        }
        #endregion propertirs
        #region c'tor
        public ReportsController()
        {
            _srv = new DLPServices();
        }
        #endregion c'tor
        #region methods
        //public void InitReportsController()
        //{
        //    _srv = new DLPServices();
        //}
        private object ConvertModelToEntity(object source)
        {
            switch (source.GetType().ToString())
            {
                //case typeof(DLP.Entities.DlpTls):
                case "DLP.Entities.DlpTls":
                    if (searchVmConfig == null)
                    {
                        searchVmConfig = new MapperConfiguration(cfg => cfg.CreateMap<Entities.DlpTls, Models.SearchVM>());
                        _searchVmMapper = (Mapper)searchVmConfig.CreateMapper();
                    }
                    return _searchVmMapper.Map<Models.SearchVM>(source);
                //break;
                default:
                    return null;
                    //break;
            }
        }

        #endregion methods
        #region webmethods

        // GET: Reports
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Index1(Models.V_DlpTlsInfo model)
        {
            if (ModelState.IsValid)
            {
                if (!string.IsNullOrEmpty(model.SearchText))
                {
                    DLPServices service = new DLPServices();
                    //DLP.Entities.V_DlpTlsInfo
                    //var enumerable = new List<Entities.V_DlpTlsInfo>();
                    var enumerable = new[] { model };
                    //int emailType = 1;
                    //service.SearchDomains(emailType);
                    //return View(enumerable);
                    Entities.V_DlpTlsInfo retModel = service.SearchDomain(model.SearchText);
                    //(Models.SearchVM)ConvertModelToEntity(dlpTls);
                    return View("Index", service.SearchDomain(model.SearchText));
                }
                else
                {
                    //return View("Index");
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                return View("Index");
                //return View("Index", model);
            }
        }

        [HttpGet]
        public ActionResult ExcludeDomains()
        {
            if (ModelState.IsValid)
            {
                int emailType = 1;
                //InitReportsController();  /// להפוך לעצמאי
                DLPServices service = new DLPServices();
                //model = _srv.ExcludeDomainsReport(emailType);
                //model.SomeText = "Hello World";
                //TlsDlpData
                //ViewData.Model = model;
                //return RedirectToAction("Index1", model);
                //_model = model;
                return View("Index", service.ExcludeDomainsReport(emailType));
            }
            else
            {
                return View();
                //return View("Index", model);
            }
        }
        #endregion methods
    }
}