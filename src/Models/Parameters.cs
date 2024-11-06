using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace UpdownDotnet.Models
{
    public class CheckParameters
    {
        //[JsonPropertyName("token")]
        //public string Token { get; set; }
        [JsonPropertyName("url")]
        public string Url { get; set; }
        [JsonPropertyName("alias")]
        public string? Alias { get; set; }
        [JsonPropertyName("down")]
        public bool? Down { get; set; }
        [JsonPropertyName("period")]
        public double? Period { get; set; }
        [JsonPropertyName("apdex_t")]
        public double? Apdex_T { get; set; }
        [JsonPropertyName("string_match")]
        public string? String_Match { get; set; }
        [JsonPropertyName("enabled")]
        public bool? Enabled { get; set; }
        [JsonPropertyName("published")]
        public bool? Published { get; set; }
        [JsonPropertyName("disabled_locations")]
        public List<string>? Disabled_Locations { get; set; }
        [JsonPropertyName("recipients")]
        public List<string>? Recipients { get; set; }
        [JsonPropertyName("mute_until")]
        public string? Mute_Until { get; set; }
        [JsonPropertyName("favicon_url")]
        public string? FavIcon_Url { get; set; }
        [JsonPropertyName("custom_headers")]
        public Custom_Headers? Custom_Headers { get; set; }
        [JsonPropertyName("http_verb")]
        public string? Http_Verb { get; set; }
        [JsonPropertyName("http_body")]
        public string? Http_Body { get; set; }
    }
}
