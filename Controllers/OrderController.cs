using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using WebApi.Services;
using WebApi.Models;
using WebApi.Entities;
using System.Linq;
using System.Collections.Generic;
using LiteDB;

namespace WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class OrderController : ControllerBase
    {
        private IUserService _userService;

        public OrderController()
        {

        }

        [HttpGet]
        public IEnumerable<Order> GetAll()
        {
            // Re-use mapper from global instance
            var mapper = BsonMapper.Global;

            // Products and Customer are other collections (not embedded document)
            // you can use [BsonRef("colname")] attribute
            mapper.Entity<Order>()
                .DbRef(x => x.Products, "products")
                .DbRef(x => x.Customer, "customers");

            using (var db = new LiteDatabase(@".\DB.db"))
            {
                var customers = db.GetCollection<Customer>("customers");
                var products = db.GetCollection<Product>("products");
                var orders = db.GetCollection<Order>("orders");

                // create examples
                var john = new Customer { Name = "John Doe", Address = "New York" };
                var tv = new Product { Name = "Television", Description = "TV Sony 44\"", Price = 799 };
                var iphone = new Product { Name = "iPhone X", Description = "iPhone X 64GB Astro Grey", Price = 999 };
                var order1 = new Order { OrderDate = new DateTime(2019, 10, 1), Customer = john, Products = new List<Product>() { iphone, tv } };
                var order2 = new Order { OrderDate = new DateTime(2019, 10, 11), Customer = john, Products = new List<Product>() { iphone } };

                // insert into collections
                customers.Insert(john);
                products.Insert(new Product[] { tv, iphone });
                orders.Insert(new Order[] { order1, order2 });

                // create index in OrderDate
                orders.EnsureIndex(x => x.OrderDate);

                var query = orders
                    .Include(x => x.Customer)
                    .Include(x => x.Products)
                    .Find(x => x.OrderDate > new DateTime(2019, 9, 29));


                return query;
            }
        }
    }
}
