using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace xevents_helper.Models
{
    public class XeSession
    {
        public string Name { get; set; }
        public IEnumerable<XeEvent> Events { get; set; }
        public IEnumerable<XeTarget> Targets { get; set; }
    }
}