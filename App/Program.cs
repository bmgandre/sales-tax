using App.Models;
using App.Services;
using System.Diagnostics;
using System.Text.RegularExpressions;

string? input;
var regex = new Regex(@"(\d+) ([\w\s]+) at (\d+\.\d+)");
var shoppingBasketDto = new ShoppingBasketDto();
var purchaseService = new PurchaseService();

while ((input = Console.ReadLine()) != null && input != "")
{
    var match = regex.Match(input);
    if (!match.Success)
    {
        throw new ArgumentException($"Invalid input: {input}");
    }

    shoppingBasketDto.BasketItems.Add(new BasketItem()
    {
        Amount = int.Parse(match.Groups[1].Value),
        Name = match.Groups[2].Value,
        Price = decimal.Parse(match.Groups[3].Value)
    });
}

var receipt = purchaseService.Process(shoppingBasketDto);

new ReceiptPrinterService(receipt)
    .Print(Console.Out);

// profiles.App.commandLineArgs includes input rediction,
// forcing a break point to keep the console opened
// on debug sessions
if (Debugger.IsAttached)
{
    Debugger.Break();
}
