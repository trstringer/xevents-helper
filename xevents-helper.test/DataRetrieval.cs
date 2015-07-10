using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using xevents_helper;
using xevents_helper.Models;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace xevents_helper.test
{
    [TestClass]
    public class DataRetrieval
    {
        private DataGatherer _dataGatherer;
        private string _testReleaseName;
        private Release _testRelease;
        private string _testSearchEventName;

        public DataRetrieval()
        {
            _dataGatherer = new DataGatherer();
            _testReleaseName = "SQL 2012";
            _testRelease = new Release() { Name = _testReleaseName };
            _testSearchEventName = "sql_statement_completed";
        }

        [TestMethod]
        public void DataGathererInstantiation()
        {
            DataGatherer dg = new DataGatherer();

            Assert.IsNotNull(dg);
        }

        [TestMethod]
        public void EventsRetrieval()
        {
            IEnumerable<Event> events = _dataGatherer.GetAllEventsForRelease(_testRelease);

            Assert.IsNotNull(events);
            Assert.IsTrue(events.ToList().Count > 0);
        }

        [TestMethod]
        public void SearchFullEventName()
        {
            IEnumerable<Event> events = _dataGatherer.SearchEvents(_testRelease, _testSearchEventName, SearchOption.ByName);

            Assert.IsNotNull(events);
            Assert.IsTrue(events.Count() == 1);
        }
    }
}
