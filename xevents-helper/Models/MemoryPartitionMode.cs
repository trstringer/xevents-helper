using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace xevents_helper.Models
{
    public enum MemoryPartitionMode
    {
        NotSpecified,
        NONE,
        PER_NODE,
        PER_CPU
    }
}