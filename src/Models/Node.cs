using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace UpdownDotnet.Models
{
    /// <summary>
    /// Represents an Updown.io monitoring node/location.
    /// </summary>
    public class Node
    {
        /// <summary>
        /// Gets or sets the name of the monitoring node.
        /// </summary>
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        /// <summary>
        /// Gets or sets the city where the node is located.
        /// </summary>
        [JsonPropertyName("city")]
        public string? City { get; set; }

        /// <summary>
        /// Gets or sets the country where the node is located.
        /// </summary>
        [JsonPropertyName("country")]
        public string? Country { get; set; }

        /// <summary>
        /// Gets or sets the country code (ISO 3166-1 alpha-2).
        /// </summary>
        [JsonPropertyName("country_code")]
        public string? CountryCode { get; set; }

        /// <summary>
        /// Gets or sets the latitude coordinate of the node.
        /// </summary>
        [JsonPropertyName("lat")]
        public double? Latitude { get; set; }

        /// <summary>
        /// Gets or sets the longitude coordinate of the node.
        /// </summary>
        [JsonPropertyName("lng")]
        public double? Longitude { get; set; }

        /// <summary>
        /// Gets or sets the IPv4 address of the node.
        /// </summary>
        [JsonPropertyName("ip")]
        public string? IpAddress { get; set; }

        /// <summary>
        /// Gets or sets the IPv6 address of the node.
        /// </summary>
        [JsonPropertyName("ip6")]
        public string? Ipv6Address { get; set; }

        // Obsolete properties
        /// <summary>
        /// Gets or sets the country code.
        /// </summary>
        [System.Obsolete("Use CountryCode instead.")]
        [JsonIgnore]
        public string? Country_Code
        {
            get => CountryCode;
            set => CountryCode = value;
        }

        /// <summary>
        /// Gets or sets the IPv4 address.
        /// </summary>
        [System.Obsolete("Use IpAddress instead.")]
        [JsonIgnore]
        public string? Ip
        {
            get => IpAddress;
            set => IpAddress = value;
        }

        /// <summary>
        /// Gets or sets the IPv6 address.
        /// </summary>
        [System.Obsolete("Use Ipv6Address instead.")]
        [JsonIgnore]
        public string? Ip6
        {
            get => Ipv6Address;
            set => Ipv6Address = value;
        }
    }

    /// <summary>
    /// Represents a collection of node IPv4 addresses.
    /// </summary>
    public class NodeIpv4Addresses
    {
        /// <summary>
        /// Gets or sets the list of IPv4 addresses.
        /// </summary>
        [JsonPropertyName("ipv4")]
        public List<string>? Ipv4 { get; set; }
    }

    /// <summary>
    /// Represents a collection of node IPv6 addresses.
    /// </summary>
    public class NodeIpv6Addresses
    {
        /// <summary>
        /// Gets or sets the list of IPv6 addresses.
        /// </summary>
        [JsonPropertyName("ipv6")]
        public List<string>? Ipv6 { get; set; }
    }
}

