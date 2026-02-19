using Microsoft.Playwright;
using System.Threading.Tasks;

namespace Pages
{
    /// <summary>
    /// Page object for the Home page
    /// Handles navigation, admin link, front page link, and room selection
    /// </summary>
    public class HomePage : BasePage
    {
        private readonly string adminLink = "#navbarNav > ul > li:nth-child(6) > a";
        private readonly string secondRoomLink = "#rooms > div > div.row.g-4 > div:nth-child(2) > div > div.card-footer.bg-white.d-flex.justify-content-between.align-items-center > a";

        public HomePage(IPage page) : base(page) { }

        /// <summary>
        /// Open home page (BaseUrl from BasePage/config.json)
        /// </summary>
        public async Task OpenAsync()
        {
            await Page.GotoAsync(BaseUrl);
            await Page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);
            await Page.WaitForTimeoutAsync(2000); // Visual pause
        }

        /// <summary>
        /// Navigate to admin page
        /// </summary>
        public async Task GoAdminAsync()
        {
            await Page.WaitForSelectorAsync(adminLink, new PageWaitForSelectorOptions
            {
                State = WaitForSelectorState.Visible,
                Timeout = 5000
            });

            await Page.ClickAsync(adminLink);
            await Page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);
            await Page.WaitForTimeoutAsync(2000);
        }

        /// <summary>
        /// Go back to front/home page by navigating directly
        /// </summary>
        public async Task BackFrontAsync()
        {
            await Page.GotoAsync(BaseUrl);
            await Page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);
            await Page.WaitForTimeoutAsync(2000); // Visual pause
        }

        /// <summary>
        /// Scroll down one third of the page
        /// </summary>
        public async Task ScrollDownOneThirdAsync()
        {
            await ScrollToOneThirdAsync();
            await Page.WaitForTimeoutAsync(500); // Short visual pause
        }

        /// <summary>
        /// Open second room card
        /// </summary>
        public async Task OpenSecondRoomAsync()
        {
            await Page.WaitForSelectorAsync(secondRoomLink, new PageWaitForSelectorOptions
            {
                State = WaitForSelectorState.Visible,
                Timeout = 5000
            });

            await Page.ClickAsync(secondRoomLink);
            await Page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);
            await Page.WaitForTimeoutAsync(2000); // Visual pause
        }
    }
}
