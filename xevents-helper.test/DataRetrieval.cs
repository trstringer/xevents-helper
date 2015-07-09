using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using xevents_helper;
using xevents_helper.Models;

namespace xevents_helper.test
{
    [TestClass]
    public class DataRetrieval
    {
        [TestMethod]
        public void DataGathererInstantiation()
        {
            DataGatherer dg = new DataGatherer();

            Assert.IsNotNull(dg);
        }
    }
}
