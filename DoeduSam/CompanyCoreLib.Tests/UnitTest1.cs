﻿using System;
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
        public void PopularMonths_ValidData()
        {
            List<DateTime> dates = new List<DateTime>() { 
                new DateTime(2020, 11, 17),
                new DateTime(2020, 12, 17),
                new DateTime(2020, 10, 1) };

            dates = AnalyticsClass.PopularMonths(dates);

            // должно вернуть все 3 записи
            Assert.AreEqual(dates.Count, 3);

            Assert.AreEqual(dates[0], new DateTime(2020, 10, 1));

            //Assert.AreEqual(dates[0].Year, 2020);
            //Assert.AreEqual(dates[0].Month, 10);
            //Assert.AreEqual(dates[0].Day, 1);
        }

        [TestMethod]
        public void PopularMonths_ValidDataWithAdditionalSortByDate()
        {
            List<DateTime> dates = new List<DateTime>() {
                new DateTime(2020, 12, 17),
                new DateTime(2020, 12, 15),
                new DateTime(2020, 11, 17),
                new DateTime(2020, 11, 15),
                new DateTime(2020, 10, 1) };

            dates = AnalyticsClass.PopularMonths(dates);

            // тоже должно вернуть 3 записи
            Assert.AreEqual(dates.Count, 3);

            // но первая ноябрь
            Assert.AreEqual(dates[0], new DateTime(2020, 11, 1));
        }

    }
}
