using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using xevents_helper.Models;
using System.Collections.Generic;
using System.Diagnostics;

namespace xevents_helper.test
{
    [TestClass]
    public class DdlCreation
    {
        private Release _release;
        private XeUtility _xeUtility;
        private DataGatherer _dataGatherer;

        private string _releaseName;
        private string _eventName1;
        private string _filterData1;
        private string _eventName2;
        private string _targetName1;
        private string _targetName2;

        public DdlCreation()
        {
            _releaseName = "SQL 2012";
            _release = new Release() { Name = _releaseName };
            _xeUtility = new XeUtility();
            _dataGatherer = new DataGatherer();

            _eventName1 = "sql_statement_completed";
            _eventName2 = "sql_statement_starting";

            _targetName1 = "event_file";
            _targetName2 = "";
        }

        [TestMethod]
        public void GenerateEventDefinitionSingleEvent()
        {
            Session session = new Session();
            session.Name = "Session1";
            session.TargetRelease = _release;

            List<Event> events = new List<Event>();
            events.Add(_dataGatherer.SearchEvents(_release, _eventName1, SearchOption.ByName).First());
            session.Events = events;
            
            string sessionDefinition = _xeUtility.GetCreateDdl(session);

            Assert.IsFalse(string.IsNullOrWhiteSpace(sessionDefinition));

            Debug.WriteLine(sessionDefinition);
        }
    }
}
