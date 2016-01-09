using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace BunnyWay.Metrics.InfluxDB
{
    /// <summary>
    /// InfluxLineClient is the basic object that handles the InfluxDB communication
    /// </summary>
    public class InfluxDBLineClient
    {
        /// <summary>
        /// The URL where the InfluxDB database is accessible
        /// </summary>
        public string ServerUrl { get; private set; }

        /// <summary>
        /// The name of the database that we will be writing to
        /// </summary>
        public string DatabaseName { get; private set; }

        /// <summary>
        /// The username used for authentication with InfluxDB
        /// </summary>
        private string Username { get; set; }

        /// <summary>
        /// The password used for authentication with InfluxDB
        /// </summary>
        private string Password { get; set; }

        /// <summary>
        /// The URL authentication string part
        /// </summary>
        internal string UrlAuthenticationString { get; set; }

        /// <summary>
        /// Create a new InfluxLineClient
        /// </summary>
        public InfluxDBLineClient(string serverUrl, string databaseName, string username = null, string password = null)
        {
            if (string.IsNullOrEmpty(serverUrl))
                throw new ArgumentException("serverUrl", "A server url must be specified");
            if (string.IsNullOrEmpty(serverUrl))
                throw new ArgumentException("databaseName", "A database name must be specified");

            // Make sure the server URL ends with a slash
            if (!serverUrl.EndsWith("/"))
            {
                serverUrl += "/";
            }

            // Validate service URL
            try
            {
                var uri = new Uri(serverUrl);
            }
            catch
            {
                throw new ArgumentNullException("serverUrl", "Invalid server url specified");
            }

            this.ServerUrl = serverUrl;
            this.DatabaseName = databaseName;
            this.Username = username;
            this.Password = password;

            // Generate the URL authentication string
            if (username != null && password != null)
            {
                this.UrlAuthenticationString = $"&u={Uri.EscapeDataString(this.Username)}&p={Uri.EscapeDataString(this.Password)}";
            }
        }

        /// <summary>
        /// Open a new InfluxDBWriter with the settings from the current client
        /// </summary>
        /// <returns></returns>
        public InfluxDBWriter OpenWriter()
        {
            return new InfluxDBWriter(this);
        }
    }
}
