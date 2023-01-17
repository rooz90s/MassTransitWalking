using MassTransit;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Warehouse.Contract;

namespace Sample.Component.StateMachines.CourierActivities
{
    public class AllocateInventoryActivity : IActivity<IAllocateInventoryArguments, IAllocateInventoryLog>
    {
        //coupling here // just for test
        private readonly IRequestClient<IAllocateInventory> _client;
        private readonly ILogger<AllocateInventoryActivity> _logger;

        public AllocateInventoryActivity(IRequestClient<IAllocateInventory> client, ILogger<AllocateInventoryActivity> logger)
        {
            _client = client;
            _logger = logger;
        }

         

        public async Task<CompensationResult> Compensate(CompensateContext<IAllocateInventoryLog> context)
        {

            _logger.LogError($"Allocate Inventory Compensate with AllocationId:{context.Log.AllocationId}");

            await context.Publish<IAllocationReleaseRequested>(new
            {
                AllocationId = context.Log.AllocationId,
                Reason = "Order Faulted"
            });

            return context.Compensated();
        }

        public async Task<ExecutionResult> Execute(ExecuteContext<IAllocateInventoryArguments> context)
        {

            

            var orderId = context.Arguments.OrderId;
            var itemNumber = context.Arguments.ItemNumber;
            var quantity = context.Arguments.Quantity;


            _logger.LogInformation($"Allocate Inventory Activity Execute with OrderId:{orderId} with quantity:{quantity}");


            var allocationId = NewId.NextGuid();

            var response = await _client.GetResponse<InventoryAllocated>(new
            {
                AllocationId = allocationId,
                ItemNumber = itemNumber,
                Quantity = quantity
            });

            return context.Completed(new
            {
                AllocationId = allocationId
            });

        }
    }

    public interface IAllocateInventoryArguments
    {
        public Guid OrderId { get; set; }
        public string ItemNumber { get; set; }
        public int Quantity { get; set; }

        
    }

    public interface IAllocateInventoryLog
    {
        public Guid AllocationId { get; set; }
    }
}
