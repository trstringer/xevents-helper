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
            string addEventClause;

            addEventClause = string.Format("ADD EVENT {0}.{1}", QuoteName(xeEvent.PackageName), QuoteName(xeEvent.Name));
            addEventClause += "\r\n(\r\n";
            addEventClause += GetEventActionClause(xeEvent.Actions);
            
            return addEventClause;
        }
        private string GetEventActionClause(IEnumerable<Action> actions)
        {
            if (actions == null || actions.Count() == 0)
                return "";

            string actionClause = "ACTION\r\n(";
            string newAction;

            bool isFirst = true;
            foreach (Action action in actions)
            {
                newAction = string.Format("\r\n{0}.{1}", action.PackageName, action.Name);

                if (isFirst)
                    isFirst = false;
                else
                    newAction = "," + newAction;

                actionClause += newAction;
            }

            actionClause += "\r\n)";

            return actionClause;
        }

        private string QuoteName(string name)
        {
            return string.Format("[{0}]", name);
        }
    }
}