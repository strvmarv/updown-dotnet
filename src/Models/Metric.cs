using System;
using System.Text.Json.Serialization;

namespace UpdownDotnet.Models
{
    /// <summary>
    /// Represents performance metrics for a check during a time period.
    /// </summary>
    public class Metric
    {
        /// <summary>
        /// Gets or sets the timestamp (in seconds since epoch) for this metric data point.
        /// </summary>
        [JsonPropertyName("time")]
        public long? Time { get; set; }

        /// <summary>
        /// Gets or sets the average response time in milliseconds.
        /// </summary>
        [JsonPropertyName("apdex")]
        public double? Apdex { get; set; }

        /// <summary>
        /// Gets or sets the total number of requests.
        /// </summary>
        [JsonPropertyName("requests")]
        public Requests? Requests { get; set; }

        /// <summary>
        /// Gets or sets the response time statistics.
        /// </summary>
        [JsonPropertyName("timings")]
        public Timings? Timings { get; set; }
    }

    /// <summary>
    /// Represents request statistics for a metric period.
    /// </summary>
    public class Requests
    {
        /// <summary>
        /// Gets or sets the number of samples taken.
        /// </summary>
        [JsonPropertyName("samples")]
        public int? Samples { get; set; }

        /// <summary>
        /// Gets or sets the number of failures.
        /// </summary>
        [JsonPropertyName("failures")]
        public int? Failures { get; set; }

        /// <summary>
        /// Gets or sets the number of satisfied responses (fast).
        /// </summary>
        [JsonPropertyName("satisfied")]
        public int? Satisfied { get; set; }

        /// <summary>
        /// Gets or sets the number of tolerable responses (acceptable).
        /// </summary>
        [JsonPropertyName("tolerated")]
        public int? Tolerated { get; set; }

        /// <summary>
        /// Gets or sets a breakdown of HTTP status codes.
        /// </summary>
        [JsonPropertyName("by_response_time")]
        public ByResponseTime? ByResponseTime { get; set; }
    }

    /// <summary>
    /// Represents response time categorization.
    /// </summary>
    public class ByResponseTime
    {
        /// <summary>
        /// Gets or sets responses under 250ms.
        /// </summary>
        [JsonPropertyName("under250")]
        public int? Under250 { get; set; }

        /// <summary>
        /// Gets or sets responses under 500ms.
        /// </summary>
        [JsonPropertyName("under500")]
        public int? Under500 { get; set; }

        /// <summary>
        /// Gets or sets responses under 1000ms.
        /// </summary>
        [JsonPropertyName("under1000")]
        public int? Under1000 { get; set; }

        /// <summary>
        /// Gets or sets responses under 2000ms.
        /// </summary>
        [JsonPropertyName("under2000")]
        public int? Under2000 { get; set; }

        /// <summary>
        /// Gets or sets responses under 4000ms.
        /// </summary>
        [JsonPropertyName("under4000")]
        public int? Under4000 { get; set; }

        /// <summary>
        /// Gets or sets responses over 4000ms.
        /// </summary>
        [JsonPropertyName("over4000")]
        public int? Over4000 { get; set; }
    }

    /// <summary>
    /// Represents timing statistics.
    /// </summary>
    public class Timings
    {
        /// <summary>
        /// Gets or sets the redirect time in milliseconds.
        /// </summary>
        [JsonPropertyName("redirect")]
        public int? Redirect { get; set; }

        /// <summary>
        /// Gets or sets the name lookup time in milliseconds.
        /// </summary>
        [JsonPropertyName("namelookup")]
        public int? Namelookup { get; set; }

        /// <summary>
        /// Gets or sets the connection time in milliseconds.
        /// </summary>
        [JsonPropertyName("connection")]
        public int? Connection { get; set; }

        /// <summary>
        /// Gets or sets the handshake time in milliseconds.
        /// </summary>
        [JsonPropertyName("handshake")]
        public int? Handshake { get; set; }

        /// <summary>
        /// Gets or sets the response time in milliseconds.
        /// </summary>
        [JsonPropertyName("response")]
        public int? Response { get; set; }

        /// <summary>
        /// Gets or sets the total time in milliseconds.
        /// </summary>
        [JsonPropertyName("total")]
        public int? Total { get; set; }
    }
}

