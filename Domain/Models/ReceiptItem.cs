namespace Domain.Models
{
    public class ReceiptItem : IEquatable<ReceiptItem>
    {
        public ReceiptItem(Product product,
            decimal salesTax, decimal importTax)
        {
            if (product is null)
            {
                throw new ArgumentNullException(nameof(product), "Product cannot be null");
            }

            if (salesTax < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(salesTax), "Sales tax must be greater than or equal to 0");
            }

            if (importTax < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(importTax), "Import tax must be greater than or equal to 0");
            }

            Product = product;
            SalesTax = salesTax;
            ImportTax = importTax;
        }

        public Product Product { get; }

        public decimal SalesTax { get; }

        public decimal ImportTax { get; }

        public bool Equals(ReceiptItem? other)
        {
            if (other == null)
            {
                return false;
            }

            return Product.Equals(other.Product)
                && SalesTax == other.SalesTax
                && ImportTax == other.ImportTax;
        }

        public override bool Equals(object? obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            return Equals(obj as ReceiptItem);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Product);
        }
    }
}
