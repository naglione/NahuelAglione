using FluentAssertions;
using Reqnroll;


namespace PlaywrightTest.Steps
{
    [Binding]
    internal class Program
    {
        private HttpClient _httpClient;
        private HttpResponseMessage responseMessage;

        [Given(@"Params")]
        public async Task GivenParams()
        {
            System.Console.WriteLine("Params should be here");
        }

        [When(@"Call endpoint")]
        public async Task WhenCallEndpoint()
        {
            _httpClient = new HttpClient();
            responseMessage = await _httpClient.GetAsync("http://localhost:9876/hello-world");
        }

        [Then(@"Assertions")]
        public async Task ThenAssertions()
        {
            var body = await responseMessage.Content.ReadAsStringAsync();
            body.Should().Be("Hello, world!");
        }
    }
}
