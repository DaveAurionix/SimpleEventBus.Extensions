using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SimpleEventBus.Abstractions;
using SimpleEventBus.Abstractions.Incoming;
using SimpleEventBus.Abstractions.Outgoing;

namespace SimpleEventBus.Extensions.Utilities
{
    public class DomainUnderTestFilterIncomingBehaviour : IIncomingBehaviour, IOutgoingHeaderProvider
    {
        private static readonly AsyncLocal<string> incomingDomainUnderTestHeaderValue = new AsyncLocal<string>();

        private readonly string endpointName;
        private readonly ILogger<DomainUnderTestFilterIncomingBehaviour> logger;

        public DomainUnderTestFilterIncomingBehaviour(string endpointName, ILogger<DomainUnderTestFilterIncomingBehaviour> logger)
        {
            this.endpointName = endpointName;
            this.logger = logger;
        }

        public Task Process(IncomingMessage message, Context context, IncomingPipelineAction next)
        {
            var domainUnderTest = message.Headers.GetValueOrDefault(Constants.HeaderName);

            if(domainUnderTest != null
                && !string.Equals(endpointName, domainUnderTest, StringComparison.OrdinalIgnoreCase)
                && !endpointName.StartsWith(domainUnderTest + ".", StringComparison.OrdinalIgnoreCase))
            {
                logger.LogInformation(
                    $"Message {message.Id} of type \"{message.MessageTypeNames.First()}\" was ignored because it was part of a test of the \"{domainUnderTest}\" domain and the current endpoint name is \"{endpointName}\" which is outside that domain.");
                return Task.CompletedTask;
            }

            incomingDomainUnderTestHeaderValue.Value = domainUnderTest;

            return next(message, context);
        }

        public IEnumerable<Header> GetOutgoingHeaders()
            => new[] { new Header(Constants.HeaderName, incomingDomainUnderTestHeaderValue.Value) };
    }
}
