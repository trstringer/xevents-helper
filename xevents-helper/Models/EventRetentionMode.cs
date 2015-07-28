using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace xevents_helper.Models
{
    public enum EventRetentionMode
    {
        NotSpecified,
        ALLOW_SINGLE_EVENT_LOSS,
        ALLOW_MULTIPLE_EVENT_LOSS,
        NO_EVENT_LOSS
    }
}