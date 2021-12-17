using Domain.Models;
using FluentAssertions;
using System;
using System.Collections.Generic;
using Xunit;

namespace Tests.Models
{
    [Trait("Type", "Unit")]
    public class Purchase_Tests
    {
        [Theory]
        [MemberData(nameof(EmptyBasket))]
        public void Purchase_Should_NotAllowEmptyShoppingBasket(ShoppingBasket shoppingBasket)
        {
            Action action = () => new Purchase(shoppingBasket);

            action.Should().Throw<ArgumentOutOfRangeException>();
        }

        [Theory]
        [MemberData(nameof(BasketWithItems))]
        public void Purchase_Should_GenerateReceipt(ShoppingBasket shoppingBasket)
        {
            var purchase = new Purchase(shoppingBasket);

            var receipt = purchase.Receipt;

            receipt.Should().NotBeNull();
        }

        public static IEnumerable<object[]> BasketWithItems
        {
            get
            {
                yield return new object[]
                {
                    new ShoppingBasket()
                        .AddItem(new Product("Book", price: 12.49m))
                        .AddItem(new Product("Book", price: 12.49m))
                        .AddItem(new Product("Music CD", price: 14.99m))
                        .AddItem(new Product("Chocolate bar", price: 0.85m))
                        .AddItem(new Product("Imported box of chocolates", price: 10m))
                        .AddItem(new Product("Imported bottle of perfume", price: 47.50m))
                        .AddItem(new Product("Packet of headache pills", price: 9.75m))
                };
            }
        }

        public static IEnumerable<object[]> EmptyBasket
        {
            get
            {
                yield return new object[]
                {
                    new ShoppingBasket()
                };
            }
        }
    }
}
