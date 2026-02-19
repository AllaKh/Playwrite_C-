using Microsoft.Playwright;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Threading.Tasks;

namespace Pages
{
    /// <summary>
    /// Base class for all page objects
    /// Handles common operations such as navigation, scrolling, and element visibility
    /// </summary>
    public class BasePage
    {
        /// <summary>
        /// Playwright page instance
        /// </summary>
        protected readonly IPage Page;

        /// <summary>
        /// Base URL loaded from configuration
        /// </summary>
        private readonly string _baseUrl;

        /// <summary>
        /// Constructor that initializes the page and loads the base URL from config.json
        /// </summary>
        /// <param name="page">Playwright IPage instance</param>
        public BasePage(IPage page)
        {
            Page = page;

            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("config.json", optional: false, reloadOnChange: true)
                .Build();

            _baseUrl = config["baseURL"] ?? throw new InvalidDataException("Base URL is not defined in config.json");
        }

        /// <summary>
        /// Navigates to a relative URL using the base URL
        /// </summary>
        /// <param name="relativeUrl">Relative path to navigate to</param>
        public async Task NavigateAsync(string relativeUrl)
        {
            await Page.GotoAsync($"{BaseUrl}{relativeUrl}");
        }

        /// <summary>
        /// Checks if an element matching the selector is visible on the page
        /// </summary>
        /// <param name="selector">CSS selector of the element</param>
        /// <returns>True if visible, otherwise false</returns>
        public async Task<bool> IsVisibleAsync(string selector)
        {
            var element = await Page.QuerySelectorAsync(selector);
            if (element == null) return false;
            return await element.IsVisibleAsync();
        }

        /// <summary>
        /// Scrolls to the bottom of the page
        /// </summary>
        public async Task ScrollToBottomAsync()
        {
            await Page.EvaluateAsync("() => window.scrollBy(0, document.body.scrollHeight)");
        }

        /// <summary>
        /// Scrolls to the top of the page
        /// </summary>
        public async Task ScrollToTopAsync()
        {
            await Page.EvaluateAsync("() => window.scrollTo(0, 0)");
        }

        /// <summary>
        /// Scrolls down one third of the page height
        /// </summary>
        public async Task ScrollToOneThirdAsync()
        {
            await Page.EvaluateAsync("() => window.scrollBy(0, document.body.scrollHeight / 3)");
        }

        /// <summary>
        /// Gets the base URL from configuration
        /// </summary>
        public string BaseUrl => _baseUrl;
    }
}
