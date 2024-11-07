using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace UpdownDotnet.Models
{
    public class StatusPage
    {
        public StatusPage() { }

        public StatusPage(StatusPageParameters parameters)
        {
            Name = parameters.Name;
            Description = parameters.Description;
            Visibility = parameters.Visibility;
            Access_Key = parameters.Access_Key;
            Checks = parameters.Checks;
        }

        [JsonPropertyName("token")]
        public string Token { get; set; }
        [JsonPropertyName("url")]
        public string Url { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("description")]
        public string Description { get; set; }
        [JsonPropertyName("visibility")]
        public string Visibility { get; set; }
        [JsonPropertyName("access_key")]
        public string Access_Key { get; set; }
        [JsonPropertyName("checks")]
        public List<string> Checks { get; set; }
    }

    public class StatusPageParameters
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("description")]
        public string Description { get; set; }
        [JsonPropertyName("visibility")]
        public string Visibility { get; set; }
        [JsonPropertyName("access_key")]
        public string Access_Key { get; set; }
        [JsonPropertyName("checks")]
        public List<string> Checks { get; set; }
    }
}
