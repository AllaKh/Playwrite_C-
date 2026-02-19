using Microsoft.Playwright;
using System;
using System.Threading.Tasks;

namespace Pages
{
    /// <summary>
    /// Page object for the Admin login page
    /// Handles login functionality for both correct and incorrect credentials
    /// </summary>
    public class AdminPage : BasePage
    {
        private readonly string usernameSelector = "#username";
        private readonly string passwordSelector = "#password";
        private readonly string doLoginSelector = "#doLogin";

        // Hardcoded URL that must be reached after successful login
        private readonly string adminRoomsUrl = "https://automationintesting.online/admin/rooms";

        public AdminPage(IPage page) : base(page) { }

        /// <summary>
        /// Log in using provided username and password
        /// Works for both valid and invalid credentials
        /// </summary>
        /// <param name="user">Username to use</param>
        /// <param name="pwd">Password to use</param>
        public async Task LoginAsync(string user, string pwd)
        {
            // Clear any pre-filled values first
            await Page.FillAsync(usernameSelector, string.Empty);
            await Page.FillAsync(passwordSelector, string.Empty);

            // Fill in credentials
            await Page.FillAsync(usernameSelector, user);
            await Page.FillAsync(passwordSelector, pwd);

            // Click login button
            await Page.ClickAsync(doLoginSelector);

            // Wait for network to settle
            await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

            // Check if login succeeded for correct credentials
            if (!string.IsNullOrEmpty(user) && !string.IsNullOrEmpty(pwd))
            {
                // Wait up to 5 seconds for the URL to change to adminRoomsUrl
                try
                {
                    await Page.WaitForURLAsync(adminRoomsUrl, new PageWaitForURLOptions
                    {
                        Timeout = 5000
                    });
                }
                catch
                {
                    throw new Exception($"Login failed: expected URL '{adminRoomsUrl}', but got '{Page.Url}'");
                }
            }
        }

        /// <summary>
        /// Helper method: log in using the correct credentials from config.json
        /// </summary>
        /// <param name="settings">PlaywrightSettings instance</param>
        public async Task LoginWithConfigAsync(PlaywrightSettings settings)
        {
            await LoginAsync(settings.Auth.Username, settings.Auth.Password);
        }

        /// <summary>
        /// Helper method: log in with a specific invalid password
        /// </summary>
        /// <param name="username">Username (usually "admin")</param>
        /// <param name="invalidPassword">Password from invalid_passwords.json</param>
        public async Task LoginWithInvalidPasswordAsync(string username, string invalidPassword)
        {
            await LoginAsync(username, invalidPassword);
        }
    }
}
