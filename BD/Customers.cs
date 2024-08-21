using MySql.Data.MySqlClient;
using Testcontainers.MySql;


    internal class Customers
    {
        private MySqlContainer _mySqlContainer;
        private string connectionString;

        public async Task StartMySqlContainer()
        {
            _mySqlContainer = new MySqlBuilder()
                .WithDatabase("Customers")
                .Build();
            await _mySqlContainer.StartAsync();
            connectionString = _mySqlContainer.GetConnectionString();
            
        }

    public async Task StopMySqlContainer()
    {
        await DisposeAsync();
    }

    public async ValueTask DisposeAsync()
    {
        if (_mySqlContainer != null)
        {
            await _mySqlContainer.DisposeAsync();
        }
    }
}