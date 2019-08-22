using GherkinSpec.TestModel;
using SimpleEventBus.Abstractions;
using SimpleEventBus.Abstractions.Outgoing;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SimpleEventBus.Extensions.Utilities.IntegrationTests.StepDefinitions
{
    [Steps]
    public class MessageSendingSteps
    {
        private readonly IMessagePublisher messagePublisher;
        private readonly TestData testData;

        public MessageSendingSteps(IMessagePublisher messagePublisher, TestData testData)
        {
            this.messagePublisher = messagePublisher;
            this.testData = testData;
        }

        [When("a test event is published with a DomainUnderTest header outside this bounded context")]
        public async Task WhenAnEventIsPublishedWithDomainUnderTestOutsideThisContext()
        {
            await messagePublisher
                .PublishEvent(
                    new TestFirstEvent
                    {
                        Property = testData.TestEventContent
                    },
                    new HeaderCollection
                    {
                        { "DomainUnderTest", "SimpleEventBus.SomethingElse" }
                    })
                .ConfigureAwait(false);
        }

        [When("a test event is published with a DomainUnderTest header inside this bounded context")]
        [Given("a test event was published with a DomainUnderTest header inside this bounded context")]
        public async Task WhenAnEventIsPublishedWithDomainUnderTestInsideThisContext()
        {
            await messagePublisher
                .PublishEvent(
                    new TestFirstEvent
                    {
                        Property = testData.TestEventContent
                    },
                    new HeaderCollection
                    {
                        { "DomainUnderTest", "SimpleEventBus.Extensions.Utilities.IntegrationTests" }
                    })
                .ConfigureAwait(false);
        }
    }
}
