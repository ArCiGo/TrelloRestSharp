using NUnit.Framework;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using RestSharp_Trello_Sample.Utilities;

namespace RestSharp_Trello_Sample.Tests
{
    public class Client
    {
        private string trelloKey = "";
        private string trelloToken = "";
        private string boardId = "";
        private string boardName = "";
        private string listId = "";
        private string listName = "";
        private string cardId = "";
        private string cardName = "";

        private readonly RestClient trelloClient = new RestClient("https://api.trello.com/1");

        public string GetBoardId()
        {
            return boardId;
        }

        public void SetBoardId(string value)
        {
            boardId = value;
        }

        public string GetBoardName()
        {
            return boardName;
        }

        public void SetBoardName(string value)
        {
            boardName = value;
        }

        public string GetListId()
        {
            return listId;
        }

        public void SetListId(string value)
        {
            listId = value;
        }

        public string GetListName()
        {
            return listName;
        }

        public void SetListName(string value)
        {
            listName = value;
        }

        public string GetCardId()
        {
            return cardId;
        }

        public void SetCardId(string value)
        {
            cardId = value;
        }

        public string GetCardName()
        {
            return cardName;
        }

        public void SetCardName(string value)
        {
            cardName = value;
        }

        internal IRestResponse GetBoard(string boardId)
        {
            IRestRequest getBoardRequest = new RestRequest("/boards/" + boardId);

            getBoardRequest.Method = Method.GET;
            getBoardRequest.AddParameter("key", trelloKey);
            getBoardRequest.AddParameter("token", trelloToken);

            IRestResponse createResponse = trelloClient.Execute(getBoardRequest);

            return createResponse;
        }

        internal IRestResponse CreateBoard(string boardName, Dictionary<string, string> extraParams = null)
        {
            IRestRequest createBoardRequest = new RestRequest("/boards");
            createBoardRequest.Method = Method.POST;
            createBoardRequest.AddParameter("name", boardName);
            createBoardRequest.AddParameter("key", trelloKey);
            createBoardRequest.AddParameter("token", trelloToken);
            //Need to move below to extra params
            createBoardRequest.AddParameter("defaultLists", "false");
            //TODO: Above
            Utilities.Utilities.AddExtraParams(extraParams, createBoardRequest);

            IRestResponse createBoardResponse = trelloClient.Execute(createBoardRequest);
            Console.WriteLine(createBoardResponse.Content);
            return createBoardResponse;
        }

        internal IRestResponse CreateBoard()
        {
            IRestRequest createBoardRequest = new RestRequest("/boards");

            createBoardRequest.Method = Method.POST;
            createBoardRequest.AddParameter("name", "My Amazing Board");
            createBoardRequest.AddParameter("key", trelloKey);
            createBoardRequest.AddParameter("token", trelloToken);
            createBoardRequest.AddParameter("defaultLists", "false");

            IRestResponse createBoardResponse = trelloClient.Execute(createBoardRequest);
            Console.WriteLine(createBoardResponse.Content);

            return createBoardResponse;
        }

        internal IRestResponse DeleteBoard(string boardId)
        {
            IRestRequest deleteBoardRequest = new RestRequest("/boards/" + boardId);

            deleteBoardRequest.Method = Method.DELETE;
            deleteBoardRequest.AddParameter("key", trelloKey);
            deleteBoardRequest.AddParameter("token", trelloToken);

            IRestResponse deleteBoardResponse = trelloClient.Execute(deleteBoardRequest);

            return deleteBoardResponse;
        }

        internal IRestResponse CreateList(string boardId, string listName, Dictionary<string, string> extraParams = null)
        {
            IRestRequest createListRequest = new RestRequest("/boards/" + boardId + "/lists");

            createListRequest.Method = Method.POST;
            createListRequest.AddParameter("name", listName);
            createListRequest.AddParameter("key", trelloKey);
            createListRequest.AddParameter("token", trelloToken);
            Utilities.Utilities.AddExtraParams(extraParams, createListRequest);

            IRestResponse createListResponse = trelloClient.Execute(createListRequest);

            return createListResponse;
        }

        internal IRestResponse AddCardToList(string boardId, string cardName, string listId, Dictionary<string, string> extraParams = null)
        {
            IRestRequest addCardRequest = new RestRequest("/cards");

            addCardRequest.Method = Method.POST;
            addCardRequest.AddParameter("idBoard", boardId);
            addCardRequest.AddParameter("name", cardName);
            addCardRequest.AddParameter("idList", listId);
            //Need to add this to extra params in test
            addCardRequest.AddParameter("keepFromSource", "all");
            //TODO: Above
            addCardRequest.AddParameter("key", trelloKey);
            addCardRequest.AddParameter("token", trelloToken);
            Utilities.Utilities.AddExtraParams(extraParams, addCardRequest);

            IRestResponse cardAdditionResponse = trelloClient.Execute(addCardRequest);

            return cardAdditionResponse;
        }

        internal IRestResponse UpdateCard(string boardId, string listId, string cardId, Dictionary<string, string> extraParams = null)
        {
            IRestRequest updateCardRequest = new RestRequest("/cards/" + cardId);

            updateCardRequest.Method = Method.PUT;
            updateCardRequest.AddParameter("token", trelloToken);
            updateCardRequest.AddParameter("key", trelloKey);
            updateCardRequest.AddParameter("idBoard", boardId);
            updateCardRequest.AddParameter("idList", listId);
            Utilities.Utilities.AddExtraParams(extraParams, updateCardRequest);

            IRestResponse updateCardResponse = trelloClient.Execute(updateCardRequest);

            return updateCardResponse;
        }
    }
}
