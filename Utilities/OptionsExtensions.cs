using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using SimpleEventBus.Abstractions.Incoming;
using SimpleEventBus.Abstractions.Outgoing;
using SimpleEventBus.Extensions.Utilities;

namespace SimpleEventBus
{
    public static class OptionsExtensions
    {
        public static Options UseDomainUnderTestFilter(this Options options)
        {
            options.Services.AddSingleton(
                sp => new DomainUnderTestFilterIncomingBehaviour(
                    options.EndpointName,
                    sp.GetService<ILogger<DomainUnderTestFilterIncomingBehaviour>>() ?? NullLogger<DomainUnderTestFilterIncomingBehaviour>.Instance));

            options.Services.AddSingleton<IIncomingBehaviour>(
                sp => sp.GetRequiredService<DomainUnderTestFilterIncomingBehaviour>());

            options.Services.AddSingleton<IOutgoingHeaderProvider>(
                sp => sp.GetRequiredService<DomainUnderTestFilterIncomingBehaviour>());

            return options;
        }
    }
}
