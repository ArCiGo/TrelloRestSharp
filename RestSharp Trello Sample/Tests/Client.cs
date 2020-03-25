using NUnit.Framework;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace RestSharp_Trello_Sample.Tests
{
    public class Client
    {
        public RestClient client;

        [SetUp]
        public void SetUp()
        {
            string baseURL = "https://api.trello.com/1";
            client = new RestClient(baseURL);
        }
    }
}
