using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace xevents_helper.Models
{
    public class TargetParameter
    {
        public string Name { get; set; }
        public string TypeName { get; set; }
        public bool IsMandatory { get; set; }
        public string Description { get; set; }
        public XeDataType DataType { get; set; }
    }
}