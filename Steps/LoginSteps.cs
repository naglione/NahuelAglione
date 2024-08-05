using FluentAssertions;
using Microsoft.Playwright;
using PlaywrightTest.Hooks;
using PlaywrightTest.Pages;
using TechTalk.SpecFlow;

namespace PlaywrightTests;

[Binding]
public class LoginSteps
{
    private readonly LoginPage _loginPage;
    private readonly IPage _page;

    public LoginSteps(IPage page)
    {
        _page = page;
        _loginPage = new LoginPage(_page);
    }

    [Given("Navigate to saucedemo page")]
    public async Task NavigateToPage()
    {
        await _loginPage.VisitLoginPage();
    }

    [When("Login with (.*) user")]
    public async Task EnterUserData(string user)
    {
        await _loginPage.LoginAsync(user, "secret_sauce");
    }


    [Then("Validate the inventory is loaded")]
    public async Task ValidateInventory()
    {
        await _loginPage.ValidateCorrectLogin();
    }

    [Then("Validate locked out error message")]
    public async Task ThenValidateLockedOutErrorMessage()
    {
        string textContent = await _loginPage.GetLockedOutUserMsg();
        textContent.Should().Be("Epic sadface: Sorry, this user has been locked out.");
    }
}