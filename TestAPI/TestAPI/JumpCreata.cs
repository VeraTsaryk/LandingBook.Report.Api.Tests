using NUnit.Framework;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
namespace APItestAutomation {
    class JumpCreate {
        [Test]
        public void CreateJumpFromAPI() {
            RestClient client = new RestClient("https://landing-book-dev.azurewebsites.net/api");
            RestRequest request = new RestRequest("jumps", Method.Post);
            PostJump postName = new PostJump() {
                name = "BOB",
                result = 4,
                windSpeed = 1,
                date = 0,
                isCompetition = true,
                personId = "85da8808-e1aa-485e-e4f2-08db1f09ef1b"
            };
            request.AddBody(postName);
            var responce = client.Execute<string>(request);
            Assert.NotNull(responce.Data);
            Assert.That(responce.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            RestRequest requestPut = new RestRequest("jumps", Method.Put);
            PutJumps putName = new PutJumps() {
                name = "BOB",
                result = 9,
                windSpeed = 1,
                date = 19,
                isCompetition = true,
                personId = "85da8808-e1aa-485e-e4f2-08db1f09ef1b",
                Id = responce.Data
            };
            requestPut.RequestFormat = DataFormat.Json;
            requestPut.AddJsonBody(putName);
            var responcePut = client.Execute<PutJumps>(requestPut);
            Assert.That(responcePut.Data.result, Is.EqualTo(9));
            Assert.That(responcePut.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            RestRequest requestGet = new RestRequest($"jumps/{responce.Data}", Method.Get);
            var responceGet = client.Execute<PutJumps>(requestGet);
            Assert.That(responceGet.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.IsNotNull(responceGet.Request);
            RestRequest requestDelete = new RestRequest($"jumps/{responce.Data}", Method.Delete);
            var responceDelete = client.Execute<string>(requestDelete);
            Assert.That(responceDelete.Data, Is.EqualTo(responce.Data));
            Assert.That(responceDelete.StatusCode, Is.EqualTo(HttpStatusCode.OK));

        }
    }
}
