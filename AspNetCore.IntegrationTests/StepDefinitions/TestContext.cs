using System;

namespace SimpleEventBus.Extensions.AspNetCore.IntegrationTests.StepDefinitions
{
    public class TestContext
    {
        public string CorrelationId { get; set; } = Guid.NewGuid().ToString();
    }
}
