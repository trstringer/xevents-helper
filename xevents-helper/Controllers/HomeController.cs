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

        public JsonResult GetEventDescription(string releaseName, string eventName)
        {
            DataGatherer dataGatherer = new DataGatherer();

            string eventDescription = dataGatherer.GetEventDescription(releaseName, eventName);

            return Json(new { eventName = eventName, eventDescription = eventDescription }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAllEventsForRelease(string releaseName)
        {
            DataGatherer dataGatherer = new DataGatherer();

            IEnumerable<XeEvent> events = dataGatherer.GetAllEventsForRelease(dataGatherer.GetRelease(releaseName));

            return Json(events, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SearchEvents(string releaseName, string searchTerm, bool searchDescriptions)
        {
            DataGatherer dataGatherer = new DataGatherer();
            
            IEnumerable<XeEvent> events = dataGatherer.SearchEvents(
                dataGatherer.GetRelease(releaseName), 
                searchTerm, 
                searchDescriptions ? SearchOption.ByNameAndDescription : SearchOption.ByName);

            return Json(events, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCreateSessionDdl(XeSession session)
        {
            XeUtility xeUtil = new XeUtility();
            string createSessionDdl = xeUtil.GetCreateDdl(session);

            return Json(createSessionDdl, JsonRequestBehavior.AllowGet);
        }
    }
}