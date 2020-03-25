using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace RestSharp_Trello_Sample
{
    public class TrelloBoardPrefsModel
    {
        [JsonProperty("permissionLevel")]
        public string PermissionLevel { get; set; }

        [JsonProperty("hideVotes")]
        public string HideVotes { get; set; }

        [JsonProperty("voting")]
        public string Voting { get; set; }
    }
}
