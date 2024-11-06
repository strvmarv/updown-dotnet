using System;
using System.Collections.Generic;

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

        public string Token { get; set; }
        public string? Url { get; set; }
        public string? Alias { get; set; }
        public double? Last_Status { get; set; }
        public double? Uptime { get; set; }
        public bool? Down { get; set; }
        public DateTimeOffset? Down_Since { get; set; }
        public DateTimeOffset? Up_Since { get; set; }
        public string? Error { get; set; }
        public double? Period { get; set; }
        public double? Apdex_T { get; set; }
        public string? String_Match { get; set; }
        public bool? Enabled { get; set; }
        public bool? Published { get; set; }
        public List<string>? Disabled_Locations { get; set; }
        public List<string>? Recipients { get; set; }
        public DateTimeOffset? Last_Check_At { get; set; }
        public DateTimeOffset? Next_Check_At { get; set; }
        public DateTimeOffset? Created_At { get; set; }
        public string? Mute_Until { get; set; }
        public string? FavIcon_Url { get; set; }
        public Custom_Headers? Custom_Headers { get; set; }
        public string? Http_Verb { get; set; }
        public string? Http_Body { get; set; }
        public Ssl? Ssl { get; set; }
    }

    public class Custom_Headers
    {
    }

    public class Ssl
    {
        public DateTimeOffset? Tested_At { get; set; }
        public DateTimeOffset? Expires_At { get; set; }
        public bool? Valid { get; set; }
        public string? Error { get; set; }
    }
}
