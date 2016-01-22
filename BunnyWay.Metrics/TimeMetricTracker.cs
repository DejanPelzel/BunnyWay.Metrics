using System;
using System.Diagnostics;

namespace BunnyWay.Metrics
{
    /// <summary>
    /// TimeTracker is used for measuring execution time
    /// </summary>
    public class TimeMetricTracker : IDisposable
    {
        /// <summary>
        /// The time elapsed in miliseconds from creating until disposing the tracker
        /// </summary>
        public long ElapsedMiliseconds { get; private set; }

        /// <summary>
        /// The metric name that is being tracked
        /// </summary>
        public string MetricName { get; private set; }

        /// <summary>
        /// The tags that will be used for the metric
        /// </summary>
        public Tag[] Tags { get; private set; }

        /// <summary>
        /// The stopwatch used for measuring the time
        /// </summary>
        private Stopwatch _stopwatch;

        /// <summary>
        /// The event called when the tracker is disposed and the metric has been tracked
        /// </summary>
        public event MetricTrackedEventHandler MetricTracked;

        /// <summary>
        /// Create and start a new time metric tracker
        /// </summary>
        public TimeMetricTracker(string metricName, params Tag[] tags)
        {
            this.MetricName = metricName;
            this.Tags = tags;
            this._stopwatch = Stopwatch.StartNew();
        }

        /// <summary>
        /// Dispose the tracker and call the callback to register the values
        /// </summary>
        public void Dispose()
        {
            this.ElapsedMiliseconds = this._stopwatch.ElapsedMilliseconds;
            if(this.MetricTracked != null)
            {
                this.MetricTracked(this, new MetricTrackedEventArgs(this.MetricName, this.ElapsedMiliseconds));
            }
        }
    }
}
