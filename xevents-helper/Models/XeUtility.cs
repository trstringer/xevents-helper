using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace xevents_helper.Models
{
    public class XeUtility
    {
        public string GetCreateDdl(XeSession session)
        {
            string sessionDefinition = GetCreateEventSessionClause(session.Name);

            sessionDefinition += ";\r\nGO";

            return sessionDefinition;
        }

        private string GetCreateEventSessionClause(string sessionName)
        {
            return string.Format("CREATE EVENT SESSION {0}\r\nON SERVER", QuoteName(sessionName));
        }

        private string QuoteName(string name)
        {
            return string.Format("[{0}]", name);
        }
    }
}