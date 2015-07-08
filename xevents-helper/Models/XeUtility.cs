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
        private string GetStartEventSessionClause(Session session)
        {
            return
                string.Format("ALTER EVENT SESSION {0}\r\nON SERVER\r\nSTATE = START;\r\nGO", QuoteName(session.Name));
        }
        private string GetStopEventSessionClause(Session session)
        {
            return
                string.Format("ALTER EVENT SESSION {0}\r\nON SERVER\r\nSTATE = STOP;\r\nGO", QuoteName(session.Name));
        }
        private string GetDropEventSessionClause(Session session)
        {
            return
                string.Format("DROP EVENT SESSION {0}\r\nON SERVER;\r\nGO", QuoteName(session.Name));
        }

        private string GetAddEventClause(Event xeEvent)
        {
            string addEventClause;

            addEventClause = string.Format("ADD EVENT {0}.{1}", QuoteName(xeEvent.PackageName), QuoteName(xeEvent.Name));
            addEventClause += "\r\n(\r\n";
            addEventClause += GetEventActionClause(xeEvent.Actions);
            addEventClause += GetEventWhereClause(xeEvent.Predicates);
            
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
        private string GetEventWhereClause(IEnumerable<Predicate> predicates)
        {
            if (predicates == null || predicates.Count() == 0)
                return "";

            string whereClause = "WHERE\r\n(";
            string newWhere;

            bool isFirst = true;
            foreach (Predicate predicate in predicates)
            {
                newWhere = string.Format(
                    "\r\n{0} = {1}",
                    GetEventDataFullName(predicate.EventData),
                    FormatEventDataComparisonData(predicate));

                if (isFirst)
                    isFirst = false;
                else
                {
                    // append the conditional operator, as as this point 
                    // we are no longer on the first element and we need 
                    // to handle conditional AND or OR
                    //
                    string conditionalOperatorString;
                    if (predicate.ConditionalOperator == ConditionalOperator.And)
                        conditionalOperatorString = "AND";
                    else if (predicate.ConditionalOperator == ConditionalOperator.Or)
                        conditionalOperatorString = "OR";
                    else
                        throw new NotImplementedException("Only AND and OR have been implemented for predicates");

                    newWhere = string.Format("{0} {1}", conditionalOperatorString, newWhere);
                }

                whereClause += newWhere;
            }

            return whereClause;
        }
        private string GetEventDataFullName(IEventData eventData)
        {
            // if the event data is an action then we need to show this in 
            // the form of package_name.action_name, otherwise it must be 
            // an event field in which case the field name is sufficient
            //
            if (eventData is IAction)
                return string.Format("{0}.{1}", ((IAction)eventData).PackageName, eventData.Name);
            else
                return eventData.Name;
        }
        private string FormatEventDataComparisonData(Predicate predicate)
        {
            return 
                predicate.EventData.DataType == EventDataType.Character ? 
                string.Format("N'{0}'", predicate.ComparisonValue) :
                predicate.ComparisonValue.ToString();
        }

        private string QuoteName(string name)
        {
            return string.Format("[{0}]", name);
        }
    }
}