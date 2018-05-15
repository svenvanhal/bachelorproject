﻿using NUnit.Framework;
using System;
using System.ComponentModel;
using System.IO;
using Timetabling.Helper;
using Timetabling.Resources;

namespace Timetabling.Algorithm.Tests
{

    class CommandLineArgumentsTest
    {

        [Test]
        public void CombineTestEmptyFirst()
        {

            var cla = new CommandLineArguments();
            var cla2 = new CommandLineArguments
            {
                { "key1", "" }
            };

            CommandLineArguments merged = cla.Combine(cla2);

            Assert.True(merged.ContainsKey("key1"));

        }

        [Test]
        public void CombineTestEmptySecond()
        {

            var cla = new CommandLineArguments
            {
                { "key1", "value1" }
            };
            var cla2 = new CommandLineArguments();

            CommandLineArguments merged = cla.Combine(cla2);

            Assert.True(merged.ContainsKey("key1"));

        }

        [Test]
        public void CombineTestOverwrite()
        {

            var cla = new CommandLineArguments
            {
                { "key1", "old_value" }
            };

            var cla2 = new CommandLineArguments
            {
                { "key1", "new_value" }
            };

            CommandLineArguments merged = cla.Combine(cla2);

            Assert.AreEqual(merged["key1"], "new_value");

        }

        [Test]
        public void EncodeArgumentTestSpace()
        {

            var unencoded = "value with spaces";
            var expected = "\"value with spaces\"";

            Assert.AreEqual(expected, CommandLineArguments.EncodeArgument(unencoded));

        }

        [Test]
        public void EncodeArgumentTestEmpty()
        {

            var unencoded = "";
            var expected = "\"\"";

            Assert.AreEqual(expected, CommandLineArguments.EncodeArgument(unencoded));

        }


    }
}
