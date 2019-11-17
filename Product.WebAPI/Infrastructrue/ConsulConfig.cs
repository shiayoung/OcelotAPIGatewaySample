using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Product.WebAPI.Infrastructrue
{
    public class ConsulConfig
    {
        public string Address { get; set; }
        public string ServiceName { get; set; }
        public string ServiceID { get; set; }
    }
}
