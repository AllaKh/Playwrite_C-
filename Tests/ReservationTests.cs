using NUnit.Framework;
using Microsoft.Playwright;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Pages;

namespace Tests
{
    [TestFixture]
    public class ReservationTests
    {
        private IPlaywright _pw;
        private IBrowser _browser;
        private IPage _page;
        private PlaywrightSettings _settings;

        [SetUp]
        public async Task SetupAsync()
        {
            _settings = PlaywrightSettings.Load();

            _pw = await Playwright.CreateAsync();
            _browser = await _pw.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
            {
                Headless = _settings.Headless
            });
            _page = await _browser.NewPageAsync();
        }

        [TearDown]
        public async Task TearDownAsync()
        {
            await _page.CloseAsync();
            await _browser.CloseAsync();
            _pw.Dispose();
        }

        [Test]
        public async Task FullReservationFlowAsync()
        {
            // Initialize page objects
            var home = new HomePage(_page);
            var admin = new AdminPage(_page);
            var room = new RoomPage(_page);
            var report = new AdminReportPage(_page);

            // 1. Open home page
            await home.OpenAsync();

            // 2. Go to admin page and login
            await home.GoAdminAsync();
            await admin.LoginAsync(_settings.Auth.Username, _settings.Auth.Password);

            // 3. Go back to front page
            await home.BackFrontAsync();
            await home.ScrollDownOneThirdAsync();
            await home.OpenSecondRoomAsync();

            // 4. Room page: generate random dates and go to reservation URL
            var (start, end) = room.GenerateRandomDates();
            await room.GoToRoomWithDatesAsync(2, start, end);

            // Wait for reservation page to load
            await _page.WaitForTimeoutAsync(2000);

            // 5. Click "Make Reservation"
            await room.OpenReservationAsync();

            // 6. Load booking payload from TestData directory
            string projectRoot = Directory.GetParent(Directory.GetCurrentDirectory())!.Parent!.Parent!.FullName;
            string payloadPath = Path.Combine(projectRoot, "TestData", "payload.json");

            if (!File.Exists(payloadPath))
                Assert.Fail($"Test data file not found: {payloadPath}");

            string payloadJson = await File.ReadAllTextAsync(payloadPath);
            var payload = JsonSerializer.Deserialize<JsonElement>(payloadJson);

            // 7. Fill booking form and submit
            await room.FillBookingFormAsync(payload!);

            // Wait a moment for confirmation
            await _page.WaitForTimeoutAsync(1000);

            // 8. Go to admin report page and verify booking
            await report.OpenAsync();
            string fullName = $"{payload.GetProperty("first_name").GetString()} {payload.GetProperty("last_name").GetString()}";
            Assert.That(await report.FindBookingInTableAsync(fullName), Is.True, $"Booking for {fullName} not found in admin report");

            // 9. Logout
            await report.LogoutAsync();
        }
    }
}
