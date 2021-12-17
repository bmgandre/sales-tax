using App.Models;
using Domain.Models;

namespace App.Services
{
    internal class PurchaseService
    {
        internal Receipt Process(ShoppingBasketDto shoppingBasketDto)
        {
            var items = shoppingBasketDto.BasketItems
                .SelectMany(dto =>
                {
                    return Enumerable.Range(0, dto.Amount)
                        .Select(i => new Product(name: dto.Name, price: dto.Price));
                });

            var shoppingBasket = new ShoppingBasket();
            items.ToList().ForEach(item => shoppingBasket.AddItem(item));

            var purchase = new Purchase(shoppingBasket);

            return purchase.Receipt;
        }
    }
}
