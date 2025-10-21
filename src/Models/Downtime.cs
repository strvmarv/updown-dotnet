using System;
using System.Text.Json.Serialization;

namespace UpdownDotnet.Models
{
    /// <summary>
    /// Represents a downtime period for a check.
    /// </summary>
    public class Downtime
    {
        /// <summary>
        /// Gets or sets the error message that caused the downtime.
        /// </summary>
        [JsonPropertyName("error")]
        public string? Error { get; set; }

        /// <summary>
        /// Gets or sets the time when the downtime started.
        /// </summary>
        [JsonPropertyName("started_at")]
        public DateTimeOffset? StartedAt { get; set; }

        /// <summary>
        /// Gets or sets the time when the downtime ended (null if still down).
        /// </summary>
        [JsonPropertyName("ended_at")]
        public DateTimeOffset? EndedAt { get; set; }

        /// <summary>
        /// Gets or sets the duration of the downtime in seconds.
        /// </summary>
        [JsonPropertyName("duration")]
        public long? Duration { get; set; }

        // Obsolete properties
        /// <summary>
        /// Gets or sets the started at time.
        /// </summary>
        [Obsolete("Use StartedAt instead.")]
        [JsonIgnore]
        public DateTimeOffset? Started_At
        {
            get => StartedAt;
            set => StartedAt = value;
        }

        /// <summary>
        /// Gets or sets the ended at time.
        /// </summary>
        [Obsolete("Use EndedAt instead.")]
        [JsonIgnore]
        public DateTimeOffset? Ended_At
        {
            get => EndedAt;
            set => EndedAt = value;
        }
    }
}

