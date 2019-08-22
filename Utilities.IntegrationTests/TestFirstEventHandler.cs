using SimpleEventBus.Abstractions.Outgoing;
using SimpleEventBus.Testing;
using System.Threading.Tasks;

namespace SimpleEventBus.Extensions.Utilities.IntegrationTests
{
    public class TestFirstEventHandler : CapturingHandlerBase<TestFirstEvent>
    {
        private readonly IMessagePublisher messagePublisher;

        public TestFirstEventHandler(IMessagePublisher messagePublisher, OutgoingHeaderProviders outgoingHeaderProviders)
            : base(outgoingHeaderProviders)
        {
            this.messagePublisher = messagePublisher;
        }

        public override async Task HandleMessage(TestFirstEvent message)
        {
            await base
                .HandleMessage(message)
                .ConfigureAwait(false);

            await messagePublisher
                .PublishEvent(new TestSecondEvent { Property = message.Property })
                .ConfigureAwait(false);
        }
    }
}
