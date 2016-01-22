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

            // Track a counter metric
            MetricsTracker.Default.TrackMetric("WebApp.Login.FailedLoginAttempt");

            // Track a metric
            MetricsTracker.Default.TrackMetric("WebApp.ImageProcessing.QueueLength", 230);

            // Track with tags
            MetricsTracker.Default.TrackMetric("WebApp.ImageProcessing.QueueLength", 230, new Tag("ServerName", "as1"), new Tag("ServerGroup", "2"));

            // Simple way to track timing
            using (var timer = MetricsTracker.Default.TrackTimeMetric("WebApp.PasswordGeneration.Time"))
            {
                Thread.Sleep(1000);
            }

            Console.ReadLine();
        }
    }
}
