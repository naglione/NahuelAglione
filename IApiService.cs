using System.Threading.Tasks;

namespace PlaywrightTest.Services
{
    internal interface IApiService
    {
        Task<Booking> GetBookingAsync(int bookingId, string url);
    }
}