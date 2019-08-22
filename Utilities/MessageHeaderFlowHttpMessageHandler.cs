using SimpleEventBus.Abstractions.Outgoing;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace SimpleEventBus.Extensions.Utilities
{
    public class MessageHeaderFlowHttpMessageHandler : DelegatingHandler
    {
        private readonly OutgoingHeaderProviders outgoingHeaderProviders;

        public MessageHeaderFlowHttpMessageHandler(HttpMessageHandler innerHandler, OutgoingHeaderProviders outgoingHeaderProviders)
             : base(innerHandler)
        {
            this.outgoingHeaderProviders = outgoingHeaderProviders;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            foreach (var header in outgoingHeaderProviders.GetOutgoingHeaders())
            {
                request.Headers.Add(header.HeaderName, header.Value);
            }

            return base.SendAsync(request, cancellationToken);
        }
    }
}
