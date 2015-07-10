using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using xevents_helper;
using xevents_helper.Models;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;

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

            Debug.WriteLine(string.Format("{0} events found in release {1}", events.Count(), _testRelease.Name));
        }

        [TestMethod]
        public void SearchFullEventName()
        {
            IEnumerable<Event> events = _dataGatherer.SearchEvents(_testRelease, _testSearchEventName, SearchOption.ByName);

            Assert.IsNotNull(events);
            Assert.IsTrue(events.Count() == 1);
        }

        [TestMethod]
        public void SearchPartialEventName()
        {
            IEnumerable<Event> events = _dataGatherer.SearchEvents(_testRelease, _testSearchEventName.Substring(0, _testSearchEventName.Length - 1), SearchOption.ByName);

            Assert.IsNotNull(events);
            Assert.IsTrue(events.Count() == 1);
        }

        [TestMethod]
        public void ActionsRetrieval()
        {
            IEnumerable<xevents_helper.Models.Action> actions = _dataGatherer.GetAllActions(_testRelease);

            Assert.IsNotNull(actions);
            Assert.IsTrue(actions.Count() > 0);

            Debug.WriteLine(string.Format("{0} actions found in release {1}", actions.Count(), _testRelease.Name));
        }

        [TestMethod]
        public void GetAllReleases()
        {
            IEnumerable<Release> releases = _dataGatherer.GetAllReleases();

            Assert.IsNotNull(releases);
            Assert.IsTrue(releases.Count() > 0);

            Debug.WriteLine(string.Format("{0} releases found", releases.Count()));
        }

        [TestMethod]
        public void GetRelease()
        {
            Release release = _dataGatherer.GetRelease(_testReleaseName);

            Assert.IsNotNull(release);
            Assert.IsTrue(release.Name.ToUpper() == _testReleaseName);
        }

        [TestMethod]
        public void GetDescriptionFromEvent()
        {
            Event xeEvent = _dataGatherer.SearchEvents(_testRelease, _testSearchEventName, SearchOption.ByName).First();
            Assert.IsFalse(string.IsNullOrWhiteSpace(xeEvent.Description));

            string description = _dataGatherer.GetEventDescription(_testRelease, _testSearchEventName);
            Assert.IsFalse(string.IsNullOrWhiteSpace(description));

            Assert.AreEqual(description, xeEvent.Description);
        }
    }
}
