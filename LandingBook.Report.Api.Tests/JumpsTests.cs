using LandingBook.Report.Api.Tests.Models;
using NUnit.Framework;
using RestSharp;
using System.Net;
namespace LandingBook.Report.Api.Tests {
    class JumpsTests {
        private RestClient _client;
        private const string _serverName = "https://landing-book-dev.azurewebsites.net/api";
        private const string _controllerName = "jumps";

        [SetUp]
        public void Setup() {
            _client = new RestClient(_serverName);
        }

        [Test]
        public void JumpCreateReadUpdateDelete() {
            string jumpId = CreateJump();
            UpdateJump(jumpId);
            GetJumpById(jumpId);
            DeleteJump(jumpId);
        }

        private string CreateJump() {
            RestRequest request = new RestRequest(_controllerName, Method.Post);
            JumpCreateModel model = new JumpCreateModel() {
                Name = "BOB",
                Result = 4,
                WindSpeed = 1,
                Date = 0,
                IsCompetition = true,
                PersonId = "85da8808-e1aa-485e-e4f2-08db1f09ef1b"
            };
            request.AddBody(model);
            var responce = _client.Execute<string>(request);
            Assert.NotNull(responce.Data);
            Assert.That(responce.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            return responce.Data;
        }

        private void UpdateJump(string jumpId) {
            RestRequest request = new RestRequest(_controllerName, Method.Put);
            JumpsUpdateModel model = new JumpsUpdateModel() {
                Id = jumpId,
                Name = "BOB",
                Result = 9,
                WindSpeed = 1,
                Date = 19,
                IsCompetition = true,
                PersonId = "85da8808-e1aa-485e-e4f2-08db1f09ef1b"
            };
            request.RequestFormat = DataFormat.Json;
            request.AddJsonBody(model);
            var responce = _client.Execute<JumpsUpdateModel>(request);
            Assert.That(responce.Data.Result, Is.EqualTo(model.Result));
            Assert.That(responce.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        private void GetJumpById(string jumpId) {
            RestRequest request = new RestRequest($"{_controllerName}/{jumpId}", Method.Get);
            var responce = _client.Execute<JumpsUpdateModel>(request);
            Assert.That(responce.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.AreEqual(responce.Data.Id, jumpId);
        }

        private void DeleteJump(string jumpId) {
            RestRequest request = new RestRequest($"{_controllerName}/{jumpId}", Method.Delete);
            var responce = _client.Execute<string>(request);
            Assert.That(responce.Data, Is.EqualTo(jumpId));
            Assert.That(responce.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }
    }
}
