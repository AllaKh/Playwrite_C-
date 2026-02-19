using NUnit.Framework;
using Microsoft.Playwright;
using Pages;
using System.Threading.Tasks;
using System.IO;
using System.Text.Json;
using System.Collections.Generic;

namespace Tests
{
    /// <summary>
    /// Tests for navigating to the admin page and validating login errors
    /// </summary>
    public class AdminNavigationTests
    {
        private IPlaywright _playwright;
        private IBrowser _browser;
        private IPage _page;

        /// <summary>
        /// Setup Playwright and open a browser before each test
        /// </summary>
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

        /// <summary>
        /// Close browser and dispose Playwright after each test
        /// </summary>
        [TearDown]
        public async Task TearDown()
        {
            if (_browser != null)
                await _browser.CloseAsync();

            _playwright?.Dispose();
        }

        /// <summary>
        /// Test that invalid admin passwords show the correct error message
        /// Loops through all passwords in TestData/invalid_passwords.json
        /// </summary>
        [Test]
        public async Task Should_Show_Error_For_Invalid_Admin_Passwords()
        {
            var admin = new AdminPage(_page);

            // Navigate directly to admin login page
            await _page.GotoAsync("https://automationintesting.online/admin");
            await _page.WaitForTimeoutAsync(2000); // 2 seconds delay to see the page

            // Load invalid passwords from TestData directory
            string projectRoot = Directory.GetParent(Directory.GetCurrentDirectory())!.Parent!.Parent!.FullName;
            string jsonFile = Path.Combine(projectRoot, "TestData", "invalid_passwords.json");

            if (!File.Exists(jsonFile))
                Assert.Fail($"Test data file not found: {jsonFile}");

            var jsonText = await File.ReadAllTextAsync(jsonFile);
            using var doc = JsonDocument.Parse(jsonText);

            var invalidPasswords = new List<string>();
            if (doc.RootElement.TryGetProperty("invalidPasswords", out var passwordsElement))
            {
                foreach (var pwd in passwordsElement.EnumerateArray())
                    invalidPasswords.Add(pwd.GetString() ?? string.Empty);
            }

            // Selector for login error message
            string errorSelector =
                "#root-container > div > div > div > div > div.col-sm-8 > div > div.card-body > div";

            // Loop through all invalid passwords in the same session
            foreach (var pwd in invalidPasswords)
            {
                // Fill username once
                await _page.FillAsync("#username", "admin");

                // Clear password field and enter new password
                await _page.FillAsync("#password", string.Empty);
                await _page.FillAsync("#password", pwd);

                // Click login button
                await _page.ClickAsync("#doLogin");

                // Wait 1 second to allow error message to appear
                await _page.WaitForTimeoutAsync(1000);

                // Wait for the error element to appear
                await _page.WaitForSelectorAsync(errorSelector, new PageWaitForSelectorOptions
                {
                    State = WaitForSelectorState.Visible,
                    Timeout = 2000
                });

                // Verify the error message
                var errorText = await _page.TextContentAsync(errorSelector);
                Assert.That(errorText, Is.Not.Null.And.Contains("Invalid credentials"));
            }
        }
    }
}
