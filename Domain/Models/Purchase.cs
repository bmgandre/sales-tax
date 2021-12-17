namespace Domain.Models
{
    public class Purchase
    {
        private readonly Receipt _receipt;

        public Purchase(ShoppingBasket shoppingBasket)
        {
            if (shoppingBasket is null || shoppingBasket.Items.Count <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(shoppingBasket), "Shopping basket must contain items");
            }

            var receiptItems = shoppingBasket.Items
                .Select(SellProduct)
                .ToList();

            _receipt = new Receipt(receiptItems);
        }

        public Receipt Receipt => _receipt;

        private ReceiptItem SellProduct(Product product)
        {
            return new ReceiptItem(product, ComputeSalesTax(product), ComputeImportTax(product));
        }

        private decimal ComputeSalesTax(Product product)
        {
            if (IsSalesTaxApplicable(product))
            {
                var baseTax = product.Price * .1m;
                var salexTax = Math.Ceiling(baseTax * 20) / 20;
                return salexTax;
            }

            return 0m;
        }

        private decimal ComputeImportTax(Product product)
        {
            if (IsImportTaxApplicable(product))
            {
                var baseTax = product.Price * .05m;
                var importTax = Math.Ceiling(baseTax * 20) / 20;
                return importTax;
            }

            return 0m;
        }

        private bool IsSalesTaxApplicable(Product product)
        {
            switch (product.Name)
            {
                case var name when IsFood(name) || IsBook(name) || IsMedicalProduct(name):
                    return false;
                default:
                    return true;
            }
        }

        private bool IsFood(string name)
        {
            return Foods.Any(food => name.Contains(food, StringComparison.OrdinalIgnoreCase));
        }

        private bool IsBook(string name)
        {
            return Books.Any(book => name.Contains(book, StringComparison.OrdinalIgnoreCase));
        }

        private bool IsMedicalProduct(string name)
        {
            return MedicalProducts.Any(medicalProduct => name.Contains(medicalProduct, StringComparison.OrdinalIgnoreCase));
        }

        private bool IsImportTaxApplicable(Product product)
        {
            return ImportedProducts.Any(importedProduct => product.Name.Contains(importedProduct, StringComparison.OrdinalIgnoreCase));
        }

        private readonly List<string> Foods = new()
        {
            "chocolate",
        };

        private readonly List<string> Books = new()
        {
            "book",
        };

        private readonly List<string> MedicalProducts = new()
        {
            "pill",
        };

        private readonly List<string> ImportedProducts = new()
        {
            "imported",
        };
    }
}
