using IcasDrive.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Web.Mvc;

namespace IcasDrive.Controllers
{
    public class ControllerBase : Controller
    {
        public ControllerBase()
        {
            HttpDataProvider = new HttpDataProvider(ConfigurationManager.AppSettings["ExamApiBaseUrl"]);
            ApplicationName = ConfigurationManager.AppSettings["ApplicationName"];
        }

        internal static HttpDataProvider HttpDataProvider { get; set; }

        internal static string ApplicationName { get; set; }
    }
}