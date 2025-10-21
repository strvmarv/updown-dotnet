using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace UpdownDotnet.Models
{
    /// <summary>
    /// Represents an Updown.io monitoring check.
    /// </summary>
    public class Check
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Check"/> class.
        /// </summary>
        public Check() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Check"/> class from parameters.
        /// </summary>
        /// <param name="parameters">The check parameters.</param>
        public Check(CheckParameters parameters)
        {
            Url = parameters.Url;
            Alias = parameters.Alias;
            Down = parameters.Down;
            Period = parameters.Period;
            ApdexT = parameters.ApdexT;
            StringMatch = parameters.StringMatch;
            Enabled = parameters.Enabled;
            Published = parameters.Published;
            DisabledLocations = parameters.DisabledLocations;
            Recipients = parameters.Recipients;
            MuteUntil = parameters.MuteUntil;
            FavIconUrl = parameters.FavIconUrl;
            CustomHeaders = parameters.CustomHeaders;
            HttpVerb = parameters.HttpVerb;
            HttpBody = parameters.HttpBody;
        }

        /// <summary>
        /// Gets or sets the unique token identifying this check.
        /// </summary>
        [JsonPropertyName("token")]
        public string? Token { get; set; }

        /// <summary>
        /// Gets or sets the URL being monitored.
        /// </summary>
        [JsonPropertyName("url")]
        public string? Url { get; set; }

        /// <summary>
        /// Gets or sets the alias/name for this check.
        /// </summary>
        [JsonPropertyName("alias")]
        public string? Alias { get; set; }

        /// <summary>
        /// Gets or sets the last HTTP status code received.
        /// </summary>
        [JsonPropertyName("last_status")]
        public double? LastStatus { get; set; }

        /// <summary>
        /// Gets or sets the uptime percentage.
        /// </summary>
        [JsonPropertyName("uptime")]
        public double? Uptime { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the check is currently down.
        /// </summary>
        [JsonPropertyName("down")]
        public bool? Down { get; set; }

        /// <summary>
        /// Gets or sets the time when the check went down.
        /// </summary>
        [JsonPropertyName("down_since")]
        public DateTimeOffset? DownSince { get; set; }

        /// <summary>
        /// Gets or sets the time when the check came back up.
        /// </summary>
        [JsonPropertyName("up_since")]
        public DateTimeOffset? UpSince { get; set; }

        /// <summary>
        /// Gets or sets the last error message, if any.
        /// </summary>
        [JsonPropertyName("error")]
        public string? Error { get; set; }

        /// <summary>
        /// Gets or sets the check period in seconds.
        /// </summary>
        [JsonPropertyName("period")]
        public double? Period { get; set; }

        /// <summary>
        /// Gets or sets the Apdex threshold in seconds for performance monitoring.
        /// </summary>
        [JsonPropertyName("apdex_t")]
        public double? ApdexT { get; set; }

        /// <summary>
        /// Gets or sets the string to match in the response body.
        /// </summary>
        [JsonPropertyName("string_match")]
        public string? StringMatch { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this check is enabled.
        /// </summary>
        [JsonPropertyName("enabled")]
        public bool? Enabled { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this check is published on status page.
        /// </summary>
        [JsonPropertyName("published")]
        public bool? Published { get; set; }

        /// <summary>
        /// Gets or sets the list of disabled monitoring locations.
        /// </summary>
        [JsonPropertyName("disabled_locations")]
        public List<string>? DisabledLocations { get; set; }

        /// <summary>
        /// Gets or sets the list of recipient IDs to notify.
        /// </summary>
        [JsonPropertyName("recipients")]
        public List<string>? Recipients { get; set; }

        /// <summary>
        /// Gets or sets the time of the last check.
        /// </summary>
        [JsonPropertyName("last_check_at")]
        public DateTimeOffset? LastCheckAt { get; set; }

        /// <summary>
        /// Gets or sets the time of the next scheduled check.
        /// </summary>
        [JsonPropertyName("next_check_at")]
        public DateTimeOffset? NextCheckAt { get; set; }

        /// <summary>
        /// Gets or sets the time when this check was created.
        /// </summary>
        [JsonPropertyName("created_at")]
        public DateTimeOffset? CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets the mute until expression (e.g., "forever", specific time).
        /// </summary>
        [JsonPropertyName("mute_until")]
        public string? MuteUntil { get; set; }

        /// <summary>
        /// Gets or sets the favicon URL for this check.
        /// </summary>
        [JsonPropertyName("favicon_url")]
        public string? FavIconUrl { get; set; }

        /// <summary>
        /// Gets or sets the custom HTTP headers to send with requests.
        /// </summary>
        [JsonPropertyName("custom_headers")]
        public Dictionary<string, string>? CustomHeaders { get; set; }

        /// <summary>
        /// Gets or sets the HTTP verb/method to use (GET, POST, etc.).
        /// </summary>
        [JsonPropertyName("http_verb")]
        public string? HttpVerb { get; set; }

        /// <summary>
        /// Gets or sets the HTTP request body to send.
        /// </summary>
        [JsonPropertyName("http_body")]
        public string? HttpBody { get; set; }

        /// <summary>
        /// Gets or sets the SSL certificate information.
        /// </summary>
        [JsonPropertyName("ssl")]
        public Ssl? Ssl { get; set; }

        // Obsolete properties for backward compatibility
        /// <summary>
        /// Gets or sets the last HTTP status code received.
        /// </summary>
        [Obsolete("Use LastStatus instead.")]
        [JsonIgnore]
        public double? Last_Status
        {
            get => LastStatus;
            set => LastStatus = value;
        }

        /// <summary>
        /// Gets or sets the time when the check went down.
        /// </summary>
        [Obsolete("Use DownSince instead.")]
        [JsonIgnore]
        public DateTimeOffset? Down_Since
        {
            get => DownSince;
            set => DownSince = value;
        }

        /// <summary>
        /// Gets or sets the time when the check came back up.
        /// </summary>
        [Obsolete("Use UpSince instead.")]
        [JsonIgnore]
        public DateTimeOffset? Up_Since
        {
            get => UpSince;
            set => UpSince = value;
        }

        /// <summary>
        /// Gets or sets the Apdex threshold.
        /// </summary>
        [Obsolete("Use ApdexT instead.")]
        [JsonIgnore]
        public double? Apdex_T
        {
            get => ApdexT;
            set => ApdexT = value;
        }

        /// <summary>
        /// Gets or sets the string to match.
        /// </summary>
        [Obsolete("Use StringMatch instead.")]
        [JsonIgnore]
        public string? String_Match
        {
            get => StringMatch;
            set => StringMatch = value;
        }

        /// <summary>
        /// Gets or sets the disabled monitoring locations.
        /// </summary>
        [Obsolete("Use DisabledLocations instead.")]
        [JsonIgnore]
        public List<string>? Disabled_Locations
        {
            get => DisabledLocations;
            set => DisabledLocations = value;
        }

        /// <summary>
        /// Gets or sets the time of the last check.
        /// </summary>
        [Obsolete("Use LastCheckAt instead.")]
        [JsonIgnore]
        public DateTimeOffset? Last_Check_At
        {
            get => LastCheckAt;
            set => LastCheckAt = value;
        }

        /// <summary>
        /// Gets or sets the time of the next check.
        /// </summary>
        [Obsolete("Use NextCheckAt instead.")]
        [JsonIgnore]
        public DateTimeOffset? Next_Check_At
        {
            get => NextCheckAt;
            set => NextCheckAt = value;
        }

        /// <summary>
        /// Gets or sets the creation time.
        /// </summary>
        [Obsolete("Use CreatedAt instead.")]
        [JsonIgnore]
        public DateTimeOffset? Created_At
        {
            get => CreatedAt;
            set => CreatedAt = value;
        }

        /// <summary>
        /// Gets or sets the mute until value.
        /// </summary>
        [Obsolete("Use MuteUntil instead.")]
        [JsonIgnore]
        public string? Mute_Until
        {
            get => MuteUntil;
            set => MuteUntil = value;
        }

        /// <summary>
        /// Gets or sets the favicon URL.
        /// </summary>
        [Obsolete("Use FavIconUrl instead.")]
        [JsonIgnore]
        public string? FavIcon_Url
        {
            get => FavIconUrl;
            set => FavIconUrl = value;
        }

        /// <summary>
        /// Gets or sets the custom headers.
        /// </summary>
        [Obsolete("Use CustomHeaders instead.")]
        [JsonIgnore]
        public Dictionary<string, string>? Custom_Headers
        {
            get => CustomHeaders;
            set => CustomHeaders = value;
        }

        /// <summary>
        /// Gets or sets the HTTP verb.
        /// </summary>
        [Obsolete("Use HttpVerb instead.")]
        [JsonIgnore]
        public string? Http_Verb
        {
            get => HttpVerb;
            set => HttpVerb = value;
        }

        /// <summary>
        /// Gets or sets the HTTP body.
        /// </summary>
        [Obsolete("Use HttpBody instead.")]
        [JsonIgnore]
        public string? Http_Body
        {
            get => HttpBody;
            set => HttpBody = value;
        }
    }

    /// <summary>
    /// Represents SSL certificate information for a check.
    /// </summary>
    public class Ssl
    {
        /// <summary>
        /// Gets or sets the time when the SSL certificate was last tested.
        /// </summary>
        [JsonPropertyName("tested_at")]
        public DateTimeOffset? TestedAt { get; set; }

        /// <summary>
        /// Gets or sets the time when the SSL certificate expires.
        /// </summary>
        [JsonPropertyName("expires_at")]
        public DateTimeOffset? ExpiresAt { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the SSL certificate is valid.
        /// </summary>
        [JsonPropertyName("valid")]
        public bool? Valid { get; set; }

        /// <summary>
        /// Gets or sets the SSL certificate error message, if any.
        /// </summary>
        [JsonPropertyName("error")]
        public string? Error { get; set; }

        // Obsolete properties
        /// <summary>
        /// Gets or sets the tested at time.
        /// </summary>
        [Obsolete("Use TestedAt instead.")]
        [JsonIgnore]
        public DateTimeOffset? Tested_At
        {
            get => TestedAt;
            set => TestedAt = value;
        }

        /// <summary>
        /// Gets or sets the expires at time.
        /// </summary>
        [Obsolete("Use ExpiresAt instead.")]
        [JsonIgnore]
        public DateTimeOffset? Expires_At
        {
            get => ExpiresAt;
            set => ExpiresAt = value;
        }
    }

    /// <summary>
    /// Parameters for creating or updating a check.
    /// </summary>
    public class CheckParameters
    {
        /// <summary>
        /// Gets or sets the URL to monitor.
        /// </summary>
        [JsonPropertyName("url")]
        public string? Url { get; set; }

        /// <summary>
        /// Gets or sets the alias/name for the check.
        /// </summary>
        [JsonPropertyName("alias")]
        public string? Alias { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the check is down.
        /// </summary>
        [JsonPropertyName("down")]
        public bool? Down { get; set; }

        /// <summary>
        /// Gets or sets the check period in seconds.
        /// </summary>
        [JsonPropertyName("period")]
        public double? Period { get; set; }

        /// <summary>
        /// Gets or sets the Apdex threshold in seconds.
        /// </summary>
        [JsonPropertyName("apdex_t")]
        public double? ApdexT { get; set; }

        /// <summary>
        /// Gets or sets the string to match in the response.
        /// </summary>
        [JsonPropertyName("string_match")]
        public string? StringMatch { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the check is enabled.
        /// </summary>
        [JsonPropertyName("enabled")]
        public bool? Enabled { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the check is published.
        /// </summary>
        [JsonPropertyName("published")]
        public bool? Published { get; set; }

        /// <summary>
        /// Gets or sets the disabled monitoring locations.
        /// </summary>
        [JsonPropertyName("disabled_locations")]
        public List<string>? DisabledLocations { get; set; }

        /// <summary>
        /// Gets or sets the recipient IDs.
        /// </summary>
        [JsonPropertyName("recipients")]
        public List<string>? Recipients { get; set; }

        /// <summary>
        /// Gets or sets the mute until value.
        /// </summary>
        [JsonPropertyName("mute_until")]
        public string? MuteUntil { get; set; }

        /// <summary>
        /// Gets or sets the favicon URL.
        /// </summary>
        [JsonPropertyName("favicon_url")]
        public string? FavIconUrl { get; set; }

        /// <summary>
        /// Gets or sets the custom HTTP headers.
        /// </summary>
        [JsonPropertyName("custom_headers")]
        public Dictionary<string, string>? CustomHeaders { get; set; }

        /// <summary>
        /// Gets or sets the HTTP verb/method.
        /// </summary>
        [JsonPropertyName("http_verb")]
        public string? HttpVerb { get; set; }

        /// <summary>
        /// Gets or sets the HTTP request body.
        /// </summary>
        [JsonPropertyName("http_body")]
        public string? HttpBody { get; set; }

        // Obsolete properties
        /// <summary>
        /// Gets or sets the Apdex threshold.
        /// </summary>
        [Obsolete("Use ApdexT instead.")]
        [JsonIgnore]
        public double? Apdex_T
        {
            get => ApdexT;
            set => ApdexT = value;
        }

        /// <summary>
        /// Gets or sets the string match value.
        /// </summary>
        [Obsolete("Use StringMatch instead.")]
        [JsonIgnore]
        public string? String_Match
        {
            get => StringMatch;
            set => StringMatch = value;
        }

        /// <summary>
        /// Gets or sets the disabled locations.
        /// </summary>
        [Obsolete("Use DisabledLocations instead.")]
        [JsonIgnore]
        public List<string>? Disabled_Locations
        {
            get => DisabledLocations;
            set => DisabledLocations = value;
        }

        /// <summary>
        /// Gets or sets the mute until value.
        /// </summary>
        [Obsolete("Use MuteUntil instead.")]
        [JsonIgnore]
        public string? Mute_Until
        {
            get => MuteUntil;
            set => MuteUntil = value;
        }

        /// <summary>
        /// Gets or sets the favicon URL.
        /// </summary>
        [Obsolete("Use FavIconUrl instead.")]
        [JsonIgnore]
        public string? FavIcon_Url
        {
            get => FavIconUrl;
            set => FavIconUrl = value;
        }

        /// <summary>
        /// Gets or sets the custom headers.
        /// </summary>
        [Obsolete("Use CustomHeaders instead.")]
        [JsonIgnore]
        public Dictionary<string, string>? Custom_Headers
        {
            get => CustomHeaders;
            set => CustomHeaders = value;
        }

        /// <summary>
        /// Gets or sets the HTTP verb.
        /// </summary>
        [Obsolete("Use HttpVerb instead.")]
        [JsonIgnore]
        public string? Http_Verb
        {
            get => HttpVerb;
            set => HttpVerb = value;
        }

        /// <summary>
        /// Gets or sets the HTTP body.
        /// </summary>
        [Obsolete("Use HttpBody instead.")]
        [JsonIgnore]
        public string? Http_Body
        {
            get => HttpBody;
            set => HttpBody = value;
        }
    }
}
