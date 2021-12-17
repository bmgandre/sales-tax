namespace Domain.Models
{
    public class Receipt
    {
        private readonly Dictionary<ReceiptItem, int> _receiptItems = new();

        public Receipt(IList<ReceiptItem> receiptItems)
        {
            receiptItems.ToList()
                .ForEach(item => AddItem(item));
        }

        private void AddItem(ReceiptItem product)
        {
            if (_receiptItems.ContainsKey(product))
            {
                _receiptItems[product] = _receiptItems[product] + 1;
            }
            else
            {
                _receiptItems[product] = 1;
            }
        }

        public IReadOnlyDictionary<ReceiptItem, int> Items => _receiptItems;

        public decimal Total => _receiptItems.Sum(item => item.Value * TotalPerItem(item.Key));

        public decimal Taxes => _receiptItems.Sum(item => item.Value * TotalRaxPerItem(item.Key));

        private static decimal TotalPerItem(ReceiptItem receiptItem)
        {
            return TotalRaxPerItem(receiptItem) + receiptItem.Product.Price;
        }

        private static decimal TotalRaxPerItem(ReceiptItem receiptItem)
        {
            return receiptItem.SalesTax + receiptItem.ImportTax;
        }
    }
}
