using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Media.Models;

namespace Media.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View(new DataModel());
        }
    }
}