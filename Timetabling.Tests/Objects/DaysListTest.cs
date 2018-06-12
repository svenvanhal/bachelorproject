﻿using NUnit.Framework;
using Moq;
using System.Data.Entity;
using Timetabling.Objects;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Timetabling.DB;

namespace Timetabling.Tests.Objects
{
    [TestFixture()]
    internal class DaysListTest
    {
        XElement test;

        [SetUp]
        public void Init()
        {
            var data = new List<Section_WeekEnd>{
                new Section_WeekEnd{dayIndex = 0, sectionId =1},
                new Section_WeekEnd{dayIndex = 1, sectionId= 1},
                new Section_WeekEnd{dayIndex = 3,sectionId = 1}
            }.AsQueryable();

            var mockSet = new Mock<DbSet<Section_WeekEnd>>();
            mockSet.As<IQueryable<Section_WeekEnd>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<Section_WeekEnd>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<Section_WeekEnd>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<Section_WeekEnd>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            var mockDB = new Mock<DataModel>();
            mockDB.Setup(item => item.Section_WeekEnd).Returns(mockSet.Object);

            var list = new DaysList(mockDB.Object);
            test = list.Create();

        }

        [Test]
        public void ElementNameTest()
        {
            Assert.AreEqual("Days_List", test.Name.ToString());
        }


        [Test]
        public void NumberOfDaysElementTest()
        {

            Assert.AreEqual("7", test.Elements("Number_of_Days").First().Value);
        }

        [Test]
        public void NumberOfDaysTest()
        {
            Assert.AreEqual(7, test.Elements("Day").Count());
        }

        [Test]
        public void WeekDayTest()
        {
            Assert.AreEqual(1, test.Elements("Day").Elements("Name").Count(item => item.Value.Equals(Days.Tuesday.ToString())));
        }
    }
}
