using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Configuration;
using BunnyWay.Metrics.InfluxDB;

namespace BunnyWay.Metrics
{
    public class MetricsTracker
    {
        /// <summary>
        /// The data interval at which the metric will be aggregated and processed
        /// </summary>
        public int DataInterval { get; private set; }

        /// <summary>
        /// The current metric value
        /// </summary>
        private long Value { get; set; }

        /// <summary>
        /// The timer used for sending the metrics
        /// </summary>
        private Timer _Timer;

        /// <summary>
        /// The trackers
        /// </summary>
        private Dictionary<string, double> _Metrics = null;

        /// <summary>
        /// The client that will be used for sending the statistics to InfluxDB
        /// </summary>
        public InfluxDBLineClient InfluxDBClient { get; private set; }

        /// <summary>
        /// The default metrics tracker with config loaded from the config file
        /// </summary>
        public static MetricsTracker Default { get; private set; }

        /// <summary>
        /// Create and start a new metric tracker with the given period in seconds
        /// </summary>
        /// <param name="dataInterval"></param>
        public MetricsTracker(int dataInterval = 60, InfluxDBLineClient influxDbClient = null)
        {
            this._Metrics = new Dictionary<string, double>();
            this.DataInterval = dataInterval;
            this._Timer = new Timer(this.TimerCallback, null, dataInterval * 1000, dataInterval * 1000);

            if (influxDbClient == null)
            {
                // Load the configuration data
                string serverUrl = ConfigurationManager.AppSettings["BunnyMetrics.InfluxDB.ServerUrl"];
                string databaseName = ConfigurationManager.AppSettings["BunnyMetrics.InfluxDB.DatabaseName"];
                string username = ConfigurationManager.AppSettings["BunnyMetrics.InfluxDB.Username"];
                string password = ConfigurationManager.AppSettings["BunnyMetrics.InfluxDB.Password"];

                // Load the a client with the default credentials
                influxDbClient = new InfluxDBLineClient(
                    serverUrl,
                    databaseName,
                    string.IsNullOrWhiteSpace(username) ? null : username,
                    string.IsNullOrWhiteSpace(password) ? null : password);
            }


            this.InfluxDBClient = influxDbClient;
        }

        /// <summary>
        /// The callback called when the data is ready to be sent
        /// </summary>
        /// <param name="stateInfo"></param>
        private void TimerCallback(Object stateInfo)
        {

            Dictionary<string, double> metricsToSend = new Dictionary<string, double>();

            try
            {
                // Make a copy of the data so we don't block the threads
                lock (this._Metrics)
                {
                    var keys = this._Metrics.Keys.ToList();
                    for (int i = 0; i < keys.Count; i++)
                    {
                        var key = keys[i];
                        var value = this._Metrics[key];

                        metricsToSend.Add(key, value);
                        this._Metrics[key] = 0;
                    }
                }
            }
            catch { }

            // Send the metrics
            Task.Run(() =>
            {
                try
                {
                    using (var writer = this.InfluxDBClient.OpenWriter())
                    {
                        foreach (var keyValue in metricsToSend) {
                            writer.WriteDataPoint(keyValue.Key, keyValue.Value);
                        }
                    }
                }
                catch { }
            });
        }

        /// <summary>
        /// Remove the metric from the tracker
        /// </summary>
        /// <param name="metric"></param>
        public void RemoveMetric(string metric)
        {
            lock (this._Metrics)
            {
                try
                {
                    this._Metrics.Remove(metric);
                }
                catch { }
            }
        }

        /// <summary>
        /// Fire the event, incremeneing the metric value
        /// </summary>
        /// <param name="metricName">The name of the metrics that we are tracking</param>
        /// <param name="value">The value of the metrics that we are tracking</param>
        /// <param name="tags">(Optional) The tags of the metric that will be sent</param>
        public void TrackMetric(string metricName, long value, params Tag[] tags)
        {
            try
            {
                // Apply tags
                if (tags != null)
                {
                    metricName += " " + tags.ToString();
                }

                lock (this._Metrics)
                {
                    double currentValue = 0;
                    try
                    {
                        currentValue = this._Metrics[metricName];
                    }
                    catch { }

                    this._Metrics[metricName] = currentValue + value;
                }
            }
            catch { }
        }

        /// <summary>
        /// Initialize the default metrics tracker with config loaded from the config file
        /// </summary>
        public static void InitializeDefault(int dataInterval = 60, InfluxDBLineClient influxDbClient = null)
        {
            MetricsTracker.Default = new MetricsTracker(dataInterval, influxDbClient);
        }
    }
}
