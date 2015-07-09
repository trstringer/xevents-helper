using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace xevents_helper.Models
{
    public class TargetSetting
    {
        public Target Target { get; set; }
        public TargetParameter Parameter { get; set; }
        public object Setting { get; set; }
    }
}