using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IcasDrive.Models;

namespace IcasDrive.Controllers
{
    public class SelectionController : ControllerBase
    {
        // GET: Selection
        public ActionResult Index()
        {
            return View();
        }
    }
}