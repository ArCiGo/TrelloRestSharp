using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace RestSharp_Trello_Sample.Utilities
{
    public class Utilities
    {
        public static void AddExtraParams(Dictionary<string, string> extraParams, IRestRequest request)
        {
            if (extraParams != null)
            {
                foreach (KeyValuePair<string, string> param in extraParams)
                {
                    request.AddParameter(param.Key, param.Value);
                }
            }
        }
    }
}
