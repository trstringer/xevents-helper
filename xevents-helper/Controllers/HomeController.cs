using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using xevents_helper.ViewModels;
using xevents_helper.Models;

namespace xevents_helper.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            DataGatherer dataGatherer = new DataGatherer();
            HelperViewModel viewModel = new HelperViewModel();
            viewModel.Releases = new SelectList(dataGatherer.GetAllReleases(), "Name", "Name");

            return View(viewModel);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}