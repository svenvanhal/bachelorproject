﻿using System.Threading;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using Timetabling.Algorithms;
using Timetabling.Resources;

namespace Timetabling.Tests
{

    [TestFixture]
    public class TimetableGeneratorTest
    {

        [Test]
        public void RefreshIdTest()
        {
            var ttg = new TimetableGenerator();

            var previousIdentifier = ttg.CurrentRunIdentifier;
            ttg.RefreshIdentifier();

            // Verify that the identifier is refreshed
            Assert.AreNotEqual(previousIdentifier, ttg.CurrentRunIdentifier);
        }

        [Test]
        public void RunAlgorithmTest()
        {
            var ttg = new TimetableGenerator();

            var mockAlgorithm = new Mock<Algorithm>();
            var input = "";

            var previousIdentifier = ttg.CurrentRunIdentifier;

            var task = ttg.RunAlgorithm(mockAlgorithm.Object, input);

            // Verify that the identifier is refreshed
            Assert.AreNotEqual(previousIdentifier, ttg.CurrentRunIdentifier);

            // Verify that a Timetable task is generated
            mockAlgorithm.Verify(mock => mock.GenerateTask(ttg.CurrentRunIdentifier, input, It.IsAny<CancellationToken>()), Times.Once);
            Assert.IsInstanceOf<Task<Timetable>>(task);
        }

    }
}
