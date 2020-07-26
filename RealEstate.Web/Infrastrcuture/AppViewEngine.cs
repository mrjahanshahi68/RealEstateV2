using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RealEstate.Web.Infrastrcuture
{
    public class AppViewEngine : RazorViewEngine
    {
        public AppViewEngine()
        {
            ViewLocationFormats = new[]
            {
                "~/ClientApp/Views/{1}/{0}.cshtml",
                "~/ClientApp/Views/Shared/{0}.cshtml",
                "~/ClientApp/Views/{1}/{0}.vbhtml",
                "~/ClientApp/Views/Shared/{0}.vbhtml"
            };
            PartialViewLocationFormats = new[]
            {
                "~/ClientApp/Views/{1}/{0}.cshtml",
                "~/ClientApp/Views/Shared/{0}.cshtml",
                "~/ClientApp/Views/{1}/{0}.vbhtml",
                "~/ClientApp/Views/Shared/{0}.vbhtml"
            };
            MasterLocationFormats = new[]
             {
             "~/ClientApp/Views/{1}/{0}.cshtml",
             "~/ClientApp/Views/Shared/{0}.cshtml",
             "~/ClientApp/Views/{1}/{0}.vbhtml",
             "~/ClientApp/Views/Shared/{0}.vbhtml"
             };
            AreaViewLocationFormats = new[]
             {
             "~/ClientApp/Areas/{2}/Views/{1}/{0}.cshtml",
             "~/ClientApp/Areas/{2}/Views/Shared/{0}.cshtml",
             "~/ClientApp/Areas/{2}/Views/{1}/{0}.vbhtml",
             "~/ClientApp/Areas/{2}/Views/Shared/{0}.vbhtml"
             };
            AreaMasterLocationFormats = new[]
             {
             "~/ClientApp/Areas/{2}/Views/{1}/{0}.cshtml",
             "~/ClientApp/Areas/{2}/Views/Shared/{0}.cshtml",
             "~/ClientApp/Areas/{2}/Views/{1}/{0}.vbhtml",
             "~/ClientApp/Areas/{2}/Views/Shared/{0}.vbhtml"
             };
            AreaPartialViewLocationFormats = new[]
             {
             "~/ClientApp/Areas/{2}/Views/{1}/{0}.cshtml",
             "~/ClientApp/Areas/{2}/Views/Shared/{0}.cshtml",
              "~/ClientApp/Areas/{2}/Views/{1}/{0}.vbhtml",
             "~/ClientApp/Areas/{2}/Views/Shared/{0}.vbhtml"
             };
        }
        protected override IView CreatePartialView(ControllerContext controllerContext, string partialPath)
        {
            return base.CreatePartialView(controllerContext, partialPath);
        }
        protected override IView CreateView(ControllerContext controllerContext, string viewPath, string masterPath)
        {
            return base.CreateView(controllerContext, viewPath, masterPath);
        }
        public override ViewEngineResult FindPartialView(ControllerContext controllerContext, string partialViewName, bool useCache)
        {
            return base.FindPartialView(controllerContext, partialViewName, useCache);
        }
        public override ViewEngineResult FindView(ControllerContext controllerContext, string viewName, string masterName, bool useCache)
        {
            return base.FindView(controllerContext, viewName, masterName, useCache);
        }
        protected override bool FileExists(ControllerContext controllerContext, string virtualPath)
        {
            return base.FileExists(controllerContext, virtualPath);
        }
        
    }
}