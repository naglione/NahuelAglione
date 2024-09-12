using FluentAssertions;
using TechTalk.SpecFlow;
using DotPulsar.Abstractions;
using System.Buffers;
using System.Text;
using DotPulsar.Extensions;
using Newtonsoft.Json.Linq;

namespace PlaywrightTest.Steps
{
    [Binding]
    public class PulsarMessageStepDefinitions
    {
        private readonly IProducer<ReadOnlySequence<byte>> _pulsarProducer;
        private readonly IConsumer<ReadOnlySequence<byte>> _pulsarConsumer;

        public PulsarMessageStepDefinitions(BDHooks hooks) {
            _pulsarProducer = hooks.PulsarProducer;
            _pulsarConsumer = hooks.PulsarConsumer;
        }

        [When(@"I send a simple '(.*)' message")]
        public async Task WhenISendASimpleMessage(string message)
        {
            var messageBytes = Encoding.UTF8.GetBytes(message);
            await _pulsarProducer.Send(messageBytes);
        }

        [Then(@"I should receive the same message '(.*)'")]
        public async Task ThenIShouldReceiveTheSameMessage(string expectedMessage)
        {
            var message = await _pulsarConsumer.Receive();
            var receivedData = Encoding.UTF8.GetString(message.Data.ToArray());

            expectedMessage.Should().Be(receivedData);
            await _pulsarConsumer.Acknowledge(message);
        }

        [When(@"I send a complex message to the topic")]
        public async Task WhenISendAComplexMessageToTheTopic()
        {
            var complexMessage = "{\"type\":\"complex\", \"content\":{\"text\":\"Hello Pulsar\", \"number\":123}}";
            var messageBytes = Encoding.UTF8.GetBytes(complexMessage);

            await _pulsarProducer.Send(messageBytes);
        }

        [Then(@"I should receive the complex message from the topic")]
        public async Task ThenIShouldReceiveTheComplexMessageFromTheTopic()
        {
            var message = await _pulsarConsumer.Receive();
            var receivedData = Encoding.UTF8.GetString(message.Data.ToArray());

            var expectedMessage = "{\"type\":\"complex\", \"content\":{\"text\":\"Hello Pulsar\", \"number\":123}}";
            receivedData.Should().Be(expectedMessage, "the received message should match the complex message");

            await _pulsarConsumer.Acknowledge(message);
        }

        [When(@"I send a list message to the topic")]
        public async Task WhenISendAListMessageToTheTopic()
        {
            var listMessage = new JObject
            {
                { "type", "list" },
                { "content", new JArray("item1", "item2", "item3") }
            };
            var messageString = listMessage.ToString();
            var messageBytes = Encoding.UTF8.GetBytes(messageString);

            await _pulsarProducer.Send(messageBytes);
        }

        [Then(@"I should receive the list message from the topic")]
        public async Task ThenIShouldReceiveTheListMessageFromTheTopic()
        {
            var message = await _pulsarConsumer.Receive();
            var receivedData = Encoding.UTF8.GetString(message.Data.ToArray());
            var receivedJson = JObject.Parse(receivedData);

            var expectedJson = new JObject
            {
                { "type", "list" },
                { "content", new JArray("item1", "item2", "item3") }
            };
            receivedJson.Should().BeEquivalentTo(expectedJson, "the received message should match the list message");

            await _pulsarConsumer.Acknowledge(message);
        }
    }
}