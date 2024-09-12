Feature: PulsarListMessage

@pulsar_test
Scenario: List message
	When I send a list message to the topic
	Then I should receive the list message from the topic