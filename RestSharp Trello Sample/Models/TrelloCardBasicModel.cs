using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace RestSharp_Trello_Sample
{
    class TrelloCardBasicModel
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("closed")]
        public bool Closed { get; set; }

        [JsonProperty("idList")]
        public string IdList { get; set; }

        [JsonProperty("idBoard")]
        public string IdBoard { get; set; }

        //[JsonProperty("badges")]
        //public Dictionary<string, dynamic>? Badges { get; set; }

        [JsonProperty("badges")]
        public dynamic? Badges { get; set; }
    }
}
