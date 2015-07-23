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
        private XeUtility _xeUtility;
        private string _testReleaseName;
        private Release _testRelease;
        private string _testSearchEventName;
        private string _testSearchEventNameMultiple;
        private string _testTargetName;
        private string _testTargetMandatoryParamName;
        private string _testActionName;

        public DataRetrieval()
        {
            _dataGatherer = new DataGatherer();
            _xeUtility = new XeUtility();
            _testReleaseName = "SQL 2012";
            _testRelease = new Release() { Name = _testReleaseName };
            _testSearchEventName = "sql_statement_completed";
            _testSearchEventNameMultiple = "sql";
            _testTargetName = "event_file";
            _testTargetMandatoryParamName = "filename";
            _testActionName = "sql_text";
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
        public void SearchPartialEventNameMultiple()
        {
            IEnumerable<Event> events = _dataGatherer.SearchEvents(_testRelease, _testSearchEventNameMultiple, SearchOption.ByName);

            Assert.IsNotNull(events);
            Assert.IsTrue(events.Count() > 1);

            Debug.WriteLine(string.Format("{0} events found for \"{1}\"", events.Count(), _testSearchEventNameMultiple));
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

        [TestMethod]
        public void GetAllTargets()
        {
            IEnumerable<Target> targets = _dataGatherer.GetAllTargets(_testRelease);

            Assert.IsNotNull(targets);
            Assert.IsTrue(targets.Count() > 1);

            Debug.WriteLine(string.Format("Found {0} targets on release {1}", targets.Count(), _testRelease.Name));
        }

        [TestMethod]
        public void GetTarget()
        {
            Target target = _dataGatherer.GetTarget(_testRelease, _testTargetName);

            Assert.IsNotNull(target);
            Assert.AreEqual(_testTargetName, target.Name);
        }

        [TestMethod]
        public void GetTargetParams()
        {
            IEnumerable<TargetParameter> tParams = _dataGatherer.GetTargetParameters(_testRelease, _testTargetName);

            Assert.IsNotNull(tParams);
            Assert.IsTrue(tParams.Count() > 1);

            Debug.WriteLine(string.Format("{0} target params found for {1} in release {2}", tParams.Count(), _testTargetName, _testRelease.Name));
        }

        [TestMethod]
        public void GetActionFound()
        {
            xevents_helper.Models.Action action = _dataGatherer.GetAction(_testRelease, _testActionName);

            Assert.IsNotNull(action);
        }

        [TestMethod]
        public void GetActionNotFound()
        {
            xevents_helper.Models.Action action = _dataGatherer.GetAction(_testRelease, _testActionName + "blah");

            Assert.IsNull(action);
        }

        [TestMethod]
        public void TargetMandatoryParamFailure()
        {
            Target target = _dataGatherer.GetTarget(_testRelease, _testTargetName);

            Assert.IsNotNull(target);

            Assert.IsFalse(_xeUtility.AllMandatorySettingsDefined(target));
        }

        [TestMethod]
        public void TargetMandatoryParamSuccess()
        {
            Target target = _dataGatherer.GetTarget(_testRelease, _testTargetName);

            Assert.IsNotNull(target);

            TargetParameter param = target.Parameters.Where(m => m.Name == _testTargetMandatoryParamName).First();

            TargetSetting setting = new TargetSetting();
            setting.Parameter = param;
            setting.Setting = @"C:\SomeDirectory\SomeFile.xel";

            target.Settings = new List<TargetSetting>() { setting };

            Assert.IsTrue(_xeUtility.AllMandatorySettingsDefined(target));
        }
    }
}
