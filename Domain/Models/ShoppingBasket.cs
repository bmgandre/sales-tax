namespace Domain.Models
{
    public class ShoppingBasket
    {
        private readonly List<Product> _products = new();

        public ShoppingBasket AddItem(Product product)
        {
            _products.Add(product);
            return this;
        }

        public IReadOnlyList<Product> Items => _products;
    }
}
