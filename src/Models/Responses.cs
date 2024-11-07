using System.Text.Json.Serialization;

namespace UpdownDotnet.Models
{
    public class CheckDeleteResponse
    {
        [JsonPropertyName("deleted")]
        public bool Deleted { get; set; }
    }
}
