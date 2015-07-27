using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace xevents_helper.Models
{
    public class SessionOptions
    {
        public int? MaxMemory { get; set; }
        public EventRetentionMode EventRetentionMode { get; set; }
        public int? MaxDispatchLatency { get; set; }
        public int? MaxEventSize { get; set; }
        public MemoryPartitionMode MemoryPartitionMode { get; set; }
        public bool? TrackCausality { get; set; }
        public bool? StartWithInstance { get; set; }
    }
}