using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using RestSharp;
using RestSharp.Serialization.Json;
using RestSharp_Trello_Sample.Tests;

namespace RestSharp_Trello_Sample
{
    [TestFixture]
    class EntryPoint
    {
        public Client trelloClient = new Client();
        public JsonDeserializer deserializer = new JsonDeserializer();

        [Test, Order(1)]
        public void CreateBoard()
        {
            string boardName = "My Amazing Board";

            IRestResponse createBoardResponse = trelloClient.createBoard(boardName);
            TrelloBoardModel values = deserializer.Deserialize<TrelloBoardModel>(createBoardResponse);

            trelloClient.SetBoardId(values.Id);
            trelloClient.SetBoardName(values.Name);

            Assert.AreEqual(HttpStatusCode.OK, createBoardResponse.StatusCode);
            Assert.AreEqual(boardName, values.Name);
            Assert.AreEqual("private", values.Prefs["permissionLevel"]);
            Assert.False(values.Closed);
        }

        [Test, Order(2)]
        public void GetBoard()
        {
            IRestResponse getBoardResponse = trelloClient.getBoard(trelloClient.GetBoardId());
            TrelloBoardModel values = deserializer.Deserialize<TrelloBoardModel>(getBoardResponse);

            Assert.AreEqual(HttpStatusCode.OK, getBoardResponse.StatusCode);
        }

        [Test, Order(3)]
        public void CreateList()
        {
            string listName = "My Amazing List";

            IRestResponse createListResponse = trelloClient.createList(trelloClient.GetBoardId(), listName);
            TrelloListBasicModel values = deserializer.Deserialize<TrelloListBasicModel>(createListResponse);

            trelloClient.SetListId(values.Id);
            trelloClient.SetListName(values.Name);

            Assert.AreEqual(HttpStatusCode.OK, createListResponse.StatusCode);
            Assert.AreEqual(listName, values.Name);
            Assert.False(values.Closed);
        }

        [Test, Order(4)]
        public void AddingACard()
        {
            string cardName = "My Amazing Card";
            IRestResponse addCardResponse = trelloClient.addCardToList(trelloClient.GetBoardId(), cardName, trelloClient.GetListId());

            TrelloCardBasicModel values = deserializer.Deserialize<TrelloCardBasicModel>(addCardResponse);

            trelloClient.SetCardId(values.Id);
            trelloClient.SetCardName(values.Name);

            Assert.AreEqual(HttpStatusCode.OK, addCardResponse.StatusCode);
            Assert.AreEqual(cardName, values.Name);
            Assert.False(values.Closed);
            Assert.AreEqual(trelloClient.GetListId(), values.IdList);
            Assert.AreEqual(trelloClient.GetBoardId(), values.IdBoard);
            Assert.Zero(values.Badges[0].Votes);
            Assert.Zero(values.Badges[0].Attachments);
        }

        [Test, Order(5)]
        public void UpdateCard()
        {
            string cardName = "My Amazing Card Updated!";
            Dictionary<string, string> extraParams = new Dictionary<string, string>
            {
                { "name", cardName }
            };

            IRestResponse updateCardResponse = trelloClient.updateCard(trelloClient.GetBoardId(), trelloClient.GetListId(), trelloClient.GetCardId(), extraParams);
            TrelloCardBasicModel values = deserializer.Deserialize<TrelloCardBasicModel>(updateCardResponse);

            Assert.AreEqual(HttpStatusCode.OK, updateCardResponse.StatusCode);
            Assert.AreEqual(cardName, values.Name);
            Assert.False(values.Closed);
            Assert.AreEqual(trelloClient.GetListId(), values.IdList);
            Assert.AreEqual(trelloClient.GetBoardId(), values.IdBoard);
            Assert.Zero(values.Badges[0].Votes);
            Assert.Zero(values.Badges[0].Attachments);
        }

        [Test, Order(6)]
        public void DeleteBoard()
        {
            IRestResponse deleteBoardResponse = trelloClient.deleteBoard(trelloClient.GetBoardId());

            TrelloBoardModel values = deserializer.Deserialize<TrelloBoardModel>(deleteBoardResponse);

            Assert.AreEqual(HttpStatusCode.OK, deleteBoardResponse.StatusCode);
            Assert.IsNull(values._Value);
        }

        [Test, Order(7)]
        public void GetBoardAfterDeleteIt()
        {
            string messageNotFound = "The requested resource was not found.";

            IRestResponse getBoardResponse = trelloClient.getBoard(trelloClient.GetBoardId());

            Assert.AreEqual(HttpStatusCode.NotFound, getBoardResponse.StatusCode);
            Assert.AreEqual(messageNotFound, getBoardResponse.Content);
        }
    }
}
