using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using NUnit.Framework;
using RestSharp;
using RestSharp.Serialization.Json;
using RestSharp_Trello_Sample.Tests;

namespace RestSharp_Trello_Sample
{
    [TestFixture]
    class EntryPoint
    {
        public string boardId = "";
        public string boardName = "";
        public string listId = "";
        public string listName = "";
        public string cardId = "";
        public string cardName = "";

        public Client trelloClient = new Client();
        public JsonDeserializer deserializer = new JsonDeserializer();

        [Test, Order(1)]
        public void CreateBoard()
        {
            string boardName = "My Amazing Board";
            
            IRestResponse createBoardResponse = trelloClient.createBoard(boardName);
            TrelloBoardModel values = deserializer.Deserialize<TrelloBoardModel>(createBoardResponse);

            boardId = values.Id;
            this.boardName = values.Name;

            Assert.AreEqual(HttpStatusCode.OK, createBoardResponse.StatusCode);
            Assert.AreEqual(boardName, values.Name);
            Assert.AreEqual("private", values.Prefs["permissionLevel"]);
            Assert.False(values.Closed);
        }

        [Test, Order(2)]
        public void CreateList()
        {
            string listName = "My Amazing List";

            IRestResponse createListResponse = trelloClient.createList(boardId, listName);
            TrelloListBasicModel values = deserializer.Deserialize<TrelloListBasicModel>(createListResponse);

            listId = values.Id;
            this.listName = values.Name;

            Assert.AreEqual(HttpStatusCode.OK, createListResponse.StatusCode);
            Assert.AreEqual(listName, values.Name);
            Assert.False(values.Closed);
        }

        [Test, Order(3)]
        public void AddingACard()
        {
            string carName = "My Amazing Card";
            IRestResponse addCardResponse = trelloClient.addCardToList(boardId, cardName, listId);

            TrelloCardBasicModel values = deserializer.Deserialize<TrelloCardBasicModel>(addCardResponse);

            cardId = values.Id;
            this.cardName = values.Name;

            Assert.AreEqual(HttpStatusCode.OK, addCardResponse.StatusCode);
            Assert.AreEqual(cardName, values.Name);
            Assert.False(values.Closed);
            Assert.AreEqual(listId, values.IdList);
            Assert.AreEqual(boardId, values.IdBoard);
            Assert.Zero(values.Badges[0].Votes);
            Assert.Zero(values.Badges[0].Attachments);
        }

        [Test, Order(4)]
        public void UpdateCard()
        {
            string cardName = "My Amazing Card Updated!";
            Dictionary<string, string> extraParams = new Dictionary<string, string>
            {
                { "name", cardName }
            };

            IRestResponse updateCardResponse = trelloClient.updateCard(boardId, listId, cardId, extraParams);
            TrelloCardBasicModel values = deserializer.Deserialize<TrelloCardBasicModel>(updateCardResponse);

            Assert.AreEqual(HttpStatusCode.OK, updateCardResponse.StatusCode);
            Assert.AreEqual(cardName, values.Name);
            Assert.False(values.Closed);
            Assert.AreEqual(listId, values.IdList);
            Assert.AreEqual(boardId, values.IdBoard);
            Assert.Zero(values.Badges[0].Votes);
            Assert.Zero(values.Badges[0].Attachments);
        }

        [Test, Order(5)]
        public void DeleteBoard()
        {
            IRestResponse deleteBoardResponse = trelloClient.deleteBoard(boardId);

            TrelloBoardModel values = deserializer.Deserialize<TrelloBoardModel>(deleteBoardResponse);

            Assert.AreEqual(HttpStatusCode.OK, deleteBoardResponse.StatusCode);
            Assert.IsNull(values._Value);
        }

        [Test, Order(6)]
        public void GetBoardAfterDeleteIt()
        {
            string messageNotFound = "The requested resource was not found.";

            IRestResponse getBoardResponse = trelloClient.getBoard(boardId);

            Assert.AreEqual(HttpStatusCode.NotFound, getBoardResponse.StatusCode);
            Assert.AreEqual(messageNotFound, getBoardResponse.Content);
        }
    }
}
