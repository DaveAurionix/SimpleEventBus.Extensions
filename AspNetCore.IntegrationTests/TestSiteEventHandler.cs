using SimpleEventBus.Abstractions.Outgoing;
using SimpleEventBus.Extensions.AspNetCore.TestSite.Contract;
using SimpleEventBus.Testing;

namespace SimpleEventBus.Extensions.AspNetCore.IntegrationTests
{
    public class TestSiteEventHandler : CapturingHandlerBase<TestSiteEvent>
    {
        public TestSiteEventHandler(OutgoingHeaderProviders outgoingHeaderProviders)
            : base(outgoingHeaderProviders)
        {
        }
    }
}