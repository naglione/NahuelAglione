using TechTalk.SpecFlow;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;

namespace PlaywrightTest
{
    [Binding]
    public sealed class Hooks
    {
        MockApi mockApi = new MockApi();
        Customers customers = new Customers();

        [BeforeScenario]
        public void SetUp()
        {
            mockApi.StartServer();
            mockApi.CreateHelloWorldStub();
            customers.StartMySqlContainer();
        }

        [AfterScenario]
        public void StopServer()
        {
            mockApi.StopServer();
            customers.StopMySqlContainer();
        }
    }
}