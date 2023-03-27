using LandingBook.Report.Api.Tests.Models;
using NUnit.Framework;
using RestSharp;
using System.Net;

namespace LandingBook.Report.Api.Tests {
    class OrganizationsTests {
        private RestClient _client;
        private const string _serverName = "https://landing-book-dev.azurewebsites.net/api";
        private const string _controllerName = "organizations";

        [SetUp]
        public void Setup() {
            _client = new RestClient(_serverName);
        }

        [Test]
        public void OrganizationCreateReadUpdateDelet() {
            string organizationId = CreateOrganization();
            UpdateOrganization(organizationId);
            GetOrganizationById(organizationId);
            DeleteOrganizationById(organizationId);
        }

        private string CreateOrganization() {
            RestRequest request = new RestRequest(_controllerName, Method.Post);
            OrganizationCreateModel model = new OrganizationCreateModel() {
                Name = "Person",
            };
            request.AddBody(model);
            var responce = _client.Execute<string>(request);
            Assert.NotNull(responce.Data);
            Assert.That(responce.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            return responce.Data;
        }

        private void UpdateOrganization(string organizationId) {
            RestRequest request = new RestRequest(_controllerName, Method.Put);
            OrganizationUpdateModel model = new OrganizationUpdateModel() {
                Name = "BigHouse",
                Id = organizationId
            };
            request.RequestFormat = DataFormat.Json;
            request.AddJsonBody(model);
            var responce = _client.Execute<OrganizationUpdateModel>(request);
            Assert.That(responce.Data.Name, Is.EqualTo(model.Name));
            Assert.That(responce.StatusCode, Is.EqualTo(HttpStatusCode.OK));

        }

        private void GetOrganizationById (string organizationId) {
            RestRequest request = new RestRequest($"{_controllerName}/{organizationId}", Method.Get);
            var responce = _client.Execute<OrganizationUpdateModel>(request);
            Assert.That(responce.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.IsNotNull(responce.Data.Id);
        }

        private void DeleteOrganizationById(string organizationId) {
            RestRequest request = new RestRequest($"{_controllerName}/{organizationId}", Method.Delete);
            var responce = _client.Execute<string>(request);
            Assert.That(responce.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(responce.Data, Is.EqualTo(organizationId));
        }
        }
}
