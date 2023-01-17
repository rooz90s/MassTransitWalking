using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Contract
{
    public interface IOrderStatus
    {
        public Guid OrderId { get; set; }
        public string CustomerId { get; set; }
        public DateTime Updated { get; set; }
        public string Status { get; set; }
    }
}
