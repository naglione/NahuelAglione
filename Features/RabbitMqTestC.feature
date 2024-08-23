Feature: RabbitMqTestC

@rabbitmq_test
Scenario: listQueue
	Given a RabbitMQ queue named list_queue
	When I publish a list message to the queue
	Then the message from list_queue should be consumed and validated