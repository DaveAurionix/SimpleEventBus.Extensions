Feature: Correlation-ID flows

Scenario: Correlation-ID header flows from HTTP request to published events
	Given an endpoint has subscribed to website test events
	When an HTTP request is made with a Correlation-ID header
	Then the endpoint receives the website test event with the correct Correlation-ID
