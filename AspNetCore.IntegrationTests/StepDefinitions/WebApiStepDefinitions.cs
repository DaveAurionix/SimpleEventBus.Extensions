using GherkinSpec.TestModel;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SimpleEventBus.Extensions.AspNetCore.IntegrationTests.StepDefinitions
{
    [Steps]
    public class WebApiStepDefinitions
    {
        private readonly WebApplicationFactory<TestSite.Startup> webApplicationFactory;
        private readonly TestContext testContext;

        public WebApiStepDefinitions(WebApplicationFactory<TestSite.Startup> webApplicationFactory, TestContext testContext)
        {
            this.webApplicationFactory = webApplicationFactory;
            this.testContext = testContext;
        }

        [When("an HTTP request is made with a Correlation-ID header")]
        public async Task WhenCallIsMade()
        {
            var client = webApplicationFactory.CreateClient();

            using (var message = new HttpRequestMessage(HttpMethod.Put, "/api/values/5"))
            {
                message.Headers.Add("Correlation-ID", testContext.CorrelationId);
                message.Content = new StringContent("\"Hello world\"", Encoding.UTF8, "application/json");

                using (var response = await client
                    .SendAsync(message)
                    .ConfigureAwait(false))
                {
                    response.EnsureSuccessStatusCode();
                }
            }
        }

        [When("an HTTP request is made with a DomainUnderTest header")]
        public async Task WhenCallIsMadeWithDomainUnderTest()
        {
            var client = webApplicationFactory.CreateClient();

            using (var message = new HttpRequestMessage(HttpMethod.Put, "/api/values/5"))
            {
                message.Headers.Add("DomainUnderTest", "SimpleEventBus.Extensions.AspNetCore");
                message.Content = new StringContent("\"Hello world\"", Encoding.UTF8, "application/json");

                using (var response = await client
                    .SendAsync(message)
                    .ConfigureAwait(false))
                {
                    response.EnsureSuccessStatusCode();
                }
            }
        }
    }
}
