using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace xevents_helper.Models
{
    public class XeUtility
    {
        public string GetCreateDdl(Session session)
        {
            string sessionDefinition = GetCreateEventSessionClause(session);

            sessionDefinition += ";\r\nGO";

            return sessionDefinition;
        }

        private string GetCreateEventSessionClause(Session session)
        {
            return string.Format("CREATE EVENT SESSION {0}\r\nON SERVER", QuoteName(session.Name));
        }

        private string GetAddEventClause(Event xeEvent)
        {
            string addEventClause = string.Format("ADD EVENT {0}.{1}", QuoteName(xeEvent.PackageName), QuoteName(xeEvent.Name));
            
            return addEventClause;
        }

        private string QuoteName(string name)
        {
            return string.Format("[{0}]", name);
        }
    }
}