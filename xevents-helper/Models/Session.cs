using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace xevents_helper.Models
{
    public class Session
    {
        public string Name { get; set; }
        public IEnumerable<Event> Events { get; set; }
        public IEnumerable<Target> Targets { get; set; }
        public Release TargetRelease { get; set; }
    }
}