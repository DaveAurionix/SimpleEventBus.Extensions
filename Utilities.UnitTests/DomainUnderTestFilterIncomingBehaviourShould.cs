using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleEventBus.Abstractions;
using SimpleEventBus.Abstractions.Incoming;
using System;
using System.Threading.Tasks;

namespace SimpleEventBus.Extensions.Utilities.UnitTests
{
    [TestClass]
    public class DomainUnderTestFilterIncomingBehaviourShould
    {
        bool nextActionWasCalled;

        [TestMethod]
        public async Task AllowThroughMessagesWithoutADomainUnderTestHeader()
        {
            var behaviour = new DomainUnderTestFilterIncomingBehaviour(
                "A",
                NullLogger<DomainUnderTestFilterIncomingBehaviour>.Instance);

            var message = new IncomingMessage(
                id: Guid.NewGuid().ToString(),
                body: null,
                messageTypeNames: new[] { "test" },
                dequeuedUtc: DateTime.UtcNow,
                lockExpiresUtc: DateTime.UtcNow,
                1);

            await behaviour
                .Process(message, new Context(null), NextAction)
                .ConfigureAwait(false);

            Assert.IsTrue(nextActionWasCalled);
        }

        [DataTestMethod]
        [DataRow("A", "A", true)]
        [DataRow("A", "B", false)]
        [DataRow("A", "A.A", true)]
        [DataRow("A.B", "A.A", false)]
        [DataRow("A.B", "A.B", true)]
        public async Task AutomaticallyCompleteMessagesWithADomainUnderTestHeaderThatExcludesTheCurrentEndpoint(string domainUnderTestHeaderValue, string endpointName, bool expectedToBeProcessed)
        {
            var behaviour = new DomainUnderTestFilterIncomingBehaviour(
                endpointName,
                NullLogger<DomainUnderTestFilterIncomingBehaviour>.Instance);

            var message = new IncomingMessage(
                id: Guid.NewGuid().ToString(),
                body: null,
                messageTypeNames: new[] { "test" },
                dequeuedUtc: DateTime.UtcNow,
                lockExpiresUtc: DateTime.UtcNow,
                1,
                headers: new HeaderCollection
                {
                    { Constants.HeaderName, domainUnderTestHeaderValue }
                });

            await behaviour
                .Process(message, new Context(null), NextAction)
                .ConfigureAwait(false);

            Assert.AreEqual(expectedToBeProcessed, nextActionWasCalled);
        }

        private Task NextAction(IncomingMessage message, Context context)
        {
            nextActionWasCalled = true;
            return Task.CompletedTask;
        }
    }
}
