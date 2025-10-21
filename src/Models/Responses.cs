using System.Text.Json.Serialization;

namespace UpdownDotnet.Models
{
    /// <summary>
    /// Represents a delete operation response.
    /// </summary>
    public class DeleteResponse
    {
        /// <summary>
        /// Gets or sets a value indicating whether the resource was deleted.
        /// </summary>
        [JsonPropertyName("deleted")]
        public bool Deleted { get; set; }
    }
}
