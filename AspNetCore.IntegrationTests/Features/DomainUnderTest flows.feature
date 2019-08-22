Feature: DomainUnderTest flows

Scenario: DomainUnderTest header flows from request to any published events
	Given an endpoint has subscribed to website test events
	When an HTTP request is made with a DomainUnderTest header
	Then the endpoint receives the website test event with the correct DomainUnderTest
