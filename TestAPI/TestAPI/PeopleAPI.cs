using Newtonsoft.Json;
using NUnit.Framework;
using RestSharp;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace APItestAutomation {
    public class Tests {
        [SetUp]
        public void Setup() {
        }

        [Test]
        public void GetNameAllPeopleFromAPI() {
            RestClient client = new RestClient("https://landing-book-dev.azurewebsites.net/api");
            RestRequest request = new RestRequest("people", Method.Get);
            var responce = client.Execute(request);
            Assert.That(responce.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }
        [Test]
        public void GetNamePersonFromAPI() {
            RestClient client = new RestClient("https://landing-book-dev.azurewebsites.net/api");
            RestRequest request = new RestRequest("people/537d900c-adaf-4a68-a0fc-08db2092348a", Method.Get);
            var responce = client.Execute<PersonUpdateModel>(request);
            Assert.That(responce.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.IsNotNull(responce.Data.Id);
        }


        [Test]
        public void PostNamePersonFromAPI() {
            RestClient client = new RestClient("https://landing-book-dev.azurewebsites.net/api");
            RestRequest request = new RestRequest("people", Method.Post);
            PersonCreateModel postName = new PersonCreateModel() {
                FirstName = "Ig",
                LastName = "Ivanov",
                SerialNumber = "147",
                OrganizationId = "3fa85f64-5717-4562-b3fc-2c963f66afa6"
            };
            request.AddBody(postName);
            var responce = client.Execute<Guid>(request);
            Assert.NotNull(responce.Data);
            Assert.That(responce.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }
        [Test]
        public void PutNamePersonFromAPI() {
            RestClient client = new RestClient("https://landing-book-dev.azurewebsites.net/api");
            RestRequest request = new RestRequest("people", Method.Put);
            PersonUpdateModel postName = new PersonUpdateModel() {
                FirstName = "Bol",
                LastName = "TOMA",
                SerialNumber = 114,
                OrganizationId = "3fa85f64-5717-4562-b3fc-2c963f66afa6",
                Id = "ddbf137b-a400-4604-95a6-08db1f121a90"
            };
            request.RequestFormat = DataFormat.Json;
            request.AddJsonBody(postName);
            var responce = client.Execute<PersonUpdateModel>(request);
            Assert.That(responce.Data.FirstName, Is.EqualTo("Bol"));
            Assert.That(responce.Data.LastName, Is.EqualTo("TOMA"));
            Assert.That(responce.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }
        [Test]
        public void DeleteNamePersonFromAPI() {
            RestClient client = new RestClient("https://landing-book-dev.azurewebsites.net/api");
            RestRequest request = new RestRequest("people/ecc79853-43a0-4403-a0fd-08db2092348a", Method.Delete);

            var responce = client.Execute<string>(request);
            Assert.That(responce.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.IsNotNull(responce.Data);
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
        public void CreatePersonFromAPI() {
            RestClient client = new RestClient("https://landing-book-dev.azurewebsites.net/api");
            RestRequest request = new RestRequest("people", Method.Post);
            PersonCreateModel postName = new PersonCreateModel() {
                FirstName = "Ivan",
                LastName = "Igor",
                SerialNumber = "72",
                OrganizationId = "3fa85f64-5717-4562-b3fc-2c963f66afa6"
            };
            request.AddBody(postName);
            var responce = client.Execute<string>(request);
            Assert.NotNull(responce.Data);
            Assert.That(responce.StatusCode, Is.EqualTo(HttpStatusCode.OK));
           RestRequest requestPut = new RestRequest("people", Method.Put);
            PersonUpdateModel putName = new PersonUpdateModel() {
                FirstName = "Roma",
                LastName = "Tirov",
                SerialNumber = 4,
                OrganizationId = "3fa85f64-5717-4562-b3fc-2c963f66afa6",
                Id = responce.Data
                };
                requestPut.RequestFormat = DataFormat.Json;
                requestPut.AddJsonBody(putName);
                var responcePut = client.Execute<PersonUpdateModel>(requestPut);
                Assert.That(responcePut.Data.FirstName, Is.EqualTo("Roma"));
                Assert.That(responcePut.Data.LastName, Is.EqualTo("Tirov"));
                Assert.That(responcePut.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            RestRequest requestDelete = new RestRequest($"people/{responce.Data}", Method.Delete);
            var responceDelete = client.Execute<string>(requestDelete);
            Assert.That(responceDelete.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }
    }
}