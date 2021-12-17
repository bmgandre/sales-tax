using Domain.Models;
using FluentAssertions;
using System;
using Xunit;

namespace Tests.Models
{
    [Trait("Type", "Unit")]
    public class ReceiptItem_Tests
    {
        [Fact]
        public void ReceiptItem_Should_NotAcceptEmptyProduct()
        {
            Action action = () => new ReceiptItem(product: null, salesTax: 0, importTax: 0);

            action.Should().Throw<ArgumentNullException>();
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(-100)]
        public void ReceiptItem_Should_NotAcceptNegativeSalesTax(int salexTax)
        {
            Action action = () => new ReceiptItem(new Product("book", price: 1), salexTax, importTax: 0);

            action.Should().Throw<ArgumentOutOfRangeException>();
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(-100)]
        public void ReceiptItem_Should_NotAcceptNegativeImportTax(int importTax)
        {
            Action action = () => new ReceiptItem(new Product("book", 1), salesTax: 0, importTax);

            action.Should().Throw<ArgumentOutOfRangeException>();
        }
    }
}
