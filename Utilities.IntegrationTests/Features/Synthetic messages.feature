Feature: Synthetic messages

Background:
	Given an endpoint has subscribed to events

Scenario: Test messages from a different bounded context are ignored
	When a test event is published with a DomainUnderTest header outside this bounded context
	Then the endpoint never receives the test event

Scenario: Test messages from within the same bounded context are processed
	When a test event is published with a DomainUnderTest header inside this bounded context
	Then the endpoint receives the test event

Scenario: Test messages published by a handler are decorated with the same DomainUnderTest header as the original triggering message
	Given a test event was published with a DomainUnderTest header inside this bounded context
	When the endpoint receives the test event and publishes a second event
	Then the second event is received with the correct DomainUnderTest header
