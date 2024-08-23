Feature: RabbitMqTestB

@rabbitmq_test
Scenario: multipleFieldsMessage
	Given a RabbitMQ queue named complex_queue
	When I publish a complex message to the queue
	Then the message from complex_queue should be consumed and validated