using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace UpdownDotnet.Models
{
    public class Check
    {
        public Check() { }

        public Check(CheckParameters parameters)
        {
            Url = parameters.Url;
            Alias = parameters.Alias;
            Down = parameters.Down;
            Period = parameters.Period;
            Apdex_T = parameters.Apdex_T;
            String_Match = parameters.String_Match;
            Enabled = parameters.Enabled;
            Published = parameters.Published;
            Disabled_Locations = parameters.Disabled_Locations;
            Recipients = parameters.Recipients;
            Mute_Until = parameters.Mute_Until;
            FavIcon_Url = parameters.FavIcon_Url;
            Custom_Headers = parameters.Custom_Headers;
            Http_Verb = parameters.Http_Verb;
            Http_Body = parameters.Http_Body;
        }

        [JsonPropertyName("token")]
        public string Token { get; set; }
        [JsonPropertyName("url")]
        public string Url { get; set; }
        [JsonPropertyName("alias")]
        public string Alias { get; set; }
        [JsonPropertyName("last_status")]
        public double? Last_Status { get; set; }
        [JsonPropertyName("uptime")]
        public double? Uptime { get; set; }
        [JsonPropertyName("down")]
        public bool? Down { get; set; }
        [JsonPropertyName("down_since")]
        public DateTimeOffset? Down_Since { get; set; }
        [JsonPropertyName("up_since")]
        public DateTimeOffset? Up_Since { get; set; }
        [JsonPropertyName("error")]
        public string Error { get; set; }
        [JsonPropertyName("period")]
        public double? Period { get; set; }
        [JsonPropertyName("apdex_t")]
        public double? Apdex_T { get; set; }
        [JsonPropertyName("string_match")]
        public string String_Match { get; set; }
        [JsonPropertyName("enabled")]
        public bool? Enabled { get; set; }
        [JsonPropertyName("published")]
        public bool? Published { get; set; }
        [JsonPropertyName("disabled_locations")]
        public List<string> Disabled_Locations { get; set; }
        [JsonPropertyName("recipients")]
        public List<string> Recipients { get; set; }
        [JsonPropertyName("last_check_at")]
        public DateTimeOffset? Last_Check_At { get; set; }
        [JsonPropertyName("next_check_at")]
        public DateTimeOffset? Next_Check_At { get; set; }
        [JsonPropertyName("created_at")]
        public DateTimeOffset? Created_At { get; set; }
        [JsonPropertyName("mute_until")]
        public string Mute_Until { get; set; }
        [JsonPropertyName("favicon_url")]
        public string FavIcon_Url { get; set; }
        [JsonPropertyName("custom_headers")]
        public Custom_Headers Custom_Headers { get; set; }
        [JsonPropertyName("http_verb")]
        public string Http_Verb { get; set; }
        [JsonPropertyName("http_body")]
        public string Http_Body { get; set; }
        [JsonPropertyName("ssl")]
        public Ssl Ssl { get; set; }
    }

    public class Custom_Headers
    {
        [JsonPropertyName("User-Agent")]
        public string UserAgent { get; set; }
        [JsonPropertyName("Accept")]
        public string Accept { get; set; }
        [JsonPropertyName("Connection")]
        public string Connection { get; set; }
        [JsonPropertyName("Accept-Language")]
        public string Accept_Language { get; set; }
        [JsonPropertyName("Accept_Encoding")]
        public string Accept_Encoding { get; set; }
    }

    public class Ssl
    {
        [JsonPropertyName("tested_at")]
        public DateTimeOffset? Tested_At { get; set; }
        [JsonPropertyName("expires_at")]
        public DateTimeOffset? Expires_At { get; set; }
        [JsonPropertyName("valid")]
        public bool? Valid { get; set; }
        [JsonPropertyName("error")]
        public string Error { get; set; }
    }

    public class CheckParameters
    {
        [JsonPropertyName("url")]
        public string Url { get; set; }
        [JsonPropertyName("alias")]
        public string Alias { get; set; }
        [JsonPropertyName("down")]
        public bool? Down { get; set; }
        [JsonPropertyName("period")]
        public double? Period { get; set; }
        [JsonPropertyName("apdex_t")]
        public double? Apdex_T { get; set; }
        [JsonPropertyName("string_match")]
        public string String_Match { get; set; }
        [JsonPropertyName("enabled")]
        public bool? Enabled { get; set; }
        [JsonPropertyName("published")]
        public bool? Published { get; set; }
        [JsonPropertyName("disabled_locations")]
        public List<string> Disabled_Locations { get; set; }
        [JsonPropertyName("recipients")]
        public List<string> Recipients { get; set; }
        [JsonPropertyName("mute_until")]
        public string Mute_Until { get; set; }
        [JsonPropertyName("favicon_url")]
        public string FavIcon_Url { get; set; }
        [JsonPropertyName("custom_headers")]
        public Custom_Headers Custom_Headers { get; set; }
        [JsonPropertyName("http_verb")]
        public string Http_Verb { get; set; }
        [JsonPropertyName("http_body")]
        public string Http_Body { get; set; }
    }
}
