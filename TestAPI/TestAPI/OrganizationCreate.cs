using NUnit.Framework;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace APItestAutomation {
    class OrganizationCreate {
        [Test]
        public void CreatePersonFromAPI() {
            RestClient client = new RestClient("https://landing-book-dev.azurewebsites.net/api");
            RestRequest request = new RestRequest("organizations", Method.Post);
            PostOrganization postName = new PostOrganization() {
                name = "Person",
            };
            request.AddBody(postName);
            var responce = client.Execute<string>(request);
            Assert.NotNull(responce.Data);
            Assert.That(responce.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            RestRequest requestPut = new RestRequest("organizations", Method.Put);
            PutOrganization putName = new PutOrganization() {
                name="BigHouse",
                Id = responce.Data
            };
            requestPut.RequestFormat = DataFormat.Json;
            requestPut.AddJsonBody(putName);
            var responcePut = client.Execute<PutOrganization>(requestPut);
            Assert.That(responcePut.Data.name, Is.EqualTo("BigHouse"));
            Assert.That(responcePut.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            RestRequest requestGet = new RestRequest($"organizations/{responce.Data}", Method.Get);
            var responceGet = client.Execute<PutOrganization>(requestGet);
            Assert.That(responceGet.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.IsNotNull(responceGet.Data.Id);
            RestRequest requestDelete = new RestRequest($"organizations/{responce.Data}", Method.Delete);
            var responceDelete = client.Execute<string>(requestDelete);
            Assert.That(responceDelete.StatusCode, Is.EqualTo(HttpStatusCode.OK));

        }
    }
}
