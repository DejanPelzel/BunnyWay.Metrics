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
            // Initializes the MetricsTracker.Default object
            MetricsTracker.InitializeDefault();

            // Track a counter metric, it will simply be incremented by 1
            MetricsTracker.Default.TrackCounter("WebApp.Login.FailedLoginAttempt");

            // Track a counter metric, increment it by a set amount
            MetricsTracker.Default.TrackCounter("Server.RequestsServed", 20);

            // Track a metric, each of these is uniquely registered
            MetricsTracker.Default.TrackMetric("WebApp.ImageProcessing.QueueLength", 230);

            // Track a metric with tags
            MetricsTracker.Default.TrackMetric("WebApp.EmailQueue.QueueLength", 230, new Tag("ServerName", "as1"), new Tag("ServerGroup", "2"));

            // Simple way to track timing, just make sure that the object returned by 
            // MetricsTracker.Default.TrackTimeMetric is disposed when the tracker should be stopped
            using (var timer = MetricsTracker.Default.TrackTimeMetric("WebApp.PasswordGeneration.Time"))
            {
                Thread.Sleep(1000);
            }

            Console.ReadLine();
        }
    }
}
