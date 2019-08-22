using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleEventBus.Abstractions;
using SimpleEventBus.Abstractions.Outgoing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace SimpleEventBus.Extensions.Utilities.UnitTests
{
    [TestClass]
    public class MessageHeaderFlowHttpMessageHandlerShould
    {
        class Provider : IOutgoingHeaderProvider
        {
            public IEnumerable<Header> GetOutgoingHeaders()
            {
                return new[]
                {
                    new Header("Hello", "World")
                };
            }
        }

        class MockHandler : HttpMessageHandler
        {
            public HttpRequestMessage CapturedRequest;

            protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                CapturedRequest = request;
                return Task.FromResult(new HttpResponseMessage());
            }
        }

        [TestMethod]
        public async Task AddOutgoingMessageHeadersToOutgoingHttpRequests()
        {
            using (var innerHandler = new MockHandler())
            {
                using (var handler = new MessageHeaderFlowHttpMessageHandler(innerHandler, new OutgoingHeaderProviders(new[] { new Provider() })))
                {
                    using (var client = new HttpClient(handler))
                    {
                        using (var requestMessage = new HttpRequestMessage(HttpMethod.Get, new Uri("http://hello.com")))
                        {
                            using (await client
                                .SendAsync(requestMessage)
                                .ConfigureAwait(false))
                            {
                            }
                        }

                        Assert.IsTrue(
                            innerHandler.CapturedRequest.Headers.GetValues("Hello").Single() == "World");
                    }
                }
            }
        }
    }
}
