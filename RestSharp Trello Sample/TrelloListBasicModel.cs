using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace RestSharp_Trello_Sample
{
    public class TrelloListBasicModel
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("closed")]
        public bool Closed { get; set; }

        [JsonProperty("idBoard")]
        public string IdBoard { get; set; }
    }
}
