using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace xevents_helper.Models
{
    public class Event
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string PackageName { get; set; }
        public IEnumerable<EventField> Fields { get; set; }
        public IEnumerable<Action> Actions { get; set; }
        public IEnumerable<Predicate> Predicates { get; set; }
    }
}