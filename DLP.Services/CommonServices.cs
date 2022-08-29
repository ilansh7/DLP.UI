using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DLP.Services
{

    public static class CommonServices
    {
        #region variables
        //public static Entities.DlpTls _model;
        //private static DLPServices _srv;
        //private SecurityServices _security;
        private static AutoMapper.MapperConfiguration _searchVmConfig;
        private static AutoMapper.Mapper _searchVmMapper;
        //private static string _controllereName = "Home";
        //private static string _methodeName = "";
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
        #endregion propertirs
        #region c'tor
        //public CommonServices()
        //{
        //    //_srv = new DLPServices();
        //    //_security = new SecurityServices();
        //}
        #endregion c'tor
        #region methods
        //public static object ConvertModelToEntity(object source)
        //{
            //switch (source.GetType().ToString())
            //{
            //    //case typeof(DLP.Entities.DlpTls):
            //    case "DLP.Entities.DlpTls":
            //        if (searchVmConfig == null)
            //        {
            //            searchVmConfig = new AutoMapper.MapperConfiguration(cfg => cfg.CreateMap<Entities.DlpTls, Models.SearchVM>());
            //            _searchVmMapper = (Mapper)searchVmConfig.CreateMapper();
            //        }
            //        return null;// _searchVmMapper.Map<Models.SearchVM>(source);
            //    //break;
            //    case "DLP.Entities.V_DlpTlsInfo":
            //        if (searchVmConfig == null)
            //        {
            //            searchVmConfig = new AutoMapper.MapperConfiguration(cfg => cfg.CreateMap<Entities.V_DlpTlsInfo, Models.V_DlpTlsInfo>());
            //            _searchVmMapper = (Mapper)searchVmConfig.CreateMapper();
            //        }
            //        return null;// _searchVmMapper.Map<Models.V_DlpTlsInfo>(source);
            //    //break;
            //    default:
            //        return null;
            //        //break;
            //}
        //}

        //private static object ConvertListModelToEntity(object source, string type)
        //{
            //switch (type)//source.GetType().ToString())
            //{
            //    case "DLP.Entities.V_DlpTlsInfo":
            //        if (searchVmConfig == null)
            //        {
            //            //searchVmConfig = new MapperConfiguration(cfg => cfg.CreateMap<List<Entities.V_DlpTlsInfo>, List<Models.V_DlpTlsInfo>>());
            //            searchVmConfig = new MapperConfiguration(cfg => cfg.CreateMap<Entities.V_DlpTlsInfo, Models.V_DlpTlsInfo>());
            //            _searchVmMapper = (Mapper)searchVmConfig.CreateMapper();
            //        }
            //        return null;// _searchVmMapper.Map<List<Models.V_DlpTlsInfo>>(source);
            //    //break;
            //    default:
            //        return null;
            //        //break;
            //}
            ////List<PersonView> personViews = Mapper.Map<List<Person>, List<PersonView>>(people);
            //return null;
        //}
        #endregion methods
    }
}
