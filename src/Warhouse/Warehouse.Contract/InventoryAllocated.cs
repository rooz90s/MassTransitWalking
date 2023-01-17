using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Warehouse.Contract
{
    public interface InventoryAllocated
    {
        public Guid AllocationId { get; set; }
        public int ItemNumber { get; set; }
        public int Quantity { get; set; }
    }
}
