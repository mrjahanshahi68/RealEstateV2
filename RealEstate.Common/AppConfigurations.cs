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
		public static string AttachmentFolder
		{
			get
			{
				var attachmentFolder = ConfigurationManager.AppSettings["AttachmentFolder"];
				if (string.IsNullOrWhiteSpace(attachmentFolder))
					throw new NullReferenceException("مسیر پوشه ضمیمه ها در فایل وب کانفیگ مشخص نشده است");
				return attachmentFolder;
			}
		}
		public static string PropertyImageFolder
		{
			get
			{
				var propertyImageFolder = ConfigurationManager.AppSettings["PropertyImageFolder"];
				if (string.IsNullOrWhiteSpace(propertyImageFolder))
					throw new NullReferenceException("مسیر پوشه تصاویر ملک در فایل وب کانفیگ مشخص نشده است");
				return propertyImageFolder;
			}
		}
		public static string BlogImageFolder
		{
			get
			{
				var blogImageFolder = ConfigurationManager.AppSettings["BlogImageFolder"];
				if (string.IsNullOrWhiteSpace(blogImageFolder))
					throw new NullReferenceException("مسیر پوشه تصاویر بلاگ در فایل وب کانفیگ مشخص نشده است");
				return blogImageFolder;
			}
		}
	}
}
