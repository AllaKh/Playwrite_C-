using Microsoft.Playwright;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace Pages
{
    /// <summary>
    /// Page object for the Room page
    /// Provides methods to interact with room details and reservations
    /// </summary>
    public class RoomPage : BasePage
    {
        /// <summary>
        /// Selector for the room heading
        /// </summary>
        public readonly string HeadingSelector = "h1.room-title";

        /// <summary>
        /// Selector for the "Make Reservation" button
        /// </summary>
        public readonly string MakeReservationButton = "#make-reservation-btn";

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="page">Playwright IPage instance</param>
        public RoomPage(IPage page) : base(page) { }

        /// <summary>
        /// Checks if the room heading is visible
        /// </summary>
        /// <returns>True if visible, false otherwise</returns>
        public async Task<bool> HasHeadingAsync()
        {
            return await IsVisibleAsync(HeadingSelector);
        }

        /// <summary>
        /// Generates random start and end dates for booking
        /// </summary>
        /// <returns>Tuple of start and end dates in yyyy-MM-dd format</returns>
        public (string Start, string End) GenerateRandomDates()
        {
            var random = new Random();
            var start = DateTime.Today.AddDays(random.Next(1, 30));
            var end = start.AddDays(random.Next(1, 5));
            return (start.ToString("yyyy-MM-dd"), end.ToString("yyyy-MM-dd"));
        }

        /// <summary>
        /// Navigates to a specific room page with given dates
        /// </summary>
        /// <param name="roomId">Room ID</param>
        /// <param name="start">Start date (yyyy-MM-dd)</param>
        /// <param name="end">End date (yyyy-MM-dd)</param>
        public async Task GoToRoomWithDatesAsync(int roomId, string start, string end)
        {
            await NavigateAsync($"/reservation/{roomId}?start={start}&end={end}");
        }

        /// <summary>
        /// Clicks the "Make Reservation" button
        /// </summary>
        public async Task OpenReservationAsync()
        {
            await Page.ClickAsync(MakeReservationButton);
        }

        /// <summary>
        /// Fills the booking form with data from a JSON payload and submits
        /// </summary>
        /// <param name="payload">JSON object containing form data</param>
        public async Task FillBookingFormAsync(JsonElement payload)
        {
            foreach (var prop in payload.EnumerateObject())
            {
                var selector = $"#{prop.Name}";
                var value = prop.Value.GetString() ?? string.Empty;
                await Page.FillAsync(selector, value);
            }

            await Page.ClickAsync("#submit-btn");
        }
    }
}
