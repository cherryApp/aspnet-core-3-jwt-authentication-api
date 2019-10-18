using System;

namespace WebApi.Entities
{
    public class Product
    {

        public int Id { get; set; }

        public string Name { get; set; }
        public ValidationResult validateName() 
        {
            if (Name == "test") {
                return new ValidationResult {
                    valid = false, 
                    code = 100, 
                    message = "Name must be minimum 4 character"
                };
            }
            return new ValidationResult {valid = true, code = 0, message = "ok"};
        }

        public int Price { get; set; }

        public string Description { get; set; }

        public string Manufacturer { get; set; }
        public string ItemCode { get; set; }

        public Boolean Active { get; set; }

    }

    interface IValidator<T> {
        bool Validate(string key, T t);
    }

    class ProductValidator : IValidator<Product> {
        public bool Validate(string key, Product t)
        {
            return true;
        }
    }
}