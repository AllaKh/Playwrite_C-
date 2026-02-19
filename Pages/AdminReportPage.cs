using Microsoft.Playwright;
using System.Threading.Tasks;

namespace Pages
{
    public class AdminReportPage : BasePage
    {
        /// <summary>
        /// Selector for the report table
        /// </summary>
        public readonly string ReportTableSelector = "#report-table";

        /// <summary>
        /// Selector for the logout button
        /// </summary>
        public readonly string LogoutButton = "#logout-btn";

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="page">Playwright IPage instance</param>
        public AdminReportPage(IPage page) : base(page) { }

        /// <summary>
        /// Navigates to the admin report page
        /// </summary>
        public async Task OpenAsync()
        {
            await NavigateAsync("/admin/report");
        }

        /// <summary>
        /// Checks if a booking with the specified full name exists in the report table
        /// </summary>
        /// <param name="fullName">Full name to search for</param>
        /// <returns>True if found, otherwise false</returns>
        public async Task<bool> FindBookingInTableAsync(string fullName)
        {
            var rows = await Page.QuerySelectorAllAsync($"{ReportTableSelector} tr");
            foreach (var row in rows)
            {
                var text = await row.TextContentAsync();
                if (!string.IsNullOrEmpty(text) && text.Contains(fullName))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Clicks the logout button
        /// </summary>
        public async Task LogoutAsync()
        {
            await Page.ClickAsync(LogoutButton);
        }
    }
}
