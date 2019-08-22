using Microsoft.AspNetCore.Http;
using SimpleEventBus.Abstractions;
using SimpleEventBus.Abstractions.Outgoing;
using System.Collections.Generic;

namespace SimpleEventBus.Extensions.AspNetCore
{
    class AspNetCoreRequestHeaderProvider : IOutgoingHeaderProvider
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        private readonly IEnumerable<string> headersToFlowFromRequestToOutboundMessages;

        public AspNetCoreRequestHeaderProvider(IHttpContextAccessor httpContextAccessor, IEnumerable<string> headersToFlowFromRequestToOutboundMessages)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.headersToFlowFromRequestToOutboundMessages = headersToFlowFromRequestToOutboundMessages;
        }

        public IEnumerable<Header> GetOutgoingHeaders()
        {
            var requestHeaders = httpContextAccessor.HttpContext.Request.Headers;

            foreach (var header in headersToFlowFromRequestToOutboundMessages)
            {
                if (!requestHeaders.TryGetValue(header, out var values))
                {
                    continue;
                }

                // TODO Test case when multiple headers exist
                yield return new Header(header, values.ToString());
            }
        }
    }
}
