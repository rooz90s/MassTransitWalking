using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sample.Contract.SagaEvents;

namespace Sample.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {

        private readonly IPublishEndpoint _endpoint;

        public CustomerController(IPublishEndpoint endpoint)
        {
            _endpoint = endpoint;
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Guid Id,string CustomerNumber)
        {
            await _endpoint.Publish<ICustomerAccountClosed>(new
            {
                CustomerId = Id,
                CustomerNumber = CustomerNumber
                
            });

            return Ok(Id);
        }

    }
}
