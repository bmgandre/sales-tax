using Domain.Models;
using FluentAssertions;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Tests.Models
{
    [Trait("Type", "Unit")]
    public class ShoppingBasket_Tests
    {
        [Theory]
        [MemberData(nameof(Products))]
        public void ShoppingBasket_Should_AccumulateItems(IEnumerable<Product> products,
            int amount)
        {
            // Arrange
            var basket = new ShoppingBasket();

            // Act
            products.ToList().ForEach(p => basket.AddItem(p));

            // Assert
            basket.Items.Count.Should().Be(amount);
        }

        public static IEnumerable<object[]> Products
        {
            get
            {
                yield return new object[]
                {
                    new List<Product>
                    { 
                        new Product("Book", price: 1),
                        new Product("Book", price: 1),
                        new Product("Music CD", price: 2) 
                    },
                    3 // amount
                };
            }
        }
    }
}
