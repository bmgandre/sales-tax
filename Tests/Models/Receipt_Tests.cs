using Domain.Models;
using FluentAssertions;
using System.Collections.Generic;
using Xunit;

namespace Tests.Models
{
    [Trait("Type", "Unit")]
    public class Receipt_Tests
    {

        [Theory]
        [MemberData(nameof(BasketWithRepeatedItems))]
        public void Receipt_Should_GroupItems(ShoppingBasket shoppingBasket, int expectedItems)
        {
            var purchase = new Purchase(shoppingBasket);

            var receipt = purchase.Receipt;

            receipt.Items.Should().HaveCount(expectedItems);
        }

        [Theory]
        [MemberData(nameof(BasketWithItemsFreeOfSalesTax))]
        public void Receipt_Should_NotIncludeSalesTaxForSomeItems(ShoppingBasket shoppingBasket,
            List<ReceiptItem> receiptItems)
        {
            var purchase = new Purchase(shoppingBasket);

            var receipt = purchase.Receipt;

            receipt.Items.Keys.Should().Contain(receiptItems);
        }

        [Theory]
        [MemberData(nameof(BasketWithItemsWithSalesTax))]
        public void Receipt_Should_IncludeSalesTaxForSomeItemsRoundingToTheNearestFiveCents(ShoppingBasket shoppingBasket,
            List<ReceiptItem> receiptItems)
        {
            var purchase = new Purchase(shoppingBasket);

            var receipt = purchase.Receipt;

            receipt.Items.Keys.Should().Contain(receiptItems);
        }

        [Theory]
        [MemberData(nameof(BasketWithNoImportTaxApplicable))]
        public void Receipt_Should_NotIncludeImportTaxForSomeItems(ShoppingBasket shoppingBasket,
            List<ReceiptItem> receiptItems)
        {
            var purchase = new Purchase(shoppingBasket);

            var receipt = purchase.Receipt;

            receipt.Items.Keys.Should().Contain(receiptItems);
        }

        [Theory]
        [MemberData(nameof(BasketWithImportTaxApplicable))]
        public void Receipt_Should_IncludeImportTaxForSomeItemsRoundingToTheNearestFiveCents(ShoppingBasket shoppingBasket,
            List<ReceiptItem> receiptItems)
        {
            var purchase = new Purchase(shoppingBasket);

            var receipt = purchase.Receipt;

            receipt.Items.Keys.Should().Contain(receiptItems);
        }

        [Theory]
        [MemberData(nameof(BasketWithExpectedReceipts))]
        public void Receipt_Should_SummarizePriceAndTaxes(ShoppingBasket shoppingBasket,
            Receipt expectedReceipt, decimal expectedSalesTax, decimal expectedTotal)
        {
            var purchase = new Purchase(shoppingBasket);

            var receipt = purchase.Receipt;

            receipt.Should().BeEquivalentTo(expectedReceipt);
            receipt.Taxes.Should().Be(expectedSalesTax);
            receipt.Total.Should().Be(expectedTotal);
        }

        public static IEnumerable<object[]> BasketWithRepeatedItems
        {
            get
            {
                yield return new object[]
                {
                    new ShoppingBasket()
                        .AddItem(new Product("Book", price: 12.49m))
                        .AddItem(new Product("Book", price: 12.49m))
                        .AddItem(new Product("Music CD", price: 14.99m)),
                    2
                };
                yield return new object[]
                {
                    new ShoppingBasket()
                        .AddItem(new Product("Book", price: 12.49m))
                        .AddItem(new Product("Music CD", price: 14.99m)),
                    2
                };
            }
        }

        public static IEnumerable<object[]> BasketWithItemsFreeOfSalesTax
        {
            get
            {
                yield return new object[]
                {
                    new ShoppingBasket()
                        .AddItem(new Product("Book", price: 12.49m)),
                    new List<ReceiptItem>
                    {
                        { new ReceiptItem(new Product("Book", price: 12.49m), salesTax: 0, 0) }
                    }
                };
            }
        }

        public static IEnumerable<object[]> BasketWithItemsWithSalesTax
        {
            get
            {
                yield return new object[]
                {
                    new ShoppingBasket()
                        .AddItem(new Product("Music CD", price: 5.6m)),
                    new List<ReceiptItem>
                    {
                        { new ReceiptItem(new Product("Music CD", price: 5.6m), salesTax: .6m, 0) }
                    }
                };
            }
        }

        public static IEnumerable<object[]> BasketWithNoImportTaxApplicable
        {
            get
            {
                yield return new object[]
                {
                    new ShoppingBasket()
                        .AddItem(new Product("Book", price: 12.49m)),
                    new List<ReceiptItem>
                    {
                        { new ReceiptItem(new Product("Book", price: 12.49m), 0, 0) }
                    }
                };
            }
        }

        public static IEnumerable<object[]> BasketWithImportTaxApplicable
        {
            get
            {
                yield return new object[]
                {
                    new ShoppingBasket()
                        .AddItem(new Product("Imported box of chocolates", price: 10m))
                        .AddItem(new Product("Imported bottle of perfume", price: 47.50m)),
                    new List<ReceiptItem>
                    {
                        { new ReceiptItem(new Product("Imported box of chocolates", price: 10m), salesTax: 0, .5m) },
                        { new ReceiptItem(new Product("Imported bottle of perfume", price: 47.50m), salesTax: 4.75m, 2.4m) },
                    }
                };
            }
        }

        public static IEnumerable<object[]> BasketWithExpectedReceipts
        {
            get
            {
                // INPUT 1
                yield return new object[]
                {
                    new ShoppingBasket()
                        .AddItem(new Product("Book", price: 12.49m))
                        .AddItem(new Product("Book", price: 12.49m))
                        .AddItem(new Product("Music CD", price: 14.99m))
                        .AddItem(new Product("Chocolate bar", price: 0.85m)),
                    new Receipt(new List<ReceiptItem>
                    {
                        new ReceiptItem(new Product("Book", price: 12.49m), salesTax: 0, importTax: 0),
                        new ReceiptItem(new Product("Book", price: 12.49m), salesTax: 0, importTax: 0),
                        new ReceiptItem(new Product("Music CD", price: 14.99m), salesTax: 1.5m, importTax: 0),
                        new ReceiptItem(new Product("Chocolate bar", price: 0.85m), salesTax: 0, importTax: 0),
                    }),
                    1.5m,  // sales tax
                    42.32m // total
                };

                // INPUT 2
                yield return new object[]
                {
                    new ShoppingBasket()
                        .AddItem(new Product("Imported box of chocolates", price: 10m))
                        .AddItem(new Product("Imported bottle of perfume", price: 47.5m)),
                    new Receipt(new List<ReceiptItem>
                    {
                        new ReceiptItem(new Product("Imported box of chocolates", price: 10m), salesTax: 0, importTax: .5m),
                        new ReceiptItem(new Product("Imported bottle of perfume", price: 47.5m), salesTax: 4.75m, importTax: 2.4m),
                    }),
                    7.65m, // sales tax
                    65.15m // total
                };

                // INPUT 3
                yield return new object[]
                {
                    new ShoppingBasket()
                        .AddItem(new Product("Imported bottle of perfume", price: 27.99m))
                        .AddItem(new Product("Bottle of perfume", price: 18.99m))
                        .AddItem(new Product("Packet of headache pills", price: 9.75m))
                        .AddItem(new Product("Imported box of chocolates", price: 11.25m))
                        .AddItem(new Product("Imported box of chocolates", price: 11.25m)),
                    new Receipt(new List<ReceiptItem>
                    {
                        new ReceiptItem(new Product("Imported bottle of perfume", price: 27.99m), salesTax: 2.8m, importTax: 1.4m),
                        new ReceiptItem(new Product("Bottle of perfume", price: 18.99m), salesTax: 1.9m, importTax: 0),
                        new ReceiptItem(new Product("Packet of headache pills", price: 9.75m), salesTax: 0, importTax: 0),
                        new ReceiptItem(new Product("Imported box of chocolates", price: 11.25m), salesTax: 0, importTax: .6m),
                        new ReceiptItem(new Product("Imported box of chocolates", price: 11.25m), salesTax: 0, importTax: .6m),
                    }),
                    7.3m,  // sales tax
                    86.53m // total
                };
            }
        }
    }
}
