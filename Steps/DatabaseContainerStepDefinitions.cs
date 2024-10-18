using FluentAssertions;
using MySql.Data.MySqlClient;
using Reqnroll;

namespace PlaywrightTest.Steps
{
    [Binding]
    public class DatabaseContainerStepDefinitions
    {
        private readonly BDHooks _hooks;
        private MySqlConnection connection;
        public DatabaseContainerStepDefinitions(BDHooks hooks) 
        {
            _hooks = hooks;
        }

        [Given(@"I have a running MySQL Container")]
        public async Task GivenIHaveARunningMySQLContainer()
        {
            connection = new MySqlConnection(_hooks.connectionString);
            await connection.OpenAsync();
            var command = connection.CreateCommand();
        }

        [Then(@"the database should be accessible")]
        public async Task ThenTheDatabaseShouldBeAccessible()
        {
            MySqlCommand command = new MySqlCommand();
            command.Connection = connection;
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
