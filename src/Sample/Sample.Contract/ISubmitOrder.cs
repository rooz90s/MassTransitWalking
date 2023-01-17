using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Contract
{
    public interface ISubmitOrder
    {
        public Guid OrderId { get; set; }
        public DateTime Timestamp { get; set; }
        public string CustomerNumber { get; set; }
    }
}
