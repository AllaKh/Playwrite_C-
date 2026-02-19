using NUnit.Framework;
using Microsoft.Playwright;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

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
        _browser = await _pw.Chromium.LaunchAsync(new BrowserTypeLaunchOptions { Headless = _settings.Headless });
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
        var home = new HomePage(_page);
        var admin = new AdminPage(_page);
        var room = new RoomPage(_page);
        var report = new AdminReportPage(_page);

        // Open home
        await home.OpenAsync(_settings.BaseURL);

        // Go to admin and login
        await home.GoAdminAsync();
        await admin.LoginAsync(_settings.Auth.Username, _settings.Auth.Password);

        // Back to front page
        await home.BackFrontAsync();
        await home.ScrollDownOneThirdAsync();
        await home.OpenSecondRoomAsync();

        // Room page
        var (start, end) = room.GenerateRandomDates();
        await room.GoToRoomWithDatesAsync(2, start, end);
        await room.OpenReservationAsync();

        // Fill booking
        var payloadJson = File.ReadAllText("TestData/payload.json");
        var payload = JsonSerializer.Deserialize<JsonElement>(payloadJson);
        await room.FillBookingFormAsync(payload);

        await _page.WaitForTimeoutAsync(1000);

        // Admin report
        await report.OpenAsync();
        var fullName = $"{payload.GetProperty("first_name").GetString()} {payload.GetProperty("last_name").GetString()}";
        Assert.That(await report.FindBookingInTableAsync(fullName), Is.True, $"Booking for {fullName} not found");

        await report.LogoutAsync();
    }
}
