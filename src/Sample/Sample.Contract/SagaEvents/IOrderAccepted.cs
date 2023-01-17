using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Contract.SagaEvents
{
    public interface IOrderAccepted
    {
        public Guid OrderId { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
