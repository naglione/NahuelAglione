using Newtonsoft.Json;
using Reqnroll;
using FluentAssertions;
using System.Net;
using System.Text;
using System.Net.Mime;
using PlaywrightTest.Services;
using Moq;

namespace PlaywrightTest.Steps
{
    [Binding]
    public class ApiTestingPOST
    {
        private HttpResponseMessage _response;
        private HttpResponseMessage _responseGET;
        private Booking _booking;
        private Booking _mockBooking;

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

        [Given(@"Arrange")]
        public async Task GivenArrange()
        {
            var dates = new Bookingdates { checkin = "2018-01-01", checkout = "2019-01-01" };
            _mockBooking = new Booking { firstname = "nombre", lastname = "apellido", totalprice = 10, depositpaid = false, bookingdates = dates, additionalneeds = "test" };
            var mockApiServices = new Mock<ApiServices>();
            mockApiServices.Setup(client => client.GetBookingAsync(65, "https://restful-booker.herokuapp.com/booking/")).ReturnsAsync(_mockBooking);
        }

        [When(@"Act")]
        public async Task WhenAct()
        {
            var httpClient = new HttpClient();
            var ApiServices = new ApiServices(httpClient);
            _booking = await ApiServices.GetBookingAsync(65, "https://restful-booker.herokuapp.com/booking/");
        }

        [Then(@"Assert")]
        public async Task ThenAssert()
        {
            _booking.Should().NotBeEquivalentTo(_mockBooking, option => option.Excluding(a => a.bookingdates));
            _mockBooking.firstname.Should().BeEquivalentTo("nombre");
        }
    }
}
