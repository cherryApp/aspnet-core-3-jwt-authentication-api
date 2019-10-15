using System;

namespace WebApi.Entities {
    public class Product {

        public int Id { get; set; }

        public string Name { get; set; }

        public int Price { get; set; }

        public string Description { get; set; }

        public string Manufacturer { get; set; }

        public Boolean Active { get; set; }
        
    }
}