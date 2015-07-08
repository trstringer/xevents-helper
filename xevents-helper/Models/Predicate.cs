using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace xevents_helper.Models
{
    public class Predicate
    {
        public IEventData EventData { get; set; }
        public ConditionalOperator ConditionalOperator { get; set; }
    }
}