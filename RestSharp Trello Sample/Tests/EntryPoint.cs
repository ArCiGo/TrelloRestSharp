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

            IRestResponse createBoardResponse = trelloClient.CreateBoard(boardName);
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
            IRestResponse getBoardResponse = trelloClient.GetBoard(trelloClient.GetBoardId());
            TrelloBoardModel values = deserializer.Deserialize<TrelloBoardModel>(getBoardResponse);

            Console.WriteLine(getBoardResponse.Content);

            Assert.AreEqual(HttpStatusCode.OK, getBoardResponse.StatusCode);
            Assert.AreEqual(trelloClient.GetBoardId(), values.Id);
            Assert.AreEqual(trelloClient.GetBoardName(), values.Name);
        }

        [Test, Order(3)]
        public void CreateList()
        {
            string listName = "My Amazing List";

            IRestResponse createListResponse = trelloClient.CreateList(trelloClient.GetBoardId(), listName);
            TrelloListModel values = deserializer.Deserialize<TrelloListModel>(createListResponse);

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
            IRestResponse addCardResponse = trelloClient.AddCardToList(trelloClient.GetBoardId(), cardName, trelloClient.GetListId());

            TrelloCardModel values = deserializer.Deserialize<TrelloCardModel>(addCardResponse);

            trelloClient.SetCardId(values.Id);
            trelloClient.SetCardName(values.Name);

            Assert.AreEqual(HttpStatusCode.OK, addCardResponse.StatusCode);
            Assert.AreEqual(cardName, values.Name);
            Assert.False(values.Closed);
            Assert.AreEqual(trelloClient.GetListId(), values.IdList);
            Assert.AreEqual(trelloClient.GetBoardId(), values.IdBoard);
            Assert.Zero(values.Badges["votes"]);
            Assert.Zero(values.Badges["attachments"]);

            /* Getting nested JSON objects */

            // using a Dictionary property
            //foreach (var item in values.Badges["attachmentsByType"]["trello"])
            //{
            //    Console.WriteLine("Key: " + item.Key + ", Value: " + item.Value);
            //}

            // using a dynamic property
            Assert.Zero(values.Badges["attachmentsByType"]["trello"]["board"]);
        }

        [Test, Order(5)]
        public void UpdateCard()
        {
            string cardName = "My Amazing Card Updated!";
            Dictionary<string, string> extraParams = new Dictionary<string, string>
            {
                { "name", cardName }
            };

            IRestResponse updateCardResponse = trelloClient.UpdateCard(trelloClient.GetBoardId(), trelloClient.GetListId(), trelloClient.GetCardId(), extraParams);
            TrelloCardModel values = deserializer.Deserialize<TrelloCardModel>(updateCardResponse);

            Assert.AreEqual(HttpStatusCode.OK, updateCardResponse.StatusCode);
            Assert.AreEqual(cardName, values.Name);
            Assert.False(values.Closed);
            Assert.AreEqual(trelloClient.GetListId(), values.IdList);
            Assert.AreEqual(trelloClient.GetBoardId(), values.IdBoard);
            Assert.Zero(values.Badges["votes"]);
            Assert.Zero(values.Badges["attachments"]);
        }

        [Test, Order(6)]
        public void DeleteBoard()
        {
            IRestResponse deleteBoardResponse = trelloClient.DeleteBoard(trelloClient.GetBoardId());

            TrelloBoardModel values = deserializer.Deserialize<TrelloBoardModel>(deleteBoardResponse);

            Assert.AreEqual(HttpStatusCode.OK, deleteBoardResponse.StatusCode);
            Assert.IsNull(values._Value);
        }

        [Test, Order(7)]
        public void GetBoardAfterDeleteIt()
        {
            string messageNotFound = "The requested resource was not found.";

            IRestResponse getBoardResponse = trelloClient.GetBoard(trelloClient.GetBoardId());

            Assert.AreEqual(HttpStatusCode.NotFound, getBoardResponse.StatusCode);
            Assert.AreEqual(messageNotFound, getBoardResponse.Content);
        }
    }
}
