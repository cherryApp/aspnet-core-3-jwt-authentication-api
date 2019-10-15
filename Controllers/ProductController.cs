using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using LiteDB;

using WebApi.Entities;

namespace Ifsz.Webapi.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly ILogger<ProductController> _logger;

        public ProductController(ILogger<ProductController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<Product> Get()
        {
            _logger.LogInformation("Client has been sent a new get Request!");

            using (var db = new LiteDatabase(@".\DB.db"))
            {
                var col = db.GetCollection<Product>("products");
                return col.FindAll();
            }
        }

        /**
         * Update an existing product.
         */
        [HttpPut]
        [Route("{id}")]
        public JsonResult Put([FromBody]Product product)
        {
            using (var db = new LiteDatabase(@".\DB.db"))
            {
                var col = db.GetCollection<Product>("products");
                Product selectedProduct = col.FindById(product.Id);
                selectedProduct.Name = product.Name;
                selectedProduct.Description = product.Description;
                selectedProduct.Price = product.Price;
                selectedProduct.Active = product.Active;

                col.Update(selectedProduct);
            }

            var message = new { success = true, message = "Product updated." };
            return new JsonResult(message);
        }

        /**
         * Create a new Product.
         */
        [HttpPost]
        public JsonResult Post([FromBody]Product product)
        {

            Product newProduct = new Product
            {
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Active = product.Active,
            };

            using (var db = new LiteDatabase(@".\DB.db"))
            {
                var col = db.GetCollection<Product>("products");
                col.Insert(newProduct);
            }

            var result = new JsonResult(newProduct);
            return result;
        }

        [HttpGet]
        [Route("generate")]
        public JsonResult Generate()
        {
            String[] names = new String[] {
                "Iron", "Washing Machine", "Vacuum cleaner", 
                "Blender", "Coffee Maker", "Dishwasher",
                "Hair Dryer", "Fan", "Freezer", "Juicer",
                "Micro", "TV", "Refrigerator", "Shaver", 
                "Stove", "Toaster" 
            };

            String[] manufacturers = new String[] {
                "Ariston", "Bosch", "Zanussi", "Philips", 
                "Electrolux", "Moulinex", "AEG", "Candy", 
                "Szarvasi", "Vörös Csillag", "Custom"
            };

            Random r = new Random();

            using (var db = new LiteDatabase(@".\DB.db"))
            {
                var col = db.GetCollection<Product>("products");
                for (int i = 0; i < 10000; i++)
                {
                    Product p = new Product();
                    String name = getRandom(names);
                    String man = getRandom(manufacturers);
                    p.Name = name;
                    p.Manufacturer = man;
                    p.Price = r.Next(1000, 250000);
                    p.Description = name + " from the " + man;
                    p.Active = true;
                    col.Insert(p);
                }
            }
            
            var message = new {success = true};
            var result = new JsonResult(message);
            return result;
        }

        private String getRandom(String[] list)
        {
            Random r = new Random();
            int index = r.Next(0, list.Length);
            return list[index];
        }
    }
}

/*
var token = new JwtBuilder()
                .WithAlgorithm(new HMACSHA256Algorithm())
                .WithSecret(secret)
                .AddClaim("exp", DateTimeOffset.UtcNow.AddHours(4).ToUnixTimeSeconds())
                .AddClaim("claim2", "most secret data")
                .Build();

            var payload = new JwtBuilder()
                .WithSecret(secret)
                .MustVerifySignature()
                .Decode<IDictionary<string, object>>(token);

            var serialized = JsonConvert.SerializeObject(payload);
            _logger.LogInformation(serialized);



 */