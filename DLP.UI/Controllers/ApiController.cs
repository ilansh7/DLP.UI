using DLP.Common;
using DLP.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DLP.UI.Controllers
{
    public class Wrapper
    {
        [JsonProperty("tags")]
        public List<string> Tags { get; set; }
    }

    [Route("api/[controller]")]
    //[ApiController]
    public class ApiController : Controller
    {
        #region variables
        //public Entities.DlpTls _model;
        private static DLPServices _srv;
        //private SecurityServices _security;
        //private static AutoMapper.MapperConfiguration _searchVmConfig;
        //private static AutoMapper.Mapper _searchVmMapper;
        private static string _controllereName = "Api";
        private static string _methodeName = "";
        #endregion variables
        #region propertirs
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
        public ApiController()
        {
            _srv = new DLPServices();
            //_security = new SecurityServices();
        }
        #endregion c'tor
        #region methods

        //[Produces("application/json")]
        //[HttpGet]
        public JsonResult Search()
        {
            _methodeName = "Search";
            Log.Logging(LoggingMode.Debug, "Starting {controller}\\{methode}", _controllereName, _methodeName);
            //Log.Logging(LoggingMode.Debug, "\t Parameters : Model:{@model}", model);

            try
            {

                //string term = HttpContext.Request.Query["term"].ToString();
                string term = HttpContext.Request.QueryString.ToString();
                var searchTerm = term.Split('=');

                var ret = service.GetDomainsAndMails(searchTerm[1]);
                var obj = new Wrapper() { Tags = ret };
                var json = JsonConvert.SerializeObject(ret);
                //return Json(json);
                JsonResult js = new JsonResult();
                js.Data = json;
                //return js;
                return Json(ret, JsonRequestBehavior.AllowGet);
                //var username = db.DLP.Where(p => p.EmailAddress.Contains(term)).Select(p => p.EmailAddress).ToList();
                //return Ok(username);
            }
            catch (Exception ex)
            {
                Log.Logging(LoggingMode.Error, "\t Exception on {controller}\\{methode} : {@ex}", _controllereName, _methodeName, ex);
                //return BadRequest();
                return Json(new List<string>() { " " });
            }
        }

        //}
        // GET: Api
        //public ActionResult Index()
        //{
        //    return View();
        //}

        // GET: Api/Details/5
        //public ActionResult Details(int id)
        //{
        //    return View();
        //}

        // GET: Api/Create
        //public ActionResult Create()
        //{
        //    return View();
        //}

        // POST: Api/Create
        //[HttpPost]
        //public ActionResult Create(FormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add insert logic here

        //        return RedirectToAction("Index");
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        // GET: Api/Edit/5
        //public ActionResult Edit(int id)
        //{
        //    return View();
        //}

        // POST: Api/Edit/5
        //[HttpPost]
        //public ActionResult Edit(int id, FormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add update logic here

        //        return RedirectToAction("Index");
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        // GET: Api/Delete/5
        //public ActionResult Delete(int id)
        //{
        //    return View();
        //}

        // POST: Api/Delete/5
        //[HttpPost]
        //public ActionResult Delete(int id, FormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add delete logic here

        //        return RedirectToAction("Index");
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}
        #endregion methods
    }
}
