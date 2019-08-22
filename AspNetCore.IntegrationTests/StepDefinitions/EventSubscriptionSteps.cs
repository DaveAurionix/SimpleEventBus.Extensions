using GherkinSpec.TestModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace SimpleEventBus.Extensions.AspNetCore.IntegrationTests.StepDefinitions
{
    [Steps]
    public class EventSubscriptionSteps
    {
        private readonly TestSiteEventHandler testEventHandler;
        private readonly TestContext testData;

        public EventSubscriptionSteps(TestSiteEventHandler testEventHandler, TestContext testData)
        {
            this.testEventHandler = testEventHandler;
            this.testData = testData;
        }

        [Given("an endpoint has subscribed to website test events")]
        public static void GivenATestEventSubscriptionIsSetUp()
        {
        }

        [Then("the endpoint receives the website test event with the correct Correlation-ID")]
        [EventuallySucceeds]
        public void ThenTheTestEventIsReceived()
        {
            Assert.AreEqual(
                1,
                testEventHandler.ReceivedMessages.Count(
                    @capturedMessage => @capturedMessage.CorrelationId == testData.CorrelationId));
        }

        [Then("the endpoint receives the website test event with the correct DomainUnderTest")]
        [EventuallySucceeds]
        public void ThenTheTestEventIsReceivedWithCorrectDomainUnderTest()
        {
            Assert.AreEqual(
                1,
                testEventHandler.ReceivedMessages.Count(
                    @capturedMessage => @capturedMessage.DomainUnderTest == "SimpleEventBus.Extensions.AspNetCore"));
        }
    }
}
