using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Reqnroll;
using FluentAssertions;

namespace PlaywrightTest.Steps
{
    [Binding]
    public class RabbitMqTestStepDefinitions
    {

        private readonly BDHooks _rabbitMqHooks;
        private string _consumedMessage;
        private string _message;

        public RabbitMqTestStepDefinitions(BDHooks rabbitMqHooks)
        {
            _rabbitMqHooks = rabbitMqHooks;
        }


        [Given(@"a RabbitMQ queue named (.*)")]
        public async Task GivenARabbitMQQueueNamed(string queueName)
        {
            _rabbitMqHooks.RabbitMqChannel.QueueDeclare(queue: queueName,
            durable: false,
            exclusive: false,
            autoDelete: false,
            arguments: null);
        }

        [When(@"I publish a message to the queue (.*)")]
        public async Task WhenIPublishASimpleMessageToTheQueue(string queueName)
        {
            var message = "{ \"type\": \"simple\", \"content\": \"Hello, World\" }";
            _message = message;
            var body = Encoding.UTF8.GetBytes(message);

            _rabbitMqHooks.RabbitMqChannel.BasicPublish(exchange: "",
                routingKey: queueName,
                basicProperties: null,
                body: body);
        }

        [Then(@"the message from (.*) should be consumed and validated")]
        public async Task ThenTheMessageShouldBeConsumedAndValidated(string queueName)
        {
            var consumer = new EventingBasicConsumer(_rabbitMqHooks.RabbitMqChannel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                _consumedMessage = Encoding.UTF8.GetString(body);
            };

            _rabbitMqHooks.RabbitMqChannel.BasicConsume(queue: queueName,
                autoAck: true,
                consumer: consumer);

            Task.Delay(1000).Wait(); // Esperar a que el mensaje sea consumido

            _consumedMessage.Should().Be(_message);
        }

        [When(@"I publish a complex message to the queue")]
        public async Task WhenIPublishAComplexMessageToTheQueue()
        {
            var message = "{ \"type\": \"complex\", \"content\": { \"text\": \"Hello\", \"number\": 123 } }";
            _message = message;
            var body = Encoding.UTF8.GetBytes(message);

            _rabbitMqHooks.RabbitMqChannel.BasicPublish(exchange: "",
                routingKey: "complex_queue",
                basicProperties: null,
                body: body);
        }


        [When(@"I publish a list message to the queue")]
        public async Task WhenIPublishAListMessageToTheQueue()
        {
            var message = "{ \"type\": \"list\", \"content\": [\"item1\", \"item2\", \"item3\"] }";
            _message = message;
            var body = Encoding.UTF8.GetBytes(message);

            _rabbitMqHooks.RabbitMqChannel.BasicPublish(exchange: "",
                routingKey: "list_queue",
                basicProperties: null,
                body: body);
        }
    }
}
