using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xevents_helper.Models
{
    public interface IEventData
    {
        string Name { get; set; }
        XeDataType DataType { get; set; }
    }
}
