using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BunnyWay.Metrics;
using System.Threading;

namespace BunnyWay.Metrics.Example
{
    class Program
    {
        static void Main(string[] args)
        {
            MetricsTracker.InitializeDefault();

            Task.Run(() =>
            {
                var tags = new Tag[] { new Tag("EdgeServerId", "7") };
                var random = new Random();
                while (true)
                {
                    MetricsTracker.Default.TrackMetric("EdgeServer.RequestsServed", 2, tags);
                    Thread.Sleep(1000);
                }
            });
            
            Console.ReadLine();
        }
    }
}
