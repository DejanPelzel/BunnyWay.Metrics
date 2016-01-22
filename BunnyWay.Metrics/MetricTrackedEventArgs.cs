using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BunnyWay.Metrics
{
    /// <summary>
    /// The event args used when calling the metric tracked event
    /// </summary>
    public class MetricTrackedEventArgs
    {
        /// <summary>
        /// The name of the metric
        /// </summary>
        public string MetricName { get; set; }

        /// <summary>
        /// The value of the metric
        /// </summary>
        public double MetricValue { get; set; }

        /// <summary>
        /// The tags of the metric
        /// </summary>
        public Tag[] Tags { get; private set; }

        /// <summary>
        /// Create a new MetricTrackedEventArgs object
        /// </summary>
        /// <param name="metricValue"></param>
        public MetricTrackedEventArgs(string metricName, double metricValue, params Tag[] tags)
        {
            this.MetricName = MetricName;
            this.MetricValue = metricValue;
            this.Tags = tags;
        }
    }

    /// <summary>
    /// The event handler used when calling the metric tracked event
    /// </summary>
    public delegate void MetricTrackedEventHandler(TimeMetricTracker sender, MetricTrackedEventArgs e);
}
