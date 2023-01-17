using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Warehouse.Contract
{
    public interface IAllocationReleaseRequested
    {
        public string AllocationId { get; set; }
        public string Reason { get; set; }
    }
}
