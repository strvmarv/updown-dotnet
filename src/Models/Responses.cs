using System.Text.Json.Serialization;

namespace UpdownDotnet.Models
{
    public class DeleteResponse
    {
        [JsonPropertyName("deleted")]
        public bool Deleted { get; set; }
    }
}
