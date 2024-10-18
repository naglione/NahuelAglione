using MySql.Data.MySqlClient;
using RabbitMQ.Client;
using Testcontainers.MySql;
using Testcontainers.RabbitMq;
using Reqnroll;
using DotPulsar;
using Testcontainers.Pulsar;
using DotPulsar.Abstractions;
using DotPulsar.Extensions;
using System.Buffers;
using DotNet.Testcontainers.Builders;

[Binding]
public sealed class BDHooks : IAsyncDisposable
{
    private MySqlContainer _mySqlContainer;
    private RabbitMqContainer _rabbitMqContainer;
    public string connectionString { get; private set; }
    public IConnection RabbitMqConnection { get; private set; }
    public IModel RabbitMqChannel { get; private set; }
    private PulsarContainer _pulsarContainer;  // Contenedor de Pulsar
    public IPulsarClient _pulsarClient;       // Cliente de Pulsar
    public IProducer<ReadOnlySequence<byte>> PulsarProducer { get; private set; } // Productor de mensajes de Pulsar
    public IConsumer<ReadOnlySequence<byte>> PulsarConsumer { get; private set; } // Consumidor de mensajes de Pulsar 

    [BeforeScenario]
    public async Task SetUp()
    {
        // Configuración del contenedor MySQL
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

        connection.Close();

        // Configuración del contenedor RabbitMQ
        _rabbitMqContainer = new RabbitMqBuilder()
            .WithUsername("guest")
            .WithPassword("guest")
            .Build();

        await _rabbitMqContainer.StartAsync();

        var factory = new ConnectionFactory()
        {
            HostName = _rabbitMqContainer.Hostname,
            Port = _rabbitMqContainer.GetMappedPublicPort(5672), // Puerto por defecto de RabbitMQ
            UserName = "guest",
            Password = "guest"
        };

        RabbitMqConnection = factory.CreateConnection();
        RabbitMqChannel = RabbitMqConnection.CreateModel();

        RabbitMqChannel.QueueDeclare(
        queue: "test_queue",
        durable: false,
        exclusive: false,
        autoDelete: false,
        arguments: null);
        
        // Iniciar el contenedor de Pulsar
        _pulsarContainer = new PulsarBuilder()
            .WithImage("apachepulsar/pulsar:latest")
            .Build();

        await _pulsarContainer.StartAsync();

        // Crear el cliente de Pulsar
        _pulsarClient = PulsarClient.Builder()
            .ServiceUrl(new Uri(_pulsarContainer.GetBrokerAddress()))
            .Build();

        // Crear el productor de Pulsar para un tópico específico
        PulsarProducer = _pulsarClient.NewProducer()
            .Topic("test-topic")
            .Create();

        // Crear el consumidor de Pulsar para el mismo tópico
        PulsarConsumer = _pulsarClient.NewConsumer()
            .Topic("test-topic")
            .SubscriptionName("sub-1")
            .Create();
    }

    [AfterScenario]
    public async Task StopServer()
    {
        // Cerrar conexión y contenedor de MySQL
        await DisposeAsync();

        // Cerrar conexión y canal de RabbitMQ
        RabbitMqChannel?.Close();
        RabbitMqConnection?.Close();
    }

    public async ValueTask DisposeAsync()
    {
        if (_mySqlContainer != null)
        {
            await _mySqlContainer.DisposeAsync();
        }

        if (_rabbitMqContainer != null)
        {
            await _rabbitMqContainer.DisposeAsync();
        }

        if (_pulsarContainer != null)
        {
            await _pulsarContainer.DisposeAsync();
        }

        if (_pulsarClient != null)
        {
            await _pulsarClient.DisposeAsync();
        }

        if (PulsarProducer != null)
        {
            await PulsarProducer.DisposeAsync();
        }

        if (PulsarConsumer != null)
        {
            await PulsarConsumer.DisposeAsync();
        }
    }
}