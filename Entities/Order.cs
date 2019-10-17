using System;
using System.Collections.Generic;

namespace WebApi.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public Customer Customer { get; set; }
        public List<Product> Products { get; set; }
    }
}