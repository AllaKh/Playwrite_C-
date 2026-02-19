using Microsoft.Playwright;
using NUnit.Framework;
using Pages;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace Tests
{
    /// <summary>
    /// Tests for navigating to the admin page and validating login errors
    /// </summary>
    [TestFixture]
    public class AdminNavigationTests
    {
        private IPlaywright _playwright;
        private IBrowser _browser;
        private IPage _page;

        [SetUp]
        public async Task Setup()
        {
            _playwright = await Playwright.CreateAsync();
            _browser = await _playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
            {
                Headless = false
            });

            _page = await _browser.NewPageAsync();
        }

        [TearDown]
        public async Task TearDown()
        {
            if (_browser != null)
                await _browser.CloseAsync();

            _playwright?.Dispose();
        }

        [Test]
        public async Task Should_Show_Error_For_Invalid_Admin_Passwords()
        {
            // Navigate to Admin Login
            await _page.GotoAsync("https://automationintesting.online/admin");
            await _page.WaitForTimeoutAsync(2000);

            // Load Invalid Passwords
            string projectRoot = Directory
                .GetParent(Directory.GetCurrentDirectory())!
                .Parent!.Parent!.FullName;

            string jsonFile = Path.Combine(projectRoot, "TestData", "invalid_passwords.json");

            Assert.That(File.Exists(jsonFile),
                $"Test data file not found: {jsonFile}");

            var jsonText = await File.ReadAllTextAsync(jsonFile);
            using var doc = JsonDocument.Parse(jsonText);

            var invalidPasswords = new List<string>();

            if (doc.RootElement.TryGetProperty("invalidPasswords", out var passwordsElement))
            {
                foreach (var pwd in passwordsElement.EnumerateArray())
                    invalidPasswords.Add(pwd.GetString() ?? string.Empty);
            }

            Assert.That(invalidPasswords.Count, Is.GreaterThan(0),
                "No invalid passwords found in JSON file");

            // Validate Each Invalid Password
            string errorSelector =
                "#root-container > div > div > div > div > div.col-sm-8 > div > div.card-body > div";

            foreach (var pwd in invalidPasswords)
            {
                await _page.FillAsync("#username", "admin");

                await _page.FillAsync("#password", string.Empty);
                await _page.FillAsync("#password", pwd);

                await _page.ClickAsync("#doLogin");
                await _page.WaitForTimeoutAsync(1000);

                await _page.WaitForSelectorAsync(errorSelector, new PageWaitForSelectorOptions
                {
                    State = WaitForSelectorState.Visible,
                    Timeout = 2000
                });

                var errorText = await _page.TextContentAsync(errorSelector);

                Assert.That(errorText,
                    Is.Not.Null.And.Contains("Invalid credentials"),
                    $"Expected error message not shown for password: {pwd}");
            }
        }
    }
}
