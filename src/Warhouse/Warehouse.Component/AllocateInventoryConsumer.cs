using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Warehouse.Contract;

namespace Warehouse.Component
{
    public class AllocateInventoryConsumer : IConsumer<IAllocateInventory>
    {


        public async Task Consume(ConsumeContext<IAllocateInventory> context)
        {
            await Task.Delay(500);


            await context.RespondAsync<InventoryAllocated>(new
            {
                AllocationId = context.Message.AllocationId,
                ItemNumber = context.Message.ItemNumber,
                Quantity = context.Message.Quantity
            });

        }
    }
}
