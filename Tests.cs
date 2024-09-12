/*using TechTalk.SpecFlow;
using Testcontainers.MySql;
using FluentAssertions;
using MySql.Data.MySqlClient;

namespace Training.TestContainers
{
    [Binding]
    public class Tests : IAsyncDisposable
    {
        private MySqlContainer _mySqlContainer;
        private string connectionString;


        [BeforeScenario]
        public async Task StartMySqlContainer()
        {
            _mySqlContainer = new MySqlBuilder()
                .WithDatabase("Customers")
                .Build();
            await _mySqlContainer.StartAsync();
            connectionString = _mySqlContainer.GetConnectionString();
            using (var connection = new MySqlConnection(connectionString))
            {
                await connection.OpenAsync();
                var command = connection.CreateCommand();
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
        }


        [AfterScenario]
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


        [Test]
        public async Task Test1()
        {
            _mySqlContainer = new MySqlBuilder()
                .WithDatabase("Customers")
                .Build();
            await _mySqlContainer.StartAsync();
            connectionString = _mySqlContainer.GetConnectionString();
            using (var connection = new MySqlConnection(connectionString))
            {
                await connection.OpenAsync();
                var command = connection.CreateCommand();
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

                //await connection.OpenAsync();
                command = connection.CreateCommand();
                command.CommandText = @"
                SELECT * FROM Users
                WHERE Username = 'locked_out_user';";
                var result = command.ExecuteScalar();
                result = result.ToString();
                System.Console.WriteLine(result);
                result.Should().Be("locked_out_user");
            }
        }
    }
}*/