using Domain.Models;
using FluentAssertions;
using System;
using Xunit;

namespace Tests.Models
{
    [Trait("Type", "Unit")]
    public class Product_Tests
    {
        [Theory]
        [InlineData("Book")]
        [InlineData("Music CD")]
        public void Product_Should_BeNamed(string name)
        {
            Action action = () => new Product(name, 0);

            action.Should().NotThrow();
        }

        [Theory]
        [InlineData(10)]
        [InlineData(1)]
        [InlineData(0)]
        public void Product_Should_BePriced(int price)
        {
            Action action = () => new Product("name", price);

            action.Should().NotThrow();
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Product_Should_NotAllowEmptyName(string? name)
        {
            Action action = () => new Product(name, 0);

            action.Should().Throw<ArgumentException>();
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(-100)]
        public void Product_Should_NotAllowNegativePrice(int price)
        {
            Action action = () => new Product("name", price);

            action.Should().Throw<ArgumentOutOfRangeException>();
        }
    }
}
