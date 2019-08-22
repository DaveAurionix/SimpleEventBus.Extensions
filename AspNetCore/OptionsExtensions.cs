using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using SimpleEventBus.Abstractions.Outgoing;
using SimpleEventBus.Extensions.AspNetCore;

namespace SimpleEventBus
{
    public static class OptionsExtensions
    {
        public static Options UseAspNetCoreRequestHeaderFlow(this Options options, params string[] headersToFlowFromRequestToOutboundMessages)
        {
            if (headersToFlowFromRequestToOutboundMessages == null || headersToFlowFromRequestToOutboundMessages.Length == 0)
            {
                headersToFlowFromRequestToOutboundMessages = new[]
                {
                    "Correlation-ID",
                    "DomainUnderTest"
                };
            }

            options.Services.AddSingleton(
                sp => new AspNetCoreRequestHeaderProvider(
                    sp.GetRequiredService<IHttpContextAccessor>(),
                    headersToFlowFromRequestToOutboundMessages));

            options.Services.AddSingleton<IOutgoingHeaderProvider>(
                sp => sp.GetRequiredService<AspNetCoreRequestHeaderProvider>());

            return options;
        }
    }
}
