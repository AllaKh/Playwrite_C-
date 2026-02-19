using Microsoft.Playwright;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace Pages
{
    /// <summary>
    /// Page object for a Room
    /// Handles reservation form interactions
    /// </summary>
    public class RoomPage : BasePage
    {
        public readonly string HeadingSelector = "#root-container h1";
        private readonly string doReservationSelector = "#doReservation";

        private readonly string firstNameInput = "#root-container > div > div.container.my-5 > div > div.col-lg-4 > div > div > form > div.input-group.mb-3.room-booking-form > input";
        private readonly string lastNameInput = "#root-container > div > div.container.my-5 > div > div.col-lg-4 > div > div > form > div:nth-child(2) > input";
        private readonly string emailInput = "#root-container > div > div.container.my-5 > div > div.col-lg-4 > div > div > form > div:nth-child(3) > input";
        private readonly string phoneInput = "#root-container > div > div.container.my-5 > div > div.col-lg-4 > div > div > form > div:nth-child(4) > input";
        private readonly string submitButton = "#root-container > div > div.container.my-5 > div > div.col-lg-4 > div > div > form > button.btn.btn-primary.w-100.mb-3";

        public RoomPage(IPage page) : base(page) { }

        /// <summary>
        /// Check if room heading is visible
        /// </summary>
        public async Task<bool> HasHeadingAsync()
        {
            return await IsVisibleAsync(HeadingSelector);
        }

        /// <summary>
        /// Generate random check-in and check-out dates
        /// </summary>
        public (string CheckIn, string CheckOut) GenerateRandomDates()
        {
            var rnd = new Random();
            var checkin = DateTime.Today.AddDays(rnd.Next(0, 10));
            var checkout = checkin.AddDays(1);
            return (checkin.ToString("yyyy-MM-dd"), checkout.ToString("yyyy-MM-dd"));
        }

        /// <summary>
        /// Navigate to reservation page with specific dates
        /// </summary>
        public async Task GoToRoomWithDatesAsync(int roomId, string checkin, string checkout)
        {
            string url = $"{BaseUrl}/reservation/{roomId}?checkin={checkin}&checkout={checkout}";
            await Page.GotoAsync(url);
            await Page.WaitForTimeoutAsync(2000);
        }

        /// <summary>
        /// Click "Make Reservation" button
        /// </summary>
        public async Task OpenReservationAsync()
        {
            await Page.ClickAsync(doReservationSelector);
            await Page.WaitForTimeoutAsync(500);
        }

        /// <summary>
        /// Fill reservation form and submit
        /// </summary>
        public async Task FillBookingFormAsync(JsonElement payload)
        {
            await Page.FillAsync(firstNameInput, payload.GetProperty("first_name").GetString() ?? string.Empty);
            await Page.FillAsync(lastNameInput, payload.GetProperty("last_name").GetString() ?? string.Empty);
            await Page.FillAsync(emailInput, payload.GetProperty("email").GetString() ?? string.Empty);
            await Page.FillAsync(phoneInput, payload.GetProperty("phone").GetString() ?? string.Empty);
            await Page.ClickAsync(submitButton);
        }
    }
}
