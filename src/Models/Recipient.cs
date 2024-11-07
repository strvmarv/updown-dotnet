using System.Text.Json.Serialization;

namespace UpdownDotnet.Models
{
    public class Recipient
    {
        public Recipient() { }

        public Recipient(RecipientParameters parameters)
        {
            Type = parameters.Type;
            Name = parameters.Name;
            Value = parameters.Value;
        }

        [JsonPropertyName("id")]
        public string Id { get; set; }
        [JsonPropertyName("type")]
        public string Type { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("value")]
        public string Value { get; set; }
    }

    public class RecipientParameters
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("value")]
        public string Value { get; set; }
    }
}
