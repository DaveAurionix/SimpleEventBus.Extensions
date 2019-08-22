using Microsoft.AspNetCore.Mvc;
using SimpleEventBus.Abstractions.Outgoing;
using SimpleEventBus.Extensions.AspNetCore.TestSite.Contract;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace SimpleEventBus.Extensions.AspNetCore.TestSite.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly IMessagePublisher messagePublisher;

        public ValuesController(IMessagePublisher messagePublisher)
        {
            this.messagePublisher = messagePublisher;
        }

        [HttpPut("{id}")]
        [SuppressMessage("Style", "IDE0060:Remove unused parameter")]
        [SuppressMessage("Style", "CA1801:Remove unused parameter")]
        public async Task<IActionResult> Put(int id, [FromBody] string value)
        {
            await messagePublisher
                .PublishEvent(new TestSiteEvent())
                .ConfigureAwait(false);

            return NoContent();
        }
    }
}
