using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BunnyWay.Metrics;

namespace BunnyWay.Metrics.Tests
{
    [TestClass]
    public class TagTest
    {
        /// <summary>
        /// Test the tag encoding
        /// </summary>
        [TestMethod]
        public void TestEncoding()
        {
            var tag = new Tag("tag, Name", "value");
            Assert.AreEqual(tag.ToString(), "tag\\,\\ Name=value");

            tag = new Tag("tag, Name", "value hello, there");
            Assert.AreEqual(tag.ToString(), "tag\\,\\ Name=value\\ hello\\,\\ there");
        }

        /// <summary>
        /// Test the tag formatting
        /// </summary>
        [TestMethod]
        public void TestFormatting()
        {
            var tag = new Tag("tagName", "tagValue");
            Assert.AreEqual(tag.ToString(), "tagName=tagValue");
        }

        /// <summary>
        /// Test the tag formatting
        /// </summary>
        [TestMethod]
        public void TestBasics()
        {
            var tag = new Tag("tagName", "tagValue");

            Assert.AreEqual(tag.TagName, "tagName");
            Assert.AreEqual(tag.Value, "tagValue");
        }
    }
}
