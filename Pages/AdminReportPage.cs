using Microsoft.Playwright;
using System.Threading.Tasks;

namespace Pages
{
    /// <summary>
    /// Page object for the admin report page
    /// Handles checking bookings and logout
    /// </summary>
    public class AdminReportPage : BasePage
    {
        private readonly string eventSelector = "div.rbc-event-content";
        private readonly string logoutSelector = "#navbarSupportedContent > ul.navbar-nav.ms-auto > li:nth-child(2) > button";

        public AdminReportPage(IPage page) : base(page) { }

        /// <summary>
        /// Open admin report page
        /// </summary>
        public async Task OpenAsync()
        {
            await NavigateAsync("/admin/report");
            await Page.WaitForTimeoutAsync(2000);
        }

        /// <summary>
        /// Search booking by full name in report
        /// </summary>
        public async Task<bool> FindBookingInTableAsync(string fullName)
        {
            await Page.WaitForTimeoutAsync(2000);
            var events = await Page.QuerySelectorAllAsync(eventSelector);

            foreach (var ev in events)
            {
                var text = await ev.TextContentAsync();
                if (!string.IsNullOrEmpty(text) && text.Contains(fullName))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Logout
        /// </summary>
        public async Task LogoutAsync()
        {
            await Page.ClickAsync(logoutSelector);
        }
    }
}
