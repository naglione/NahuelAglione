Feature: PulsarComplexMessage

@pulsar_test
Scenario: Pulsar Complex Message
	When I send a complex message to the topic
	Then I should receive the complex message from the topic