using System.Text.Json.Serialization;

namespace UpdownDotnet.Models
{
    /// <summary>
    /// Represents an Updown.io notification recipient.
    /// </summary>
    public class Recipient
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Recipient"/> class.
        /// </summary>
        public Recipient() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Recipient"/> class from parameters.
        /// </summary>
        /// <param name="parameters">The recipient parameters.</param>
        public Recipient(RecipientParameters parameters)
        {
            Type = parameters.Type;
            Name = parameters.Name;
            Value = parameters.Value;
        }

        /// <summary>
        /// Gets or sets the unique recipient ID.
        /// </summary>
        [JsonPropertyName("id")]
        public string? Id { get; set; }

        /// <summary>
        /// Gets or sets the recipient type (email, slack, webhook, etc.).
        /// </summary>
        [JsonPropertyName("type")]
        public string? Type { get; set; }

        /// <summary>
        /// Gets or sets the recipient name/description.
        /// </summary>
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        /// <summary>
        /// Gets or sets the recipient value (email address, webhook URL, etc.).
        /// </summary>
        [JsonPropertyName("value")]
        public string? Value { get; set; }
    }

    /// <summary>
    /// Parameters for creating a recipient.
    /// </summary>
    public class RecipientParameters
    {
        /// <summary>
        /// Gets or sets the recipient type (email, slack, webhook, etc.).
        /// </summary>
        [JsonPropertyName("type")]
        public string? Type { get; set; }

        /// <summary>
        /// Gets or sets the recipient name/description.
        /// </summary>
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        /// <summary>
        /// Gets or sets the recipient value (email address, webhook URL, etc.).
        /// </summary>
        [JsonPropertyName("value")]
        public string? Value { get; set; }
    }
}
