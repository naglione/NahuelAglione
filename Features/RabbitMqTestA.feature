Feature: RabbitMqTestA

@rabbitmq_test
Scenario: simpleMessage
	Given a RabbitMQ queue named simple_queue
    When I publish a message to the queue simple_queue
    Then the message from simple_queue should be consumed and validated