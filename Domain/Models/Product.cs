namespace Domain.Models
{
    public class Product : IEquatable<Product>
    {
        public Product(string name, decimal price)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Product name cannot be empty", nameof(name));
            }

            if (price < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(price), "Price must be greater than or equal to zero");
            }

            Name = name;
            Price = price;
        }

        public string Name { get; }

        public decimal Price { get; }

        public bool Equals(Product? other)
        {
            if (other == null)
            {
                return false;
            }

            return Name.Equals(other.Name);
        }

        public override bool Equals(object? obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            return Equals(obj as Product);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name);
        }
    }
}
