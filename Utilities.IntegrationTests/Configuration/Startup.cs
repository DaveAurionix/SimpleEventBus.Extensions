using GherkinSpec.TestModel;
using Microsoft.Extensions.DependencyInjection;
using SimpleEventBus.Extensions.Utilities.IntegrationTests.StepDefinitions;
using System.Threading.Tasks;

namespace SimpleEventBus.Extensions.Utilities.IntegrationTests.Configuration
{
    [Steps]
    public static class Startup
    {
        private static Endpoint endpoint;

        [BeforeRun]
        public static async Task Setup(TestRunContext testRunContext)
        {
            var services = new ServiceCollection();

            testRunContext.ServiceProvider = services
                .AddScoped<TestData>()
                .AddAllStepsClassesAsScoped()
                .AddSimpleEventBus(
                    options => options
                        .UseEndpointName(typeof(TestFirstEventHandler).Namespace)
                        .UseDomainUnderTestFilter()
                        .UseInMemoryBus()
                        .UseSingletonHandlersIn(typeof(TestFirstEventHandler).Assembly))
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

            var typedProvider = (ServiceProvider)testRunContext.ServiceProvider;
            typedProvider.Dispose();
        }
    }
}
