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
        private string _actionName1;
        private string _actionName2;
        private string _eventName2;
        private string _targetName1;
        private string _targetMandatoryParamName1;
        private string _targetName2;

        public DdlCreation()
        {
            _releaseName = "SQL 2012";
            _release = new Release() { Name = _releaseName };
            _xeUtility = new XeUtility();
            _dataGatherer = new DataGatherer();

            _eventName1 = "sql_statement_completed";
            _eventName2 = "sql_statement_starting";

            _actionName1 = "sql_text";
            _actionName2 = "database_id";

            _targetName1 = "event_file";
            _targetMandatoryParamName1 = "filename";
            _targetName2 = "histogram";
        }

        [TestMethod]
        public void SingleEvent()
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

        [TestMethod]
        public void MultiEvent()
        {
            Session session = new Session();
            session.Name = "Session2 with a space";
            session.TargetRelease = _release;

            List<Event> events = new List<Event>();
            events.Add(_dataGatherer.SearchEvents(_release, _eventName1, SearchOption.ByName).First());
            events.Add(_dataGatherer.SearchEvents(_release, _eventName2, SearchOption.ByName).First());
            session.Events = events;

            string sessionDefinition = _xeUtility.GetCreateDdl(session);

            Assert.IsFalse(string.IsNullOrWhiteSpace(sessionDefinition));

            Debug.WriteLine(sessionDefinition);
        }

        [TestMethod]
        public void SingleEventSingleAction()
        {
            Session session = new Session();
            session.Name = "Session2";
            session.TargetRelease = _release;

            List<Event> events = new List<Event>();
            Event xeEvent = _dataGatherer.SearchEvents(_release, _eventName1, SearchOption.ByName).First();
            xeEvent.Actions = new List<xevents_helper.Models.Action>();
            ((List<xevents_helper.Models.Action>)xeEvent.Actions).Add(_dataGatherer.GetAction(_release, _actionName1));
            events.Add(xeEvent);
            session.Events = events;

            string sessionDefinition = _xeUtility.GetCreateDdl(session);

            Assert.IsFalse(string.IsNullOrWhiteSpace(sessionDefinition));

            Debug.WriteLine(sessionDefinition);
        }

        [TestMethod]
        public void SingleEventMultiAction()
        {
            Session session = new Session();
            session.Name = "Session3";
            session.TargetRelease = _release;

            List<Event> events = new List<Event>();
            Event xeEvent = _dataGatherer.SearchEvents(_release, _eventName1, SearchOption.ByName).First();
            xeEvent.Actions = new List<xevents_helper.Models.Action>();
            ((List<xevents_helper.Models.Action>)xeEvent.Actions).Add(_dataGatherer.GetAction(_release, _actionName1));
            ((List<xevents_helper.Models.Action>)xeEvent.Actions).Add(_dataGatherer.GetAction(_release, _actionName2));
            events.Add(xeEvent);
            session.Events = events;

            string sessionDefinition = _xeUtility.GetCreateDdl(session);

            Assert.IsFalse(string.IsNullOrWhiteSpace(sessionDefinition));

            Debug.WriteLine(sessionDefinition);
        }

        [TestMethod]
        public void MultiEventSingleAction()
        {
            Session session = new Session();
            session.Name = "Session2";
            session.TargetRelease = _release;

            List<Event> events = new List<Event>();

            Event xeEvent = _dataGatherer.SearchEvents(_release, _eventName1, SearchOption.ByName).First();
            xeEvent.Actions = new List<xevents_helper.Models.Action>();
            ((List<xevents_helper.Models.Action>)xeEvent.Actions).Add(_dataGatherer.GetAction(_release, _actionName1));
            events.Add(xeEvent);

            xeEvent = _dataGatherer.SearchEvents(_release, _eventName2, SearchOption.ByName).First();
            xeEvent.Actions = new List<xevents_helper.Models.Action>();
            ((List<xevents_helper.Models.Action>)xeEvent.Actions).Add(_dataGatherer.GetAction(_release, _actionName2));
            events.Add(xeEvent);

            session.Events = events;

            string sessionDefinition = _xeUtility.GetCreateDdl(session);

            Assert.IsFalse(string.IsNullOrWhiteSpace(sessionDefinition));

            Debug.WriteLine(sessionDefinition);
        }

        [TestMethod]
        public void MultiEventMultiAction()
        {
            Session session = new Session();
            session.Name = "Session2";
            session.TargetRelease = _release;

            List<Event> events = new List<Event>();

            Event xeEvent = _dataGatherer.SearchEvents(_release, _eventName1, SearchOption.ByName).First();
            xeEvent.Actions = new List<xevents_helper.Models.Action>();
            ((List<xevents_helper.Models.Action>)xeEvent.Actions).Add(_dataGatherer.GetAction(_release, _actionName1));
            ((List<xevents_helper.Models.Action>)xeEvent.Actions).Add(_dataGatherer.GetAction(_release, _actionName2));
            events.Add(xeEvent);

            xeEvent = _dataGatherer.SearchEvents(_release, _eventName2, SearchOption.ByName).First();
            xeEvent.Actions = new List<xevents_helper.Models.Action>();
            ((List<xevents_helper.Models.Action>)xeEvent.Actions).Add(_dataGatherer.GetAction(_release, _actionName1));
            ((List<xevents_helper.Models.Action>)xeEvent.Actions).Add(_dataGatherer.GetAction(_release, _actionName2));
            events.Add(xeEvent);

            session.Events = events;

            string sessionDefinition = _xeUtility.GetCreateDdl(session);

            Assert.IsFalse(string.IsNullOrWhiteSpace(sessionDefinition));

            Debug.WriteLine(sessionDefinition);
        }

        [TestMethod]
        public void SingleEventSingleTarget()
        {
            Session session = new Session();
            session.Name = "Session3";
            session.TargetRelease = _release;

            List<Event> events = new List<Event>();
            Event xeEvent = _dataGatherer.SearchEvents(_release, _eventName1, SearchOption.ByName).First();
            events.Add(xeEvent);
            session.Events = events;

            Target target = _dataGatherer.GetTarget(_release, _targetName1);
            session.Targets = new List<Target>() { target };

            TargetParameter param = target.Parameters.Where(m => m.Name == _targetMandatoryParamName1).First();

            TargetSetting setting = new TargetSetting();
            setting.Parameter = param;
            setting.Setting = @"C:\SomeDirectory\SomeFile.xel";

            target.Settings = new List<TargetSetting>() { setting };

            string sessionDefinition = _xeUtility.GetCreateDdl(session);

            Assert.IsFalse(string.IsNullOrWhiteSpace(sessionDefinition));

            Debug.WriteLine(sessionDefinition);
        }

        [TestMethod]
        public void SingleEventMultiTarget()
        {
            Session session = new Session();
            session.Name = "Session3";
            session.TargetRelease = _release;

            List<Event> events = new List<Event>();
            Event xeEvent = _dataGatherer.SearchEvents(_release, _eventName1, SearchOption.ByName).First();
            events.Add(xeEvent);
            session.Events = events;

            Target target = _dataGatherer.GetTarget(_release, _targetName1);
            Target target2 = _dataGatherer.GetTarget(_release, _targetName2);
            session.Targets = new List<Target>() { target, target2 };

            TargetParameter param = target.Parameters.Where(m => m.Name == _targetMandatoryParamName1).First();

            TargetSetting setting = new TargetSetting();
            setting.Parameter = param;
            setting.Setting = @"C:\SomeDirectory\SomeFile.xel";

            target.Settings = new List<TargetSetting>() { setting };

            string sessionDefinition = _xeUtility.GetCreateDdl(session);

            Assert.IsFalse(string.IsNullOrWhiteSpace(sessionDefinition));

            Debug.WriteLine(sessionDefinition);
        }

        [TestMethod]
        public void SingleEventWithSessionOptions()
        {
            Session session = new Session();
            session.Name = "Session2";
            session.TargetRelease = _release;

            List<Event> events = new List<Event>();
            Event xeEvent = _dataGatherer.SearchEvents(_release, _eventName1, SearchOption.ByName).First();
            xeEvent.Actions = new List<xevents_helper.Models.Action>();
            ((List<xevents_helper.Models.Action>)xeEvent.Actions).Add(_dataGatherer.GetAction(_release, _actionName1));
            events.Add(xeEvent);
            session.Events = events;

            session.Options = new SessionOptions();
            session.Options.TrackCausality = true;
            session.Options.StartWithInstance = true;
            session.Options.MemoryPartitionMode = MemoryPartitionMode.PerNode;

            string sessionDefinition = _xeUtility.GetCreateDdl(session);

            Assert.IsFalse(string.IsNullOrWhiteSpace(sessionDefinition));

            Debug.WriteLine(sessionDefinition);
        }
    }
}
