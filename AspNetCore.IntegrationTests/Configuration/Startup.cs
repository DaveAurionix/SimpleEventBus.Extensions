using GherkinSpec.TestModel;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using SimpleEventBus.Extensions.AspNetCore.IntegrationTests.StepDefinitions;
using System.IO;
using System.Threading.Tasks;
using TestWebApplicationStartup = SimpleEventBus.Extensions.AspNetCore.TestSite.Startup;

namespace SimpleEventBus.Extensions.AspNetCore.IntegrationTests.Configuration
{
    [Steps]
    public static class Startup
    {
        private static Endpoint endpoint;
        private static string messagesStoragePath;

        [BeforeRun]
        public static async Task Setup(TestRunContext testRunContext)
        {
            var services = new ServiceCollection();

            messagesStoragePath = Path.Combine(Directory.GetCurrentDirectory(), "messages");

            testRunContext.ServiceProvider = services
                .AddScoped<TestContext>()
                .AddAllStepsClassesAsScoped()
                .AddSingleton(
                    sp => new WebApplicationFactory<TestWebApplicationStartup>()
                        .WithWebHostBuilder(
                            builder => builder.UseSetting("FileBusMessagesPath", messagesStoragePath)))
                .AddSimpleEventBus(
                    options => options
                        .UseEndpointName(typeof(TestSiteEventHandler).Namespace)
                        .UseDomainUnderTestFilter()
                        .UseFileBus(messagesStoragePath)
                        .UseSingletonHandlersIn(typeof(TestSiteEventHandler).Assembly))
                .BuildServiceProvider();

            endpoint = testRunContext.ServiceProvider.GetRequiredService<Endpoint>();
            await endpoint
                .StartListening()
                .ConfigureAwait(false);
        }

        [AfterRun]
        public static async Task Teardown(TestRunContext testRunContext)
        {
            if (endpoint != null)
            {
                await endpoint
                    .ShutDown()
                    .ConfigureAwait(false);
            }

            if (Directory.Exists(messagesStoragePath))
            {
                Directory.Delete(messagesStoragePath, true);
            }

            var typedProvider = (ServiceProvider)testRunContext.ServiceProvider;
            typedProvider.Dispose();
        }
    }
}
