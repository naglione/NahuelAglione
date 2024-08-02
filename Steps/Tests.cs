using System;
using System.Threading.Tasks;
using Microsoft.Playwright;
using PlaywrightTest.Pages;
using Microsoft.Playwright.NUnit;
using NUnit.Framework;
using FluentAssertions;

namespace PlaywrightTests;

public class Tests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public async Task CorrectLogin()
    {
        using var playwright = await Playwright.CreateAsync();
        await using var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
        {
            Headless = false
        });
        var page = await browser.NewPageAsync();
        await page.GotoAsync(url: "https://www.saucedemo.com/");
        
        var saucePage = new LoginPage(page);

        await saucePage.LoginAsync("standard_user", "secret_sauce");

    }

    [Test]
    public async Task ValidateLockedOutUser()
    {
        using var playwright = await Playwright.CreateAsync();
        await using var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
        {
            Headless = false
        });
        var page = await browser.NewPageAsync();
        await page.GotoAsync(url: "https://www.saucedemo.com/");

        var saucePage = new LoginPage(page);

        await saucePage.LoginAsync("locked_out_user", "secret_sauce");
        string textContent = await saucePage.GetLockedOutUserMsg();
        textContent.Should().Be("Epic sadface: Sorry, this user has been locked out.");

        Thread.Sleep(5000);
    }
}