using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BunnyWay.Metrics;
using System.Net;
using BunnyWay.Metrics.InfluxDB;
using System.Threading.Tasks;

namespace BunnyWay.Metrics.Tests
{
    [TestClass]
    public class InfluxDBWriterTest
    {
        private HttpListener GetListener()
        {
            var httpListener = new HttpListener();
            httpListener.Prefixes.Add("http://127.0.0.1:3000/");
            httpListener.Start();

            return httpListener;
        }

        /// <summary>
        /// Test empty request
        /// </summary>
        [TestMethod]
        public void TestEmptyRequest()
        {
            using (var listener = this.GetListener())
            {
                var task = Task.Run(async () =>
                {
                    var context = await listener.GetContextAsync();
                    try
                    {
                        Assert.AreEqual("http://127.0.0.1:3000/write?db=data", context.Request.Url.ToString());
                    }
                    finally {
                        context.Response.Close();
                    }
                });
                
                using (var writer = new InfluxDBWriter(new InfluxDBLineClient("http://127.0.0.1:3000/", "data")))
                {
                    var request = writer.GetServiceRequest();
                    request.Timeout = 2000;
                    request.ReadWriteTimeout = 2000;
                }

                task.Wait();
            }
        }

        /// <summary>
        /// Test the tag encoding
        /// </summary>
        [TestMethod]
        public void TestGetServiceRequest()
        {
            using (var listener = this.GetListener())
            {
                var writer = new InfluxDBWriter(new InfluxDBLineClient("http://127.0.0.1:3000/", "data"));
                var request = writer.GetServiceRequest();

                // Test unauthenticated request
                Assert.AreEqual("POST", request.Method);
                Assert.AreEqual("http://127.0.0.1:3000/write?db=data", request.RequestUri.ToString());

                // Test authenticated request
                writer = new InfluxDBWriter(new InfluxDBLineClient("http://127.0.0.1:3000/", "data", "myuser", "mypass"));
                request = writer.GetServiceRequest();
                Assert.AreEqual("http://127.0.0.1:3000/write?db=data&u=myuser&p=mypass", request.RequestUri.ToString());
            }
        }
    }
}
