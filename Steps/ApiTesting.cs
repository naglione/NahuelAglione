using Microsoft.Playwright;
using Newtonsoft.Json;
using System;
using TechTalk.SpecFlow;
using FluentAssertions;
using System.Text.Json;
using System.Net;
using System.Text;
using System.Net.Mime;
using SpecFlow.Internal.Json;
using System.Net.Http.Json;

namespace PlaywrightTest.Steps
{
    [Binding]
    public class ApiTestingPOST
    {
        public HttpResponseMessage _response;
        public HttpResponseMessage _responseGET;

        [When(@"Call the Api")]
        public async Task WhenCallTheApi()
        {
            var body = new { username = "admin", password = "password123" };
            var fullBody = new StringContent(
                JsonConvert.SerializeObject(body),
                Encoding.UTF8,
                MediaTypeNames.Application.Json);
            HttpClient _client = new HttpClient();
            _response = await _client.PostAsync("https://restful-booker.herokuapp.com/auth", fullBody);
            
        }

        [Then(@"Verify response")]
        public async Task ThenVerifyResponse()
        {
            var jsonString = await _response.Content.ReadAsStringAsync();
            dynamic bodyJson = JsonConvert.DeserializeObject(jsonString);
            string token = bodyJson.token;
            token.Should().NotBeNullOrEmpty();
            _response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [When(@"Call the GET")]
        public async Task WhenCallTheGET()
        {
            HttpClient _client = new HttpClient();
            _client.DefaultRequestHeaders.Add("Accept", "application/json");
            var id = 65;
            var url = string.Format("https://restful-booker.herokuapp.com/booking/{0}", id);
            _responseGET = await _client.GetAsync(url);
        }

        [Then(@"Verify the response")]
        public async Task ThenVerifyTheResponse()
        {
            var jsonString = await _responseGET.Content.ReadAsStringAsync();
            dynamic bodyJson = JsonConvert.DeserializeObject(jsonString);
            string firstname = bodyJson.firstname;
            firstname.Should().NotBeNullOrEmpty();
            _responseGET.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [When(@"Step")]
        public void WhenStep()
        {

        }

        [Then(@"Step")]
        public void ThenStep()
        {

        }

    }
}
