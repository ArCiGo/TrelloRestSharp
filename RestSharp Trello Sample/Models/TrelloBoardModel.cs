using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace RestSharp_Trello_Sample
{
    public class TrelloBoardModel
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("closed")]
        public bool Closed { get; set; }

        [JsonProperty("prefs")]
        public Dictionary<string, dynamic> Prefs { get; set; }

        [JsonProperty("_value")]
        public object? _Value { get; set; }

        [JsonProperty("limits")]
        public dynamic? Limits { get; set; }
    }
}
