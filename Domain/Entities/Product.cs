namespace SupplyForge.Domain.Entities
{
    public sealed class Product
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public decimal Price { get; private set; }
        public decimal Weight { get; private set; }

        private Product()
        {

        }
        public Product(string name, string description, decimal price, decimal weight)
        {
            if(string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Product name cannot be null or empty.", nameof(name));
            }

            if(string.IsNullOrWhiteSpace(description)) {
                throw new ArgumentException("Product description cannot be null or empty.", nameof(description));
            }

            if(price < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(price), "Product price cannot be negative.");
            }

            if(weight < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(weight), "Product weight cannot be zero or negative.");
            }

            Id = Guid.NewGuid();
            Name = name;
            Description = description;
            Price = price;
            Weight = weight;
        }


    }
}
