using SimpleEventBus.Abstractions.Outgoing;
using SimpleEventBus.Testing;

namespace SimpleEventBus.Extensions.Utilities.IntegrationTests
{
    public class TestSecondEventHandler : CapturingHandlerBase<TestSecondEvent>
    {
        public TestSecondEventHandler(OutgoingHeaderProviders outgoingHeaderProviders)
            : base(outgoingHeaderProviders)
        {
        }
    }
}
