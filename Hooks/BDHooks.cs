using MySql.Data.MySqlClient;
using TechTalk.SpecFlow;
using Testcontainers.MySql;

[Binding]
public sealed class BDHooks
{
    private MySqlContainer _mySqlContainer;
    public string connectionString;

    [BeforeScenario]
    public async Task SetUp()
    {
        _mySqlContainer = new MySqlBuilder()
                .WithDatabase("Customers")
                .Build();
        await _mySqlContainer.StartAsync();
        connectionString = _mySqlContainer.GetConnectionString();
        MySqlConnection connection = new MySqlConnection(connectionString);
        
        connection.Open();
        MySqlCommand command = new MySqlCommand();
        command.Connection = connection;
            command.CommandText = @"
                    CREATE TABLE Users (
                        Username VARCHAR(255) NOT NULL PRIMARY KEY,
                        Password VARCHAR(255) NOT NULL
                    );";
            command.ExecuteNonQuery();
            command.CommandText = @"
                INSERT INTO Users (Username, Password) VALUES
                ('standard_user', 'secret_sauce'),
                ('locked_out_user', 'secret_sauce');";
            command.ExecuteNonQuery();
        
    }

    [AfterScenario]
    public async Task StopServer()
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