using TechTalk.SpecFlow;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;

namespace PlaywrightTest.Hooks;

[Binding]
public sealed class Hooks
{
    MockApi mockApi = new MockApi();

    [BeforeScenario]
    public void SetUp()
    {
        mockApi.StartServer();
        mockApi.CreateHelloWorldStub();
    }

    [AfterScenario]
    public void StopServer()
    { 
        mockApi.StopServer();
    }
    


    /*
    private readonly IObjectContainer _objectContainer;
    private readonly ScenarioContext _scenarioContext;

    public Hooks(IObjectContainer objectContainer, ScenarioContext scenarioContext)
    {
        _objectContainer = objectContainer;
        _scenarioContext = scenarioContext;
    }

    
    [BeforeScenario]
    public async Task SetupPlaywright()
    {
        var pw = await Playwright.CreateAsync();
        var browser = await pw.Chromium.LaunchAsync(new BrowserTypeLaunchOptions { Headless = false });
        var browserContext = await browser.NewContextAsync(new BrowserNewContextOptions { BypassCSP = true });
        var page = await browserContext.NewPageAsync();
        _objectContainer.RegisterInstanceAs(browser);
        _objectContainer.RegisterInstanceAs(page);
    }
    */
}