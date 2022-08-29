using Serilog;
using Serilog.Core;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DLP.Common
{
    #region enums
    public enum LoggingMode
    {
        Debug,
        Information,
        Warning,
        Error,
        Fatal
    }
    public enum Environment
    {
        Dev,
        Test,
        Prod
    }
    #endregion enums
    public static class Log
    {
        #region variables
        private static Logger _logger;
        private static string _environment;
        #endregion variables
        #region constractor
        static Log()
        {
            _environment = ConfigurationManager.AppSettings["Environment"].ToString();
            _logger = new LoggerConfiguration()
                            // add console as logging target
                            .WriteTo.Console()
                            // add a logging target for warnings and higher severity  logs
                            // structured in JSON format
                            //.WriteTo.File(new JsonFormatter(),
                            //              @"D:\Logs\MVS.DLP\important.json",
                            //              restrictedToMinimumLevel: LogEventLevel.Warning)
                            // add a rolling file for all logs
                            .WriteTo.File(@"C:\Logs\MVS.DLP\mvsdlp.log",
                                          rollingInterval: RollingInterval.Day)
                            //rollOnFileSizeLimit: true, 
                            //fileSizeLimitBytes: 123456)
                            // set default minimum level
                            .MinimumLevel.Debug()
                            .CreateLogger();
        }
        #endregion constractor
        #region methods
        public static void Logging(LoggingMode mode, string msg, object data, object obj1, object obj2)
        {
            //_logger.Debug("\t Parameters : Model:{@model}", model);
            switch (mode)
            {
                case LoggingMode.Debug:
                    if (_environment != Enum.GetName(typeof(Environment), Environment.Prod))
                    {
                        _logger.Debug(msg, data, obj1, obj2);
                    }
                    break;
                case LoggingMode.Information:
                    _logger.Information(msg, data, obj1, obj2);
                    break;
                case LoggingMode.Warning:
                    _logger.Warning(msg, data, obj1, obj2);
                    break;
                case LoggingMode.Error:
                    _logger.Error(msg, data, obj1, obj2);
                    break;
                case LoggingMode.Fatal:
                    _logger.Fatal(msg, data, obj1, obj2);
                    break;
                default:
                    _logger.Warning(msg, data, obj1, obj2);
                    break;
            }
        }

        public static void Logging(LoggingMode mode, string msg, object data, object obj)
        {
            Logging(mode, msg, data, obj, null);
        }

        public static void Logging(LoggingMode mode, string msg, object data)
        {
            Logging(mode, msg, data, null, null);
        }

        public static void Logging(LoggingMode mode, string msg)
        {
            Logging(mode, msg, null, null, null);
        }
        #endregion methods
    }
}
