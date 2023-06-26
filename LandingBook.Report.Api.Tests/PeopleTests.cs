using LandingBook.Report.Api.Tests.Models;
using Newtonsoft.Json;
using NUnit.Framework;
using RestSharp;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace LandingBook.Report.Api.Tests {
    public class PeopleTests {
        private RestClient _client;
        private const string _serverName = "https://landing-book-dev.azurewebsites.net/api";
        private const string _controllerName = "people";

        [SetUp]
        public void Setup() {
            _client = new RestClient(_serverName);
        }

        [Test]
        public void GetNameAllPeopleFromAPI() {
            RestRequest request = new RestRequest(_controllerName, Method.Get);
            var responce = _client.Execute<List<PersonUpdateModel>>(request);
            //todo: add check for responce.Data is not null
            Assert.IsNotNull(responce.Data);
            Assert.That(responce.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [Test]
        public void GetNamePersonFromAPI() {
            RestRequest request = new RestRequest($"{_controllerName}/537d900c-adaf-4a68-a0fc-08db2092348a", Method.Get);
            var responce = _client.Execute<PersonUpdateModel>(request);
            Assert.That(responce.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.IsNotNull(responce.Data.Id);
        }

        [Test]
        public async Task PostNewPersonFromAPI() {
            using var httpClient = new HttpClient();
            PersonUpdateModel postName = new PersonUpdateModel() {
                FirstName = "Igor",
                LastName = "Ivanov",
                SerialNumber = 37,
                OrganizationId = "3fa85f64-5717-4562-b3fc-2c963f66afa6"
            };
            JsonConvert.SerializeObject(postName);
            var Json = JsonConvert.SerializeObject(postName);
            var content = new StringContent(Json, Encoding.UTF8, MediaTypeNames.Application.Json);
            var response = await httpClient.PostAsync("https://landing-book-dev.azurewebsites.net/api/people", content);
            Assert.NotNull(await response.Content.ReadAsStringAsync());
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [Test]
        public void PeopleCreateReadUpdateDelete() {
            string peopleId = CreatePerson();
            UpdatePerson(peopleId);
            DeletePerson(peopleId);
        }

        private string CreatePerson() {
            RestRequest request = new RestRequest(_controllerName, Method.Post);
            PersonCreateModel postName = new PersonCreateModel() {
                FirstName = "Ivan",
                LastName = "Igor",
                SerialNumber = 72,
                OrganizationId = "3fa85f64-5717-4562-b3fc-2c963f66afa6"
            };
            request.AddBody(postName);
            var responce = _client.Execute<string>(request);
            Assert.NotNull(responce.Data);
            Assert.That(responce.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            return responce.Data;
        }

        private void UpdatePerson(string peopleId) {
            RestRequest request = new RestRequest(_controllerName, Method.Put);
            PersonUpdateModel model = new PersonUpdateModel() {
                FirstName = "Roma",
                LastName = "Tirov",
                SerialNumber = 4,
                OrganizationId = "3fa85f64-5717-4562-b3fc-2c963f66afa6",
                Id = peopleId
            };
            request.RequestFormat = DataFormat.Json;
            request.AddJsonBody(model);
            var responce = _client.Execute<PersonUpdateModel>(request);
            Assert.That(responce.Data.FirstName, Is.EqualTo(model.FirstName));
            Assert.That(responce.Data.LastName, Is.EqualTo(model.LastName));
            Assert.That(responce.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        private void DeletePerson(string peopleId) {
            RestRequest request = new RestRequest($"{_controllerName}/{peopleId}", Method.Delete);
            var responce = _client.Execute<string>(request);
            Assert.That(responce.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(responce.Data, Is.EqualTo(peopleId));
        }
    }
}