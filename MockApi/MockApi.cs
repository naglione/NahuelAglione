using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;


    public class MockApi
    {
        private WireMockServer server;

        public void StartServer()
        {
            server = WireMockServer.Start(9876);
        }

        public void CreateHelloWorldStub()
        {
            server.Given(
                Request.Create().WithPath("/hello-world").UsingGet()
            )
            .RespondWith(
                Response.Create()
                    .WithStatusCode(200)
                    .WithHeader("Content-Type", "text/plain")
                    .WithBody("Hello, world!")
            );
        }


        public void StopServer()
        {
            server.Stop();
        }
    }
