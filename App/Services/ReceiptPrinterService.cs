using Domain.Models;

namespace App.Services
{
    internal class ReceiptPrinterService
    {
        private readonly Receipt _receipt;

        public ReceiptPrinterService(Receipt receipt)
        {
            _receipt = receipt;
        }

        public void Print(TextWriter textWriter)
        {
            _receipt.Items
                .ToList()
                .ForEach(item => PrintItem(textWriter, item.Key, item.Value));
            textWriter.WriteLine($"Sales Taxes: {_receipt.Taxes:0.00}");
            textWriter.WriteLine($"Total: {_receipt.Total:0.00}");
        }

        private void PrintItem(TextWriter textWriter, ReceiptItem item, int amount)
        {
            var price = item.Product.Price + item.SalesTax + item.ImportTax;

            if (amount > 1)
            {
                textWriter.WriteLine($"{item.Product.Name}: {price * amount:0.00} ({amount} @ {price:0.00})");
            }
            else
            {
                textWriter.WriteLine($"{item.Product.Name}: {price:0.00}");
            }
        }
    }
}
