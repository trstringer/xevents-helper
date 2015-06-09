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

            IEnumerable<Release> allReleases = dataGatherer.GetAllReleases();
            viewModel.Releases = new SelectList(allReleases, "Name", "Name");

            IEnumerable<XeEvent> allEventsForRelease = dataGatherer.GetAllEventsForRelease(allReleases.First());
            viewModel.Events = new SelectList(allEventsForRelease, "Name", "Name");

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