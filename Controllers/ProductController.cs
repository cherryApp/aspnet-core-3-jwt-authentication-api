using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using LiteDB;
using JWT.Algorithms;
using JWT.Builder;
using Newtonsoft.Json;

using Ifsz.Webapi.Server.Models;

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