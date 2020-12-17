using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CompanyCoreLib.Tests
{
    [TestClass]
    public class AnalyticsTest
    {
        static Analytics AnalyticsClass = null;

        [ClassInitialize]
        static public void Init(TestContext tc)
        {
            AnalyticsClass = new Analytics();
        }


        [TestMethod]
        public void TestMethod1()
        {
            List<DateTime> dates = new List<DateTime>() { new DateTime(2020, 12, 17), new DateTime(2020, 11, 17), new DateTime(2020, 12, 1) };

            dates = AnalyticsClass.PopularMonths(dates);
        }
    }
}
