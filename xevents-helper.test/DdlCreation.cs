using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using xevents_helper.Models;

namespace xevents_helper.test
{
    [TestClass]
    public class DdlCreation
    {
        private Release _release;
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
        }
    }
}
