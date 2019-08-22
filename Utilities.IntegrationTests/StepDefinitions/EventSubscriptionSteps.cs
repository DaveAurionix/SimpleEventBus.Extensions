using GherkinSpec.TestModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace SimpleEventBus.Extensions.Utilities.IntegrationTests.StepDefinitions
{
    [Steps]
    public class EventSubscriptionSteps
    {
        private readonly TestFirstEventHandler testEventHandler;
        private readonly TestSecondEventHandler testSecondEventHandler;
        private readonly TestData testData;

        public EventSubscriptionSteps(TestFirstEventHandler testEventHandler, TestSecondEventHandler testSecondEventHandler, TestData testData)
        {
            this.testEventHandler = testEventHandler;
            this.testSecondEventHandler = testSecondEventHandler;
            this.testData = testData;
        }

        [When("the endpoint receives the test event and publishes a second event")]
        public static void GivenATestEventSubscriptionIsSetUp()
        {
            // No operation to perform, this is done by startup logic or code inside the test event handlers.
        }

        [Then("the endpoint receives the test event")]
        [EventuallySucceeds]
        public void ThenTheTestEventIsReceived()
        {
            Assert.AreEqual(
                1,
                testEventHandler.ReceivedMessages.Count(
                    @event => @event.Message.Property == testData.TestEventContent));
        }

        [Then("the endpoint never receives the test event")]
        [MustNotEventuallyFail]
        public void ThenTheTestEventIsNeverReceived()
        {
            Assert.AreEqual(
                0,
                testEventHandler.ReceivedMessages.Count(
                    @event => @event.Message.Property == testData.TestEventContent));
        }

        [Then("the second event is received with the correct DomainUnderTest header")]
        [EventuallySucceeds]
        public void ThenTheSecondTestEventIsReceivedWithCorrectDomainUnderTest()
        {
            Assert.AreEqual(
                1,
                testSecondEventHandler.ReceivedMessages.Count(
                    @event => @event.Message.Property == testData.TestEventContent
                        && @event.DomainUnderTest == "SimpleEventBus.Extensions.Utilities.IntegrationTests"));
        }
    }
}
