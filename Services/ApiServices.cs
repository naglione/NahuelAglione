using System.Net.Http.Json;

namespace PlaywrightTest.Services
{
    public class ApiServices : IApiService
    {
        private readonly HttpClient _httpClient;
        public ApiServices(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public virtual async Task<Booking> GetBookingAsync(int bookingId, string url)
        {
            _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
            var requestUrl = $"{url}{bookingId}";
            return await _httpClient.GetFromJsonAsync<Booking>(requestUrl);
        }
    }
}