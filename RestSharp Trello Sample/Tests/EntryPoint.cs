using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using NUnit.Framework;
using RestSharp;
using RestSharp.Serialization.Json;

namespace RestSharp_Trello_Sample
{
    [TestFixture]
    class EntryPoint
    {
        public string trelloKey = "";
        public string trelloToken = "";

        public string boardId = "";
        public string boardName = "";
        public string listId = "";
        public string listName = "";
        public string cardId = "";
        public string cardName = "";

        [Test, Order(1)]
        public void CreateBoard()
        {
            RestClient client = new RestClient("https://api.trello.com/1");
            IRestRequest createBoardRequest = new RestRequest("/boards");
            
            createBoardRequest.Method = Method.POST;
            createBoardRequest.AddParameter("name", "My Amazing Board");
            createBoardRequest.AddParameter("key", trelloKey);
            createBoardRequest.AddParameter("token", trelloToken);
            createBoardRequest.AddParameter("defaultLists", "false");

            IRestResponse createResponse = client.Execute(createBoardRequest);
            Console.WriteLine(createResponse.Content);

            TrelloBoardBasicModel values = new JsonDeserializer().Deserialize<TrelloBoardBasicModel>(createResponse);
            
            boardId = values.Id;
            boardName = values.Name;

            Assert.AreEqual(HttpStatusCode.OK, createResponse.StatusCode);
            Assert.AreEqual("My Amazing Board", values.Name);
            Assert.AreEqual("private", values.Prefs[0].PermissionLevel);
            Assert.False(values.Closed);
        }

        [Test, Order(2)]
        public void CreateList()
        {
            RestClient client = new RestClient("https://api.trello.com/1");
            IRestRequest createListRequest = new RestRequest("/boards/" + boardId + "/lists");

            createListRequest.Method = Method.POST;
            createListRequest.AddParameter("name", "My Amazing List");
            createListRequest.AddParameter("key", trelloKey);
            createListRequest.AddParameter("token", trelloToken);

            IRestResponse createResponse = client.Execute(createListRequest);

            TrelloListBasicModel values = new JsonDeserializer().Deserialize<TrelloListBasicModel>(createResponse);

            listId = values.Id;
            listName = values.Name;

            Assert.AreEqual(HttpStatusCode.OK, createResponse.StatusCode);
            Assert.AreEqual("My Amazing List", values.Name);
            Assert.False(values.Closed);
        }

        [Test, Order(3)]
        public void AddingACard()
        {
            RestClient client = new RestClient("https://api.trello.com/1");
            IRestRequest createCardRequest = new RestRequest("/cards");

            createCardRequest.Method = Method.POST;
            createCardRequest.AddParameter("name", "My Amazing Card");
            createCardRequest.AddParameter("idList", listId);
            createCardRequest.AddParameter("keepFromSource", "all");
            createCardRequest.AddParameter("key", trelloKey);
            createCardRequest.AddParameter("token", trelloToken);

            IRestResponse createResponse = client.Execute(createCardRequest);

            TrelloCardBasicModel values = new JsonDeserializer().Deserialize<TrelloCardBasicModel>(createResponse);

            cardId = values.Id;
            cardName = values.Name;

            Assert.AreEqual(HttpStatusCode.OK, createResponse.StatusCode);
            Assert.AreEqual("My Amazing Card", values.Name);
            Assert.AreEqual(false, values.Closed);
            Assert.AreEqual(listId, values.IdList);
            Assert.AreEqual(boardId, values.IdBoard);
            Assert.Zero(values.Badges[0].Votes);
            Assert.Zero(values.Badges[0].Attachments);
        }

        [Test, Order(4)]
        public void UpdateCard()
        {
            RestClient client = new RestClient("https://api.trello.com/1");
            IRestRequest updateCardRequest = new RestRequest("/cards/" + cardId);

            updateCardRequest.Method = Method.PUT;
            updateCardRequest.AddParameter("name", "My Amazing Card Updated!");
            updateCardRequest.AddParameter("idList", listId);
            updateCardRequest.AddParameter("token", trelloToken);
            updateCardRequest.AddParameter("key", trelloKey);
            updateCardRequest.AddParameter("idBoard", boardId);

            IRestResponse createResponse = client.Execute(updateCardRequest);

            TrelloCardBasicModel values = new JsonDeserializer().Deserialize<TrelloCardBasicModel>(createResponse);

            Assert.AreEqual(HttpStatusCode.OK, createResponse.StatusCode);
            Assert.AreEqual("My Amazing Card Updated!", values.Name);
            Assert.AreEqual(false, values.Closed);
            Assert.AreEqual(listId, values.IdList);
            Assert.AreEqual(boardId, values.IdBoard);
            Assert.Zero(values.Badges[0].Votes);
            Assert.Zero(values.Badges[0].Attachments);
        }

        [Test, Order(5)]
        public void DeleteBoard()
        {
            RestClient client = new RestClient("https://api.trello.com/1");
            IRestRequest deleteBoardRequest = new RestRequest("/boards/" + boardId);

            deleteBoardRequest.Method = Method.DELETE;
            deleteBoardRequest.AddParameter("key", trelloKey);
            deleteBoardRequest.AddParameter("token", trelloToken);

            IRestResponse createResponse = client.Execute(deleteBoardRequest);

            TrelloBoardBasicModel values = new JsonDeserializer().Deserialize<TrelloBoardBasicModel>(createResponse);

            Assert.IsNull(values._Value);
        }

        [Test, Order(6)]
        public void GetBoard()
        {
            RestClient client = new RestClient("https://api.trello.com/1");
            IRestRequest getBoardRequest = new RestRequest("/boards/" + boardId);

            string messageNotFound = "The requested resource was not found.";

            getBoardRequest.Method = Method.GET;
            getBoardRequest.AddParameter("key", trelloKey);
            getBoardRequest.AddParameter("token", trelloToken);

            IRestResponse createResponse = client.Execute(getBoardRequest);

            Assert.AreEqual(HttpStatusCode.NotFound, createResponse.StatusCode);
            Assert.AreEqual(messageNotFound, createResponse.Content);
        }
    }
}
