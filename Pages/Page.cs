using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using System;
using System.Threading.Tasks;
using NUnit.Framework;
using Microsoft.VisualStudio.TestPlatform.Utilities;


namespace PlaywrightTest.Pages
{
    public class LoginPage
    {
        // elements
        private readonly String userBox = "[data-test=\"username\"]"; 

        private readonly IPage _page;
        public LoginPage(IPage page)
        {
            _page = page;
        }

        public async Task LoginAsync(string username, string password)
        {
            await _page.Locator(userBox).ClickAsync();
            await _page.Locator("[data-test=\"username\"]").FillAsync(username);
            await _page.Locator("[data-test=\"password\"]").ClickAsync();
            await _page.Locator("[data-test=\"password\"]").FillAsync(password);
            await _page.Locator("[data-test=\"login-button\"]").ClickAsync();
        }

        public async Task ValidateCorrectLogin()
        {
            await _page.Locator("[data-test=\"inventory-container\"]").IsVisibleAsync();
        }

        public async Task<string> GetLockedOutUserMsg()
        {
            var errorMsg = _page.Locator(".error-message-container > h3:nth-child(1)");
            var textContent = await errorMsg.TextContentAsync();
            return textContent;

        }
    }
}
