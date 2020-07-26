using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Common
{
    public static class AppConfigurations
    {
        public static string LogFolder
        {
            get
            {
                var logPath = ConfigurationManager.AppSettings["LogFolder"];
                if (string.IsNullOrWhiteSpace(logPath)) throw new NullReferenceException("you must define log folder");
                return logPath;
            }
        }
        public static string Issuer
        {
            get
            {
                var issuer = ConfigurationManager.AppSettings["Issuer"];
                if (string.IsNullOrWhiteSpace(issuer)) throw new NullReferenceException("you must define issure");
                return issuer;
            }
        }
    }
}
