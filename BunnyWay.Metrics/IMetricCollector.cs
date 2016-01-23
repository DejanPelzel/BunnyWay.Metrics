using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BunnyWay.Metrics
{
    /// <summary>
    /// Defines the metric collector interface that is called by the metrics tracker to get a metric periodically
    /// </summary>
    public interface IMetricCollector
    {
        /// <summary>
        /// Get the metrics that should be registered
        /// </summary>
        /// <returns></returns>
        List<Metric> GetMetrics();

        /// <summary>
        /// Indicates weather the metric is a counter
        /// </summary>
        bool IsCounter { get; }
    }
}
