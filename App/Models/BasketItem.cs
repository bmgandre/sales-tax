namespace App.Models
{
    internal class BasketItem
    {
        public int Amount { get; set; } = 0;

        public string Name { get; set; } = string.Empty;

        public decimal Price { get; set; } = 0m;
    }
}
