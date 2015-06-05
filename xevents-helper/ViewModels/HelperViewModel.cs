using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using xevents_helper.Models;

namespace xevents_helper.ViewModels
{
    public class HelperViewModel
    {
        public IEnumerable<SelectListItem> Releases { get; set; }
        public string SelectedRelease { get; set; }
        public string SessionName { get; set; }
    }
}