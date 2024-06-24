using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Khumalo_durableFunction
{
    public class Order
    {
        public string Id { get; set; }
        public int Quantity { get; set; }
        public string CustomerEmail { get; set; }
    }

}
