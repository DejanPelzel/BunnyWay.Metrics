using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BunnyWay.Metrics
{
    public class Metric
    {
        /// <summary>
        /// The name of the metric that will be collected
        /// </summary>
        public string MetricName { get; set; }

        /// <summary>
        /// The value of the metric
        /// </summary>
        public double Value { get; set; }

        /// <summary>
        /// The tags of the metric
        /// </summary>
        public Tag[] Tags { get; set; }
    }
}
