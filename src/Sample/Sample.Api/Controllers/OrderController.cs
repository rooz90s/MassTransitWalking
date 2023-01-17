using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sample.Contract;
using Sample.Contract.SagaEvents;

namespace Sample.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {

        private readonly ILogger<OrderController> _logger;
        private readonly IRequestClient<ISubmitOrder> _submitOrderClient;
        private readonly IRequestClient<IOrderCheck> _OrderCheck;
        private readonly IPublishEndpoint _publishEndpoint;

        public OrderController(ILogger<OrderController> logger, IRequestClient<ISubmitOrder> submitOrderClient, IRequestClient<IOrderCheck> orderCheck, IPublishEndpoint publishEndpoint)
        {
            _logger = logger;
            _submitOrderClient = submitOrderClient;
            _OrderCheck = orderCheck;
            _publishEndpoint = publishEndpoint;
        }

        [HttpGet("giveguid")]
        public async Task<IActionResult> GetMeGUID()
        {
            return Ok(Guid.NewGuid());

        }

        [HttpPost]
        public async Task<IActionResult> Post(Guid Id,string CustomerNumber)
        {

            var response = await _submitOrderClient.GetResponse<IOrderSubmissionAccepterResponse>(new
            {
                OrderId = Id,
                Timestamp = InVar.Timestamp,
                CustomerNumber = CustomerNumber
            });


            return Ok(response.Message);

        }


        [HttpGet]
        public async Task<IActionResult> GetOrderStatus(Guid OrderId)
        {
            var response = await _OrderCheck.GetResponse<IOrderStatus>(new
            {
                OrderId = OrderId
            });


            return Ok(response.Message);

        }

        [HttpPatch]
        public async Task<IActionResult> AcceptOrder(Guid OrderId)
        {

            await _publishEndpoint.Publish<IOrderAccepted>(new
            {
                OrderId = OrderId,
                Timestamp = InVar.Timestamp
            });

            return Ok("Order Accepted Just Fired");

        }

    }
}
