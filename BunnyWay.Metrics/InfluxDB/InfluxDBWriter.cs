using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;

namespace BunnyWay.Metrics.InfluxDB
{
    /// <summary>
    /// 
    /// </summary>
    public class InfluxDBWriter : IDisposable
    {
        /// <summary>
        /// 
        /// </summary>
        public InfluxDBLineClient Client { get; set; }

        /// <summary>
        /// The string builder used for constructing data value strings
        /// </summary>
        private StringBuilder _StringBuilder;

        /// <summary>
        /// The buffer used for encoding data strings
        /// </summary>
        private byte[] _Buffer;

        /// <summary>
        /// Is true when the writer has opened a request and began writing data
        /// </summary>
        private HttpWebRequest _HttpRequest = null;

        /// <summary>
        /// The buffered request stream
        /// </summary>
        private BufferedStream _BufferedRequestStream;

        /// <summary>
        /// Create a new InfluxDBWriter object
        /// </summary>
        public InfluxDBWriter(InfluxDBLineClient client)
        {
            this.Client = client;
            this._StringBuilder = new StringBuilder(2048);
            this._Buffer = new byte[4096];
            this._HttpRequest = this.GetServiceRequest();
            this._BufferedRequestStream = new BufferedStream(this._HttpRequest.GetRequestStream(), 4096);
        }

        /// <summary>
        /// Write the data point into the current request
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public void WriteDataPoint(string key, double value)
        {
            // Create the data string
            this._StringBuilder.Clear();
            this._StringBuilder.Append(key);
            this._StringBuilder.Append(" value=");
            this._StringBuilder.Append(value.ToString());
            this._StringBuilder.Append("\n");

            // Write the data string
            var dataByteLength = Encoding.UTF8.GetBytes(this._StringBuilder.ToString(), 0, this._StringBuilder.Length, this._Buffer, 0);
            this._BufferedRequestStream.Write(this._Buffer, 0, dataByteLength);
        }

        /// <summary>
        /// Dispose the 
        /// </summary>
        public void Dispose()
        {
            this._BufferedRequestStream.Flush();
            this._BufferedRequestStream.Close();

            if (this._HttpRequest != null)
            {
                this._HttpRequest.GetResponse().Close();
            }
        }

        /// <summary>
        /// Create a new service request for the 
        /// </summary>
        /// <returns></returns>
        public HttpWebRequest GetServiceRequest()
        {
            var request = HttpWebRequest.CreateHttp($"{this.Client.ServerUrl}write?db={Uri.EscapeDataString(this.Client.DatabaseName)}{this.Client.UrlAuthenticationString}");
            request.Method = "POST";
            request.KeepAlive = true;
            return request;
        }
    }
}
