using Microsoft.Playwright;
using System.Threading.Tasks;

namespace Pages
{
    /// <summary>
    /// Page object for the Home page
    /// Handles navigation and admin link
    /// </summary>
    public class HomePage : BasePage
    {
        private readonly string adminLink = "#navbarNav > ul > li:nth-child(6) > a";

        public HomePage(IPage page) : base(page) { }

        /// <summary>
        /// Open the home page (URL is read from BasePage/config.json)
        /// </summary>
        public async Task OpenAsync()
        {
            await Page.GotoAsync(BaseUrl);
        }

        /// <summary>
        /// Navigate to the admin page by clicking the admin link
        /// Waits for the element to be visible before clicking
        /// </summary>
        public async Task GoAdminAsync()
        {
            await Page.WaitForSelectorAsync(adminLink, new PageWaitForSelectorOptions
            {
                State = WaitForSelectorState.Visible,
                Timeout = 5000
            });

            await Page.ClickAsync(adminLink);
        }

        public string AdminLink => adminLink;
    }
}
