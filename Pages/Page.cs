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
        private readonly String passwordBox = "[data-test=\"password\"]";
        private readonly String loginBtn = "[data-test=\"login-button\"]";

        private readonly IPage _page;
        public LoginPage(IPage page)
        {
            _page = page;
        }

        public async Task VisitLoginPage() 
        {
            await _page.GotoAsync(url: "https://www.saucedemo.com/");
        }

        public async Task LoginAsync(string username, string password)
        {
            await _page.Locator(userBox).ClickAsync();
            await _page.Locator(userBox).FillAsync(username);
            await _page.Locator(passwordBox).ClickAsync();
            await _page.Locator(passwordBox).FillAsync(password);
            await _page.Locator(loginBtn).ClickAsync();
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
