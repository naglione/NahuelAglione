Feature: PulsarMessage

@pulsar_test
Scenario: Produce and consume a simple Message
	When I send a simple 'Pulsar message!' message
	Then I should receive the same message 'Pulsar message!'