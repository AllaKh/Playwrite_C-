using Microsoft.Playwright;
using System.Threading.Tasks;

namespace Pages
{
    /// <summary>
    /// Page object for the Admin login page
    /// Handles login functionality
    /// </summary>
    public class AdminPage : BasePage
    {
        private readonly string usernameSelector = "#username";
        private readonly string passwordSelector = "#password";
        private readonly string doLoginSelector = "#doLogin";

        public AdminPage(IPage page) : base(page) { }

        /// <summary>
        /// Fill in username/password and click login
        /// Waits for network to be idle after login
        /// </summary>
        public async Task LoginAsync(string user, string pwd)
        {
            await Page.FillAsync(usernameSelector, user);
            await Page.FillAsync(passwordSelector, pwd);
            await Page.ClickAsync(doLoginSelector);
            await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        }
    }
}
