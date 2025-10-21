using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace UpdownDotnet.Models
{
    /// <summary>
    /// Represents an Updown.io status page.
    /// </summary>
    public class StatusPage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StatusPage"/> class.
        /// </summary>
        public StatusPage() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="StatusPage"/> class from parameters.
        /// </summary>
        /// <param name="parameters">The status page parameters.</param>
        public StatusPage(StatusPageParameters parameters)
        {
            Name = parameters.Name;
            Description = parameters.Description;
            Visibility = parameters.Visibility;
            AccessKey = parameters.AccessKey;
            Checks = parameters.Checks;
        }

        /// <summary>
        /// Gets or sets the unique token identifying this status page.
        /// </summary>
        [JsonPropertyName("token")]
        public string? Token { get; set; }

        /// <summary>
        /// Gets or sets the public URL for this status page.
        /// </summary>
        [JsonPropertyName("url")]
        public string? Url { get; set; }

        /// <summary>
        /// Gets or sets the name of the status page.
        /// </summary>
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        /// <summary>
        /// Gets or sets the description of the status page.
        /// </summary>
        [JsonPropertyName("description")]
        public string? Description { get; set; }

        /// <summary>
        /// Gets or sets the visibility setting (public, private, etc.).
        /// </summary>
        [JsonPropertyName("visibility")]
        public string? Visibility { get; set; }

        /// <summary>
        /// Gets or sets the access key for private status pages.
        /// </summary>
        [JsonPropertyName("access_key")]
        public string? AccessKey { get; set; }

        /// <summary>
        /// Gets or sets the list of check tokens included on this status page.
        /// </summary>
        [JsonPropertyName("checks")]
        public List<string>? Checks { get; set; }

        // Obsolete properties
        /// <summary>
        /// Gets or sets the access key.
        /// </summary>
        [Obsolete("Use AccessKey instead.")]
        [JsonIgnore]
        public string? Access_Key
        {
            get => AccessKey;
            set => AccessKey = value;
        }
    }

    /// <summary>
    /// Parameters for creating or updating a status page.
    /// </summary>
    public class StatusPageParameters
    {
        /// <summary>
        /// Gets or sets the name of the status page.
        /// </summary>
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        /// <summary>
        /// Gets or sets the description of the status page.
        /// </summary>
        [JsonPropertyName("description")]
        public string? Description { get; set; }

        /// <summary>
        /// Gets or sets the visibility setting.
        /// </summary>
        [JsonPropertyName("visibility")]
        public string? Visibility { get; set; }

        /// <summary>
        /// Gets or sets the access key for private status pages.
        /// </summary>
        [JsonPropertyName("access_key")]
        public string? AccessKey { get; set; }

        /// <summary>
        /// Gets or sets the list of check tokens to include.
        /// </summary>
        [JsonPropertyName("checks")]
        public List<string>? Checks { get; set; }

        // Obsolete properties
        /// <summary>
        /// Gets or sets the access key.
        /// </summary>
        [Obsolete("Use AccessKey instead.")]
        [JsonIgnore]
        public string? Access_Key
        {
            get => AccessKey;
            set => AccessKey = value;
        }
    }
}
