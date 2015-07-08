using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xevents_helper.Models
{
    public interface IAction : IEventData
    {
        string PackageName { get; set; }
    }
}
