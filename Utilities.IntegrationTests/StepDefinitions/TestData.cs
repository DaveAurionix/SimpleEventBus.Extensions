using System;

namespace SimpleEventBus.Extensions.Utilities.IntegrationTests.StepDefinitions
{
    public class TestData
    {
        public string TestEventContent { get; } = Guid.NewGuid().ToString();
    }
}
