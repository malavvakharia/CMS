using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CampaignManagementSystem.Controllers
{
    public class DashboardController : Controller
    {
        // GET: Dashboard
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AddCampaign()
        {
            return View();
        }

        public ActionResult EditCampaign()
        {
            return View();
        }

        public ActionResult ViewReports()
        {
            return View();
        }

        public ActionResult QuickCampaign()
        {
            return View();
        }

        public ActionResult UploadSheet()
        {
            return View();
        }
    }
}