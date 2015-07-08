using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace xevents_helper.Models
{
    public class Action : IAction
    {
        public string Name { get; set; }
        public string PackageName { get; set; }
        public EventDataType DataType { get; set; }
    }
}